using UnityEngine;
using System.Collections;

public class GalleryScrolling : FageState {
	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
Debug.Log("GalleryScrolling");
		GalleryScreen gs = stateMachine as GalleryScreen;
		gs.HideTexts();
		gs.HideStars();
		gs.HideButtons();

		LeanTween.move(gs.content.transform as RectTransform, new Vector3(0f, -(gs.availables-1) * gs.gap, 0f), 2f).setDelay(1.3f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(OnTweenComplete).setOnCompleteParam(gs);
	}

	private	void OnTweenComplete(object param) {
		GalleryScreen gs = param as GalleryScreen;
		gs.GetLastItem();
		//do something
		gs.ReserveState("GalleryLocked");
	}

	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId) {
		base.BeforeSwitch (stateMachine, afterId);
	}
}
