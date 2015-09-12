using UnityEngine;
using System.Collections;

public class FageDefaultCurtain : MonoBehaviour, IFageUICurtainComponent {
	public	void StartClose(System.Action callback) {
		LeanTween.alpha(transform as RectTransform, 1f, 0.4f).setFrom(0f).setOnComplete(callback);
	}

	public	void StartOpen(System.Action callback) {
		LeanTween.alpha(transform as RectTransform, 0f, 0.8f).setFrom(1f).setOnComplete(callback);
	}

	public	void SetProgress(float progress) {

	}

	public	GameObject	GetGameObject() {
		return gameObject;
	}
}
