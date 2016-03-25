using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraEffectorTween : FageState {
	private	CameraEffector	_effector;
	private	OLDTVScreen		_screen;
	private BlurOptimized	_blur;
	private	EffectPreset	_presetFrom;
	private	EffectPreset	_presetTo;

	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
		_effector = stateMachine as CameraEffector;
		_screen = _effector.screen;
		_blur = _effector.blur;

		EffectRequest request = _effector.GetRequest();
		if (request==null) {
			_effector.ReserveState("CameraEffectorIdle");
			return;
		}

		_presetFrom = _effector.DumpEffect();
		_presetTo = _effector.GetPreset(request.preset);
		if (_presetTo==null) {
			_effector.ReserveState("CameraEffectorIdle");
			return;
		}

		if (request.second > 0) {
			if ((_presetFrom.blurSize != _presetTo.blurSize) &&
				(_presetFrom.blurEnable != _presetTo.blurEnable))
				_blur.enabled = true;
			if ((_presetFrom.screenSaturation != _presetTo.screenSaturation) ||
			    (_presetFrom.chromaticAberrationMagnetude != _presetTo.chromaticAberrationMagnetude) ||
			    (_presetFrom.staticMagnetude != _presetTo.staticMagnetude) &&
			    (_presetFrom.screenEnable != _presetTo.screenEnable))
				_screen.enabled = true;
			LeanTween.value(_effector.gameObject, 0f, 1f, request.second).setOnUpdate(OnTweenUpdate).setOnComplete(OnTweenComplete);
		} else {
			OnTweenComplete();
		}
	}

	private	void OnTweenUpdate(float value) {
		_screen.screenSaturation				= Mathf.Lerp(_presetFrom.screenSaturation, _presetTo.screenSaturation, value);
		_screen.chromaticAberrationMagnetude	= Mathf.Lerp(_presetFrom.chromaticAberrationMagnetude, _presetTo.chromaticAberrationMagnetude, value);
		_screen.staticMagnetude					= Mathf.Lerp(_presetFrom.staticMagnetude, _presetTo.staticMagnetude, value);
		_blur.blurSize							= Mathf.Lerp(_presetFrom.blurSize, _presetTo.blurSize, value);
	}

	private void OnTweenComplete() {
		_screen.enabled							= _presetTo.screenEnable;
		_screen.screenSaturation				= _presetTo.screenSaturation;
		_screen.chromaticAberrationMagnetude	= _presetTo.chromaticAberrationMagnetude;
		_screen.staticMagnetude					= _presetTo.staticMagnetude;
		_blur.enabled							= _presetTo.blurEnable;
		_blur.blurSize							= _presetTo.blurSize;
		_effector.ReserveState("CameraEffectorIdle");
	}

	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId) {
		base.BeforeSwitch (stateMachine, afterId);
		_effector = null;
		_screen = null;
		_blur = null;
		_presetFrom = null;
		_presetTo = null;
	}
}
