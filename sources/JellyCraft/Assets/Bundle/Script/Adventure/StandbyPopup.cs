using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class StandbyPopup : MonoBehaviour,IFageUIPopupComponent {
	public	Image imageReady;
	public	Image imageGo;
	private Action _callback;

	public	void OnUIInstantiate(FageUIPopupMem mem, params object[] param) {
		if (param!=null && param.Length>0)
			_callback = param[0] as Action;
	}
	public	void OnUIInstantiated(FageUIPopupMem mem) {
		LeanTween.move(imageReady.rectTransform, imageGo.rectTransform.anchoredPosition, 0.5f).setEase(LeanTweenType.easeOutBounce).setOnComplete(OnReadyComplete);
		LeanTween.scale(imageReady.rectTransform, new Vector3(1f,1f,1f), 0.3f).setEase(LeanTweenType.easeOutCirc);
	}

	private	void OnReadyComplete() {
		LeanTween.scale(imageReady.rectTransform, new Vector3(0f,0f,1f), 0.3f).setEase(LeanTweenType.easeInCubic).setDelay(1f);
		LeanTween.scale(imageGo.rectTransform, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutElastic).setDelay(1f).setOnComplete(OnGoComplete);
	}

	private	void OnGoComplete() {
		LeanTween.scale(imageGo.rectTransform, new Vector3(0f,0f,1f), 0.3f).setEase(LeanTweenType.easeInBack).setDelay(0.5f).setOnComplete(OnStandbyComplete);
	}

	private	void OnStandbyComplete() {
		FageUIManager.Instance.Popdown();
	}

	public	void OnUIDestroy(FageUIPopupMem mem) {
		_callback();
	}

	public	void OnSwitchOut(FageUIPopupMem mem) {}
	public	void OnSwitchIn(FageUIPopupMem mem) {}

	public	GameObject	GetGameObject() {
		return gameObject;
	}
}
