using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FageBundleLoader : FageStateMachine {
	private	static FageBundleLoader _instance;
	public	static FageBundleLoader Instance { get { return _instance; } }

	public	TextAsset					setting;
	public	bool						flagUpdate;
	public	bool						flagLoad;
	public	bool						flagDownload;
	public	float						expireTime;
	private	float						_timeLastUpdate;
	private	List<string>				_loadedScene;
	private	List<string>				_loadedBundle;
	private	Dictionary<string, object>	_loadedAsset;
	private	Dictionary<string, AssetBundle>	_downloadedBundle;

	void Awake() {
		_instance = this;
		_timeLastUpdate = Time.unscaledTime;
		_loadedScene = new List<string> ();
		_loadedBundle = new List<string> ();
		_loadedAsset = new Dictionary<string, object> ();
		_downloadedBundle = new Dictionary<string, AssetBundle> ();
		FageConfig.LoadFromText(setting.text);
	}

	public	void ReserveUpdate() {
		flagUpdate = true;
	}
	public	void ReserveLoad() {
		flagLoad = true;
	}

	public	void ReserveDownload() {
		flagDownload = true;
	}

	public	float GetUpdateTime() {
		return _timeLastUpdate;
	}

	public	void SetUpdateTime() {
		flagLoad = false;
		_timeLastUpdate = Time.unscaledTime;
	}
	
	public	List<string> GetLoadedScene() {
		return _loadedScene;
	}

	public	List<string> GetLoadedBundles() {
		return _loadedBundle;
	}

	public	Dictionary<string, object> GetLoadedAssets() {
		return _loadedAsset;
	}

	public	Dictionary<string, AssetBundle> GetDownloadedBundles() {
		return _downloadedBundle;
	}

	public	AsyncOperation LoadLevel(string asset) {
		if (!_loadedScene.Contains (asset))
			return Application.LoadLevelAsync (asset);
#if UNITY_EDITOR
		return UnityEditor.EditorApplication.LoadLevelAsyncInPlayMode(asset);
#else
		if (_loadedScene.Contains (asset))
			return Application.LoadLevelAsync (asset);
		else
			throw new UnityException ("unknown scene asset : " + asset);
#endif
	}

	public	AsyncOperation LoadLevelAdditive(string asset) {
		if (!_loadedScene.Contains (asset))
			return Application.LoadLevelAdditiveAsync (asset);
#if UNITY_EDITOR
		return UnityEditor.EditorApplication.LoadLevelAdditiveAsyncInPlayMode(asset);
#else
		if (_loadedScene.Contains (asset))
			return Application.LoadLevelAdditiveAsync (asset);
		else
			throw new UnityException ("unknown scene asset : " + asset);
#endif
	}

	public	object Load(string id) {
		if (_loadedAsset.ContainsKey (id))
			return _loadedAsset [id];
		else
			return Resources.Load (id);
	}

	public	object Load(FageUIDetail uiDetail) {
		return Load (uiDetail.asset, uiDetail.bundle);
	}

	public	object Load(FageUICurtain uiCurtain) {
		return Load (uiCurtain.asset, uiCurtain.bundle);
	}

	public	object Load(string asset, string bundle) {
		if (_loadedAsset.ContainsKey (asset)) {
			return _loadedAsset [asset];
		} else if (_downloadedBundle.ContainsKey (bundle)) {
			return _downloadedBundle [bundle].LoadAsset (asset);
		} else {
			return Resources.Load (asset);
		}
	}
}
