using UnityEngine;
using System.Collections;

public	delegate void FageAudioEventHandler(FageAudioSourceControl control);

[RequireComponent(typeof(AudioSource))]
public	class FageAudioSourceControl : FageStateMachine {
	public	AudioSource					audioSource;
	public	FageAudioStatus				audioStatus { get { return _audioStatus; } }
	public	event FageAudioEventHandler	onStatus;
	public	event FageAudioEventHandler	onLoop;
	private	FageAudioStatus				_audioStatus;

	void Awake() {
		audioSource = GetComponent<AudioSource>();
		ReserveState("FageAudioSourceReady");
	}

	public	void SetAudioStatus(FageAudioStatus status) {
		if (_audioStatus != status) {
			_audioStatus = status;
			if (onStatus!=null) {
				onStatus(this);
			}
		}
	}

	public	void NotifyLoop() {
		if (onLoop!=null) {
			onLoop(this);
		}
	}

	public	void Play() {
		audioSource.Play ();
	}

	public	void Play(AudioClip clip, bool loop, float volumn, bool control) {
		audioSource.clip = clip;
		audioSource.loop = loop;
		audioSource.volume = volumn;
		enabled = control;

		audioSource.Play ();
	}

	public	void Stop() {
		audioSource.Stop ();
	}

	public	void Pause() {
		audioSource.Pause ();
	}

	public	void UnPause() {
		audioSource.UnPause ();
	}

	public	void UnPause(float volumn) {
		audioSource.volume = volumn;
		audioSource.UnPause ();
	}

	public	float volume {
		get {
			return audioSource.volume;
		}
		set {
			audioSource.volume = value;
		}
	}
}
