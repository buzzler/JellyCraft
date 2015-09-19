using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(OLDTVScreen))]
[RequireComponent(typeof(BlurOptimized))]
public class CameraEffector : FageStateMachine {
	private	static CameraEffector _instance;
	public	static CameraEffector Instance { get { return _instance; } }

	public	EffectPreset[]						presets;
	public	OLDTVScreen							screen;
	public	BlurOptimized						blur;
	private	Queue<EffectRequest>				_queue;
	private	Dictionary<string, EffectPreset>	_dictionary;

	void Awake() {
		_instance = this;
		_queue = new Queue<EffectRequest>();
		_dictionary = new Dictionary<string, EffectPreset>();

		for (int i = 0 ; i < presets.Length ; i++) {
			_dictionary.Add(presets[i].id, presets[i]);
		}
	}

	public	int GetRequestCount() {
		return _queue.Count;
	}

	public	EffectRequest GetRequest() {
		if (_queue.Count > 0)
			return _queue.Dequeue();
		else
			return null;
	}

	public	EffectPreset GetPreset(string id) {
		if (_dictionary.ContainsKey(id))
			return _dictionary[id];
		else
			return null;
	}

	public	EffectPreset DumpEffect() {
		EffectPreset result					= new EffectPreset();
		result.id							= "dump";
		result.screenEnable					= screen.enabled;
		result.screenSaturation				= screen.screenSaturation;
		result.chromaticAberrationMagnetude	= screen.chromaticAberrationMagnetude;
		result.staticMagnetude				= screen.staticMagnetude;
		result.blurEnable					= blur.enabled;
		result.blurSize						= blur.blurSize;
		return result;
	}

	public	void SetPreset(string presetId, float second = 0f) {
		_queue.Enqueue(new EffectRequest(presetId, second));
	}

//	public	void SetPreset(string id) {
//		for (int i = 0 ; i < presets.Length ;  i++) {
//			if (presets[i].id == id) {
//				EffectPreset preset = presets[i];
//				screen.screenSaturation				= preset.screenSaturation;
//				screen.chromaticAberrationMagnetude	= preset.chromaticAberrationMagnetude;
//				screen.staticMagnetude					= preset.staticMagnetude;
//				return;
//			}
//		}
//	}
}

public	class EffectRequest {
	private	string	_preset;
	private	float	_second;
	public	string	preset { get { return _preset; } }
	public	float	second { get { return _second; } }

	public	EffectRequest(string preset, float second) {
		_preset = preset;
		_second = second;
	}
}

[Serializable]
public	class EffectPreset {
	public	string	id;
	public	bool	screenEnable;
	[Range(0f,1f)]
	public	float	screenSaturation;
	[Range(0f,1f)]
	public	float	chromaticAberrationMagnetude;
	[Range(0f,1f)]
	public	float	staticMagnetude;
	public	bool	blurEnable;
	[Range(0f,10f)]
	public	float	blurSize;
}