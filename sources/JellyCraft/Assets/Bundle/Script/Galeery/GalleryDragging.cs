using UnityEngine;
using System.Collections;

public class GalleryDragging : FageState {
	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
Debug.Log("GalleryDragging");
	}

	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		GalleryScreen gs = stateMachine as GalleryScreen;
		if (!gs.IsDragging()) {
			RectTransform rt = gs.GetNearestItem();
			/**
			 * something
			 */
			switch(Random.Range(1,3)) {
			case 1:
				gs.ReserveState("GalleryUnlocked");
				break;
			case 2:
				gs.ReserveState("GalleryLocked");
				break;
			}
		}
	}
}
