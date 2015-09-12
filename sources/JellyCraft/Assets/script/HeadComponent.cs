﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityJellySprite))]
public class HeadComponent : MonoBehaviour {
	private	UnityJellySprite	jelly;
	private	bool				flash;

	void Awake() {
		jelly = GetComponent<UnityJellySprite> ();
		SetDraggable(true);
	}

	void Start() {
		Invoke("TestForce", Random.Range(2, 5));
	}

	void Update() {
		if (flash) {
			foreach (JellySprite.ReferencePoint rp in jelly.m_ReferencePoints) {
//				EffectComponent.Show(EffectType.FACE_DRAG_OUT, rp.transform.position);
			}
			flash = false;
		}
	}

	void OnDisable() {
		CancelInvoke ();
	}

	public	void SetDraggable(bool enable) {
		jelly.draggable = enable;
	}

	public	void TestForce() {
		jelly.Boing ();
		Invoke("TestForce", Random.Range(2, 5));
	}

	public	void Flash() {
		flash = true;
	}
}
