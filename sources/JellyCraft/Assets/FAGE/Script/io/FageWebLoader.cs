using UnityEngine;
using System.Collections;

[AddComponentMenu("Fage/Net/FageWebLoader")]
public class FageWebLoader : FageEventDispatcher {
	private	static FageWebLoader _instance;
	private	static int _countId;
	public	static FageWebLoader Instance { get { return _instance; } }

	private	WWW		_www;
	private	bool	_excute;
	private	Queue	_queue;

	void Awake() {
		_instance	= this;
		_countId	= 0;
		_www		= null;
		_excute		= FageConnectionManager.Instance.IsOnline();
		_queue		= new Queue ();
	}

	void OnEnable() {
		FageConnectionManager.Instance.AddEventListener (FageEvent.SENSOR_ONLINE,	OnOnline);
		FageConnectionManager.Instance.AddEventListener (FageEvent.SENSOR_OFFLINE,	OnOffline);
	}

	void OnDisable() {
		FageConnectionManager.Instance.RemoveEventListener (FageEvent.SENSOR_ONLINE,	OnOnline);
		FageConnectionManager.Instance.RemoveEventListener (FageEvent.SENSOR_OFFLINE,	OnOffline);
	}

	void Update() {
		if (_www != null) {
			if (_www.isDone) {
				FageWebState current = _queue.Dequeue() as FageWebState;
				DispatchEvent (new FageWebEvent (FageEvent.COMPLETE, current.requestId, _www));
				_www = null;
			} else {
				FageWebState current = _queue.Peek() as FageWebState;
				DispatchEvent (new FageWebEvent (FageWebEvent.PROGRESS, current.requestId, _www.progress));
			}
		} else if (_queue.Count > 0) {
			FageWebState current = _queue.Peek() as FageWebState;
			if (_excute) {
				if (current.version < 0) {
					if (current.wwwForm != null) {
						_www = new WWW (current.url, current.wwwForm);
					} else {
						_www = new WWW (current.url);
					}
				} else {
					_www = WWW.LoadFromCacheOrDownload(current.url, current.version);
				}
			} else if (Caching.IsVersionCached(current.url, current.version)) {
				_www = WWW.LoadFromCacheOrDownload(current.url, current.version);
			}
		}
	}

	private	void OnOnline(FageEvent fevent) {
		_excute = true;
	}

	private void OnOffline(FageEvent fevent) {
		_excute = false;
		if ((_www != null) && (_queue.Count > 0)) {
			_www = null;
		}
	}

	public	int Request(string url, WWWForm wwwForm = null) {
		_queue.Enqueue (new FageWebState(++_countId, url, -1, wwwForm));
		return _countId;
	}

	public	int Request(string url, int version) {
		_queue.Enqueue (new FageWebState(++_countId, url, version));
		return _countId;
	}
}
