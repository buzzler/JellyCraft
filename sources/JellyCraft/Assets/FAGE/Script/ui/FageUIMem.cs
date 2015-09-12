using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public	class FageUIMem : FageUICommonMem {
	private	FageUISet			_uiSet;
	private	IFageUIComponent	_component;
	private	FageUIDetail		_uiDetail;
	private	SpriteRenderer[]	_renderers;

	public	FageUISet			uiSet			{ get { return _uiSet; } }
	public	IFageUIComponent	component		{ get { return _component; } }
	public	FageUIDetail		uiDetail		{ get { return _uiDetail; } }

	public	FageUIMem(FageUISet uiSet) : base() {
		_uiSet = uiSet;
		_component = null;
		_uiDetail = null;
	}

	private	void SetTweenIn(byte tween, FageUITransition transition, System.Action callback, Transform canvas) {
		bool move = (tween & FageUITransition.POSITION) != FageUITransition.NONE;
		bool rotate = (tween & FageUITransition.ROTATION) != FageUITransition.NONE;
		bool scale = (tween & FageUITransition.SCALE) != FageUITransition.NONE;

		GameObject cach = FageBundleLoader.Instance.Load(uiDetail) as GameObject;
		GameObject go = GameObject.Instantiate (cach, move ? transition.GetPosition ():_uiDetail.GetPosition (), rotate ? transition.GetRotation ():_uiDetail.GetRotation ()) as GameObject;
		go.transform.SetParent (canvas, false);
		_component = go.GetComponent<IFageUIComponent> ();

		LTDescr ltdesc = null;
		if (move)
			ltdesc = LeanTween.moveLocal (go, _uiDetail.GetPosition (), transition.time).setDelay (transition.delay).setEase (transition.ease);
		if (rotate)
			ltdesc = LeanTween.rotateLocal (go, _uiDetail.GetRotation().eulerAngles, transition.time).setDelay(transition.delay).setEase(transition.ease);
		if (scale) {
			go.transform.localScale = transition.GetScale();
			ltdesc = LeanTween.scale (go, _uiDetail.GetScale(), transition.time).setDelay(transition.delay).setEase(transition.ease);
		}

		if ((ltdesc!=null) && (callback!=null))
			ltdesc.setOnComplete(callback);
	}

	private	void SetTweenOut(byte tween, FageUITransition transition, System.Action callback) {
		bool move = (tween & FageUITransition.POSITION) != FageUITransition.NONE;
		bool rotate = (tween & FageUITransition.ROTATION) != FageUITransition.NONE;
		bool scale = (tween & FageUITransition.SCALE) != FageUITransition.NONE;

		GameObject go = _component.GetGameObject();
		LTDescr ltdesc = null;
		if (move)
			ltdesc = LeanTween.moveLocal (go, transition.GetPosition(), transition.time).setDelay (transition.delay).setEase (transition.ease);
		if (rotate)
			ltdesc = LeanTween.rotateLocal (go, transition.GetRotation().eulerAngles, transition.time).setDelay(transition.delay).setEase(transition.ease);
		if (scale)
			ltdesc = LeanTween.scale (go, transition.GetScale(), transition.time).setDelay(transition.delay).setEase(transition.ease);

		if ((ltdesc!=null) && (callback!=null))
			ltdesc.setOnComplete(callback);
	}

	public	void Instantiate(Transform canvas, params object[] param) {
		_uiDetail = _uiSet.GetCurrentUIDetail ();
		SetTweenIn(_uiDetail.WhichTransitionOnInstantiate(), _uiDetail.GetTransitionOnInstantiate(), OnInstantiateComplete, canvas);
		_component.OnUIInstantiate (this, param);
	}

	private void OnInstantiateComplete() {
		FageScreenManager.Instance.AddEventListener (FageScreenEvent.ORIENTATION, OnScreenOrientation);
		SetState (FageUICommonMem.INTANTIATED);
	}

	public	void Destroy() {
		SetTweenOut(_uiDetail.WhichTransitionOnDestroy(), _uiDetail.GetTransitionOnDestroy (), OnDestroyComplete);
	}

	private	void OnDestroyComplete() {
		FageScreenManager.Instance.RemoveEventListener (FageScreenEvent.ORIENTATION, OnScreenOrientation);
		_component.OnUIDestroy (this);
		_uiDetail = null;
		GameObject.Destroy (_component.GetGameObject ());
		SetState (FageUICommonMem.DESTROIED);
	}
	
	public	void Resume(Transform canvas, params object[] param) {
		_uiDetail = _uiSet.GetCurrentUIDetail ();
		SetTweenIn(_uiDetail.WhichTransitionOnResume(), _uiDetail.GetTransitionOnResume (), OnResumeComplete, canvas);
		_component.OnUIResume (this, param);
	}

	private	void OnResumeComplete() {
		FageScreenManager.Instance.AddEventListener (FageScreenEvent.ORIENTATION, OnScreenOrientation);
		SetState (FageUICommonMem.INTANTIATED);
	}
	
	public	void Pause() {
		SetTweenOut(_uiDetail.WhichTransitionOnPause(), _uiDetail.GetTransitionOnPause (), OnPauseComplete);
	}

	private	void OnPauseComplete() {
		FageScreenManager.Instance.RemoveEventListener (FageScreenEvent.ORIENTATION, OnScreenOrientation);
		_component.OnUIPause (this);
		_uiDetail = null;
		GameObject.Destroy (_component.GetGameObject ());
		SetState (FageUICommonMem.PAUSED);
	}

	private	void OnScreenOrientation(FageEvent fevent) {
		FageUIDetail bakDetail = _uiDetail;
		_uiDetail = _uiSet.GetCurrentUIDetail ();
		if (bakDetail == _uiDetail)
			return;

		SetTweenOut(bakDetail.WhichTransitionOnSwitchOut(), bakDetail.GetTransitionOnSwitchOut (), OnScreenOrientationOut);
	}

	private	void OnScreenOrientationOut() {
		GameObject go = _component.GetGameObject ();
		Transform canvas = go.transform.parent;
		_component.OnSwitchOut (this);
		GameObject.Destroy (go);

		SetTweenIn(_uiDetail.WhichTransitionOnSwitchIn(), _uiDetail.GetTransitionOnSwitchIn (), OnScreenOrientationComplete, canvas);
		_component.OnSwitchIn (this);
	}

	private void OnScreenOrientationComplete() {
	}
}