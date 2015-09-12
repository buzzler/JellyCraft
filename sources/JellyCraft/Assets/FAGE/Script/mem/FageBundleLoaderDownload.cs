using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FageBundleLoaderDownload : FageState {
	private	float		_total;
	private	Hashtable	_hash;

	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
		FageWebLoader loader = FageWebLoader.Instance;
		loader.AddEventListener(FageEvent.COMPLETE, OnResponse);
		loader.AddEventListener (FageWebEvent.PROGRESS, OnProgress);
		_hash = new Hashtable();
		foreach (FageBundle bundle in FageBundleRoot.Instance.bundles) {
#if UNITY_EDITOR
			_hash.Add(loader.Request(bundle.ios, bundle.version), bundle);
#elif UNITY_IOS
			_hash.Add(loader.Request(bundle.ios, bundle.version), bundle);
#elif UNITY_ANDROID
			_hash.Add(loader.Request(bundle.android, bundle.version), bundle);
#endif
		}
		_total = (float)_hash.Count;
		stateMachine.DispatchEvent (new FageBundleEvent(FageBundleEvent.DOWNLOADING));
	}

	private void OnProgress(FageEvent fevent) {
		FageWebEvent wevent = fevent as FageWebEvent;
		if ((wevent == null) || (!_hash.ContainsKey(wevent.requestId)))
			return;

		float seg = 1f / _total;
		float p = (_total - _hash.Count) * seg + wevent.progress * seg;

		FageBundleLoader.Instance.DispatchEvent (new FageBundleEvent(FageBundleEvent.DOWNLOADING, p));
	}

	private	void OnResponse(FageEvent fevent) {
		FageWebEvent wevent = fevent as FageWebEvent;
		if ((wevent == null) || (!_hash.ContainsKey(wevent.requestId)))
			return;

		if (string.IsNullOrEmpty(wevent.www.error)) {
			FageBundle bundle = _hash[wevent.requestId] as FageBundle;
			Dictionary<string, AssetBundle> bundles = FageBundleLoader.Instance.GetDownloadedBundles();
			if (bundles.ContainsKey(bundle.id))
				bundles[bundle.id] = wevent.www.assetBundle;
			else
				bundles.Add(bundle.id, wevent.www.assetBundle);
			List<string> scenes = FageBundleLoader.Instance.GetLoadedScene();
			string[] paths = wevent.www.assetBundle.GetAllScenePaths();
			for (int i = 0 ; i < paths.Length ; i++) {
				if (!scenes.Contains(paths[i])) {
					scenes.Add(paths[i]);
				}
			}

			wevent.www.Dispose();
		} else {
			FageBundleLoader.Instance.DispatchEvent (new FageBundleEvent(FageBundleEvent.ERROR_NODATA));
			FageBundleLoader.Instance.ReserveState ("FageBundleLoaderIdle");
			_hash.Clear();
			return;
		}

		_hash.Remove(wevent.requestId);
		if (_hash.Count == 0) {
			FageBundleLoader.Instance.DispatchEvent (new FageBundleEvent(FageBundleEvent.DOWNLOADING, 1f));
			FageBundleLoader.Instance.ReserveState ("FageBundleLoaderLoad");
		}
	}

	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
	}

	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId) {
		base.BeforeSwitch (stateMachine, afterId);
		_hash = null;
		FageWebLoader.Instance.RemoveEventListener (FageEvent.COMPLETE, OnResponse);
		FageWebLoader.Instance.RemoveEventListener (FageWebEvent.PROGRESS, OnProgress);
	}
}