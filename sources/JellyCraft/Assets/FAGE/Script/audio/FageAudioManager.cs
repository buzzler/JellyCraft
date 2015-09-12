using UnityEngine;
using System.Collections;

[AddComponentMenu("Fage/Audio/FageAudioManager")]
public class FageAudioManager : FageEventDispatcher {
	private	static FageAudioManager _instance;
	public	static FageAudioManager Instance { get { return _instance; } }
	public	FageAudioNode[]		nodes;
	private	Hashtable			_hashtable;
	private	GameObject			_listener;

	void Awake() {
		_instance = this;
		_hashtable = new Hashtable ();
		_listener = new GameObject("AudioChannels", typeof(AudioListener));
		_listener.transform.SetParent (transform);
		foreach (FageAudioNode node in nodes) {
			node.Align ();
			_hashtable.Add(node, new FageAudioPooler(node, _listener)); 
		}
	}

	public	void Play(string nodeId, string resourcePath, ref FageAudioSourceControl audioSourceControl, bool loop = false) {
		FageAudioNode node = FageAudioNode.Find(nodeId);
		audioSourceControl = (_hashtable[node] as FageAudioPooler).GetFreeAudioSourceControl();
		audioSourceControl.Play(CachedResource.Load<AudioClip>(resourcePath), loop, node.GetVolume(), true);
	}

	public	void Play(string nodeId, string resourcePath, bool loop = false) {
		FageAudioNode node = FageAudioNode.Find(nodeId);
		FageAudioSourceControl audioSourceControl = (_hashtable[node] as FageAudioPooler).GetFreeAudioSourceControl();
		audioSourceControl.Play(CachedResource.Load<AudioClip>(resourcePath), loop, node.GetVolume(), false);
	}

	public	void SetVolume(string nodeId, float volume) {
		FageAudioNode node = FageAudioNode.Find(nodeId);
		node.volumn = Mathf.Clamp(volume, 0f, 1f);
		float globalVolume = node.GetVolume();
		FageAudioSourceControl[] controls = (_hashtable[node] as FageAudioPooler).GetAudioSourceControls();
		foreach (FageAudioSourceControl control in controls) {
			control.volume = globalVolume;
		}
	}
}

