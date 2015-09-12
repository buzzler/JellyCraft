using UnityEngine;
using System.Collections.Generic;

public class FageAudioPooler {
	private	FageAudioNode 				_node;
	private	FageAudioSourceControl[]	_controls;
	private	int							_index;

	public	FageAudioPooler(FageAudioNode node, GameObject listener) {
		_node = node;
		_index = 0;
		_controls = new FageAudioSourceControl[node.channels];

		for (int i = 1; i <= _node.channels; i++) {
			GameObject child = new GameObject (node.name + " " + i.ToString (), typeof(AudioSource), typeof(FageAudioSourceControl));
			child.transform.SetParent (listener.transform);
			FageAudioSourceControl control = child.GetComponent<FageAudioSourceControl>();
			control.enabled = false;
			_controls[i - 1] = control;
		}
	}

	public	FageAudioSourceControl[] GetAudioSourceControls() {
		return _controls;
	}

	public	FageAudioSourceControl GetFreeAudioSourceControl() {
		if (_node.channels == 0) {
			return null;
		}

		FageAudioSourceControl control = _controls [_index];
		if (control.audioStatus==FageAudioStatus.PLAYING) {
			control.Stop ();
		}
		_index = (_index + 1) % _node.channels;
		return control;
	}

	public	FageAudioSourceControl[] FindAudioSourceControl(AudioClip clip) {
		List<FageAudioSourceControl> buffer = new List<FageAudioSourceControl>();
		foreach (FageAudioSourceControl control in _controls) {
			if (control.audioSource.clip==clip) {
				buffer.Add(control);
			}
		}
		return buffer.ToArray();
	}
}
