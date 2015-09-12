using UnityEngine;
using System.Collections;

public class FageAudioManagerEx : FageStateMachine {
	private	static FageAudioManagerEx	_instance;
	public	static FageAudioManagerEx	Instance { get { return _instance; } }

	private	FageAudioRoot _audioRoot;

	void Awake() {
		_instance = this;
		_audioRoot = FageAudioRoot.Instance;
		AudioListener listener = (new GameObject ("AudioChannels", typeof(AudioListener))).GetComponent<AudioListener> ();
		listener.transform.SetParent (transform);
		_audioRoot.GenerateControl (listener);
	}

	public	void Play(string nodeId, string clip, ref FageAudioSourceControl audioSourceControl, bool loop = false) {
		FageAudioXML audioXML = _audioRoot.Find (nodeId);
		AudioClip ac = FageBundleLoader.Instance.Load (clip) as AudioClip;
		if (ac != null) {
			audioSourceControl = audioXML.GetControl ();
			audioSourceControl.Play (ac, loop, audioXML.GetVolume(), true);
		}
	}
	
	public	void Play(string nodeId, string clip, bool loop = false) {
		FageAudioXML audioXML = _audioRoot.Find (nodeId);
		AudioClip ac = FageBundleLoader.Instance.Load (clip) as AudioClip;
		if (ac != null) {
			audioXML.GetControl ().Play(ac, loop, audioXML.GetVolume(), false);
		}
	}
	
	public	void SetVolume(string nodeId, float volume) {
		FageAudioXML audioXML = _audioRoot.Find (nodeId);
		audioXML.SetVolume (volume);
	}
}
