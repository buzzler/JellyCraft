using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FageBundleLoaderLoad : FageState {
	private	Queue queueBundle;
	private	Queue queueAsset;
	private AssetBundleRequest request;

	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
		List<string> resources = new List<string>();
		List<string> loadBundles = new List<string>();
		List<string> unloadBundles = new List<string>();

		FageBundleLoader loader = stateMachine as FageBundleLoader;
		Dictionary<string, AssetBundle> downloadedBundles = loader.GetDownloadedBundles();
		Dictionary<string, object> loadedAsset = loader.GetLoadedAssets();
		List<string> loadedBundles = loader.GetLoadedBundles();

		FageUIManager manager = FageUIManager.Instance;
		Stack stack = manager.GetStack();
		Queue queue = manager.GetQueue();
		if (stack.Count > 0) {
			FageUIMem mem = stack.Peek() as FageUIMem;
			foreach (string s in mem.uiSet.GetBundles()) {
				if (!resources.Contains(s))
					resources.Add(s);
			}
		}
		if (queue.Count > 0) {
			FageUIPopupMem mem = queue.Peek() as FageUIPopupMem;
			foreach (string s in mem.uiSet.GetBundles()) {
				if (!resources.Contains(s))
					resources.Add(s);
			}
		}
		foreach (FageBundle bundle in FageBundleRoot.Instance.bundles) {
			if ((!bundle.isDynamic) && (!loadBundles.Contains(bundle.id))) {
				loadBundles.Add(bundle.id);
			} else if ((resources.Contains(bundle.id)) && (!loadBundles.Contains(bundle.id))) {
				loadBundles.Add(bundle.id);
			}
		}

		foreach (string id in loadedBundles) {
			if (loadBundles.Contains(id)) {
				loadBundles.Remove(id);
			} else if (!unloadBundles.Contains(id)) {
				unloadBundles.Add(id);
			}
		}

		queueBundle = new Queue();
		foreach (string s in loadBundles) {
			AssetBundle ab = downloadedBundles[s];
			ab.name = s;
			queueBundle.Enqueue(ab);
		}

		foreach (string s in unloadBundles) {
			AssetBundle ab = downloadedBundles[s];
			foreach (string abname in ab.GetAllAssetNames()) {
				loadedAsset.Remove(abname);
			}
			loadedBundles.Remove(s);
		}
		if (queueBundle.Count > 0) {
			AssetBundle ab = queueBundle.Peek () as AssetBundle;
			queueAsset = new Queue (ab.GetAllAssetNames ());
		}

		stateMachine.DispatchEvent (new FageBundleEvent(FageBundleEvent.LOADING));
	}

	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		FageBundleLoader loader = stateMachine as FageBundleLoader;

		if (request != null) {
			if (request.isDone) {
				string name = queueAsset.Dequeue() as string;
				loader.GetLoadedAssets().Add(name, request.asset);
				request = null;
			}
		} else {
			if ((queueAsset == null) || (queueAsset.Count == 0)) {
				if (queueBundle.Count > 0) {
					AssetBundle ab = queueBundle.Dequeue() as AssetBundle;
					loader.GetLoadedBundles().Add(ab.name);
				}
				
				if ((queueBundle == null) || (queueBundle.Count == 0)) {
					loader.ReserveState("FageBundleLoaderIdle");
					loader.SetUpdateTime();
					loader.DispatchEvent(new FageEvent(FageEvent.COMPLETE));
				} else {
					AssetBundle ab = queueBundle.Peek() as AssetBundle;
					queueAsset = new Queue(ab.GetAllAssetNames());
				}
			} else {
				AssetBundle ab = queueBundle.Peek() as AssetBundle;
				string name = queueAsset.Peek() as string;
				request = ab.LoadAssetAsync(name);
			}
		}
	}
}
