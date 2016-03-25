﻿using UnityEngine;
using System.Collections;

public class GalleryUnlocked : FageState {
	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
Debug.Log("GalleryUnlocked");
		GalleryScreen gs = stateMachine as GalleryScreen;
		gs.ShowTexts();
		gs.ShowStars();
		gs.HideButtons();
	}

	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		GalleryScreen gs = stateMachine as GalleryScreen;
		if (gs.IsDragging()) {
			gs.ReserveState("GalleryDragging");
		}
	}

	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId) {
		base.BeforeSwitch (stateMachine, afterId);
		GalleryScreen gs = stateMachine as GalleryScreen;
		gs.HideTexts();
		gs.HideStars();
		gs.HideButtons();
	}
}
