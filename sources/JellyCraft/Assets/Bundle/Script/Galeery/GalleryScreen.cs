using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GalleryScreen : FageStateMachine,IFageUIComponent {
	public	Text			textName;
	public	Text			textDescription;
	public	Text			textCost;
	public	Text			textValue;
	public	Image[]			imageStars;
	public	Image			imageCost;
	public	Button			buttonUnlock;
	public	Button			buttonHome;
	public	RectTransform	content;
	public	RectTransform[]	items;
	public	RectTransform	center;
	public	int				gap;
	public	int				availables;
	private	float[]			_distances;
	private	bool			_dragging;
	private	int				_minItem;
	private	float			_distFull;
	private	float			_distLite;

	void Awake() {
		_distances = new float[availables];
		_dragging = false;
		_distFull = Mathf.Abs(center.position.y - buttonHome.transform.position.y);
		_distLite = _distFull / 3f;
	}

	void Update() {
		float distance = 0f;
		for (int i = 0 ; i < availables ; i++) {
			distance = center.position.y - items[i].position.y;
			_distances[i] = Mathf.Abs(distance);

			float s = 1f;
			if (distance < 0)
				s = Mathf.Lerp(1.3f, 1f, Mathf.Min(_distances[i],_distLite)/_distLite);
			else
				s = Mathf.Lerp(1.3f, 0f, Mathf.Min (distance, _distFull) / _distFull);
			items[i].localScale = new Vector3(s,s, 1f);
		}

		if (!_dragging) {
			_minItem = Array.IndexOf<float>(_distances, Mathf.Min(_distances));
			float newY = Mathf.Lerp(content.anchoredPosition.y, (_minItem * -gap), Time.deltaTime * 10f);
			content.anchoredPosition = new Vector2(content.anchoredPosition.x, newY);
		}
	}

	public	void ShowTexts(string name = "Pomeranian", string description = "arranged by\nbuzzler") {
		Color clear = new Color(1f, 1f, 1f, 0f);
		textName.color = clear;
		textDescription.color = clear;
		textName.text = name;
		textDescription.text = description;
		textName.gameObject.SetActive(true);
		textDescription.gameObject.SetActive(true);

		LeanTween.value(gameObject, 0f, 1f, 0.4f).setEase(LeanTweenType.easeOutCubic).setOnUpdate(OnTweenTexts);
	}

	private	void OnTweenTexts(float value) {
		Color temp = textName.color;
		temp.a = value;

		textName.color = temp;
		textDescription.color = temp;
	}

	public	void HideTexts() {
		textName.gameObject.SetActive(false);
		textDescription.gameObject.SetActive(false);
	}

	public	void ShowStars(int num = 4) {
		Color clear = new Color(1f, 1f, 1f, 0f);
		num = Mathf.Clamp(num, 0, imageStars.Length);
		for (int i = 0 ; i < num ; i++) {
			Image s = imageStars[i];
			s.color = clear;
			s.gameObject.SetActive(true);
			LeanTween.cancel(s.gameObject);
			LeanTween.alpha(s.rectTransform, 1f, 0.3f).setDelay(0.05f*(float)i);
		}
	}

	public	void HideStars() {
		for (int i = imageStars.Length-1 ; i >= 0 ; i--) {
			imageStars[i].gameObject.SetActive(false);
		}
	}

	public	void ShowButtons() {
		Color clear = new Color(1f, 1f, 1f, 0f);

		buttonUnlock.gameObject.SetActive(true);
		imageCost.gameObject.SetActive(true);
		textCost.gameObject.SetActive(true);
		textValue.gameObject.SetActive(true);

		buttonUnlock.transform.localScale = new Vector3(0f, 0f, 1f);
		LeanTween.cancel(buttonUnlock.gameObject);
		LeanTween.scale(buttonUnlock.transform as RectTransform, Vector3.one, 0.5f).setEase(LeanTweenType.easeInOutElastic);

		imageCost.color = clear;
		LeanTween.alpha(imageCost.rectTransform, 1f, 0.5f);

		textCost.color = clear;
		LeanTween.alpha(textCost.rectTransform, 1f, 0.3f);
//		LeanTween.alpha(textValue.rectTransform, 1f, 0.3f).setFrom(0f);
	}

	public	void HideButtons() {
		buttonUnlock.gameObject.SetActive(false);
		imageCost.gameObject.SetActive(false);
		textCost.gameObject.SetActive(false);
		textValue.gameObject.SetActive(false);
	}

	public	RectTransform GetLastItem() {
		return items[items.Length-1];
	}

	public	RectTransform GetNearestItem() {
		return items[_minItem];
	}

	public	bool IsDragging() {
		return _dragging;
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
