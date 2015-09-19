using UnityEngine;
using System;
using System.Collections;

public class GalleryScreen : MonoBehaviour,IFageUIComponent {
	public	RectTransform	content;
	public	RectTransform[]	items;
	public	RectTransform	center;
	public	int				gap;
	private	float[]			_distances;
	private	bool			_dragging;
	private	int				_minItem;

	void Awake() {
		_distances = new float[items.Length];
		_dragging = false;
	}

	void Update() {
		float distance = 0f;
		for (int i = 0 ; i < items.Length ; i++) {
			distance = Mathf.Abs(center.position.y - items[i].position.y);
			_distances[i] = distance;
			float s = Mathf.Lerp(1.3f, 1f, Mathf.Min(distance,160f)/160f);
			items[i].localScale = new Vector3(s,s, 1f);
		}

		if (!_dragging) {
			_minItem = Array.IndexOf<float>(_distances, Mathf.Min(_distances));
			float newY = Mathf.Lerp(content.anchoredPosition.y, (_minItem * -gap), Time.deltaTime * 10f);
			content.anchoredPosition = new Vector2(content.anchoredPosition.x, newY);
		}
	}

	public	void StartDragging() {
		_dragging = true;
	}

	public	void EndDragging() {
		_dragging = false;
	}

	public	void OnUIInstantiate(FageUIMem mem, params object[] param) {
		CameraEffector.Instance.SetPreset("blurry", 1f);
	}

	public	void OnUIInstantiated(FageUIMem mem) {}
	public	void OnUIDestroy(FageUIMem mem) {
		CameraEffector.Instance.SetPreset("default", 1f);
	}
	public	void OnSwitchOut(FageUIMem mem) {}
	public	void OnSwitchIn(FageUIMem mem) {}
	public	void OnUIPause(FageUIMem mem) {}
	public	void OnUIResume(FageUIMem mem, params object[] param) {}
	public	void OnUIResumed(FageUIMem mem) {}

	public	GameObject	GetGameObject() {
		return gameObject;
	}

	public	void OnClickHome() {
		FageUIManager.Instance.Pop();
	}
}
