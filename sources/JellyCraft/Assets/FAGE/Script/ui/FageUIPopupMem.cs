using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public	class FageUIPopupMem : FageUICommonMem {
	private	FageUISet				_uiSet;
	private	IFageUIPopupComponent	_component;
	private	FageUIDetail			_uiDetail;
	
	public	FageUISet				uiSet			{ get { return _uiSet; } }
	public	IFageUIPopupComponent	component		{ get { return _component; } }
	public	FageUIDetail			uiDetail		{ get { return _uiDetail; } }
	
	public	FageUIPopupMem(FageUISet uiSet) : base() {
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
		_component = go.GetComponent<IFageUIPopupComponent> ();
		if (scale) {
			go.transform.localScale = transition.GetScale();
		}
		if (move)
			LeanTween.moveLocal (go, _uiDetail.GetPosition (), transition.time).setDelay (transition.delay).setEase (transition.ease).setOnComplete (callback);
		if (rotate)
			LeanTween.rotateLocal (go, _uiDetail.GetRotation().eulerAngles, transition.time).setDelay(transition.delay).setEase(transition.ease);
		if (scale)
			LeanTween.scale (go, _uiDetail.GetScale(), transition.time).setDelay(transition.delay).setEase(transition.ease);
	}
	
	private	void SetTweenOut(byte tween, FageUITransition transition, System.Action callback) {
		bool move = (tween & FageUITransition.POSITION) != FageUITransition.NONE;
		bool rotate = (tween & FageUITransition.ROTATION) != FageUITransition.NONE;
		bool scale = (tween & FageUITransition.SCALE) != FageUITransition.NONE;
		
		GameObject go = _component.GetGameObject();
		if (move)
			LeanTween.moveLocal (go, transition.GetPosition(), transition.time).setDelay (transition.delay).setEase (transition.ease).setOnComplete (callback);
		if (rotate)
			LeanTween.rotateLocal (go, transition.GetRotation().eulerAngles, transition.time).setDelay(transition.delay).setEase(transition.ease);
		if (scale)
			LeanTween.scale (go, transition.GetScale(), transition.time).setDelay(transition.delay).setEase(transition.ease);
	}

	public	void Instantiate(Transform canvas, params object[] param) {
		_uiDetail = _uiSet.GetCurrentUIDetail ();
		SetTweenIn(_uiDetail.WhichTransitionOnInstantiate(), _uiDetail.GetTransitionOnInstantiate(), OnInstantiateComplete, canvas);
		_component.OnUIInstantiate (this, param);
	}

	private	void OnInstantiateComplete() {
		FageScreenManager.Instance.AddEventListener (FageScreenEvent.ORIENTATION, OnScreenOrientation);
		SetState (FageUICommonMem.INTANTIATED);
	}

	public	void Destroy() {
		SetTweenOut(_uiDetail.WhichTransitionOnDestroy(), _uiDetail.GetTransitionOnDestroy(), OnDestroyComplete);
	}

	private	void OnDestroyComplete() {
		FageScreenManager.Instance.RemoveEventListener(FageScreenEvent.ORIENTATION, OnScreenOrientation);
		_component.OnUIDestroy (this);
		_uiDetail = null;
		GameObject.Destroy (_component.GetGameObject());
		SetState (FageUICommonMem.DESTROIED);
	}

	private	void OnScreenOrientation(FageEvent fevent) {
		FageUIDetail bakDetail = _uiDetail;
		_uiDetail = _uiSet.GetCurrentUIDetail ();
		if (_uiDetail == bakDetail)
			return;

		SetTweenOut(bakDetail.WhichTransitionOnSwitchOut(), bakDetail.GetTransitionOnSwitchOut(), OnScreenOrientationOut);
	}

	private	void OnScreenOrientationOut() {
		GameObject go = _component.GetGameObject ();
		Transform canvas = go.transform.parent;
		_component.OnSwitchOut (this);
		GameObject.Destroy (go);

		SetTweenIn(_uiDetail.WhichTransitionOnSwitchIn(), _uiDetail.GetTransitionOnSwitchIn(), OnScreenOrientationComplete, canvas);
		_component.OnSwitchIn (this);
	}

	private	void OnScreenOrientationComplete() {
	}
}