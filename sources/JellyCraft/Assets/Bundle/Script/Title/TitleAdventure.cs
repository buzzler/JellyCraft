using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleAdventure : MonoBehaviour, IFageUIComponent {
	public	Text	textBubble;
	public	Text	textName;
	public	Image	bubble;
	public	Vector3	sizeBubble;
	private	string	_name;
	private	string	_message;

	void Awake() {
		// temp
		_name = "Great Bear";
		_message = "괜찮겠나?\n억쑤로 힘들낀데..";

		OnTextUpdate(0f);
		bubble.rectTransform.localScale = new Vector3(0f, 0f, 1f);
	}

	public	void OnUIInstantiate(FageUIMem mem, params object[] param) {
		CameraEffector.Instance.SetPreset("blurry", 0.5f);
	}

	public	void OnUIInstantiated(FageUIMem mem) {
		LeanTween.scale(bubble.rectTransform, sizeBubble, 0.5f).setEase(LeanTweenType.easeInOutCubic);
		LeanTween.value(gameObject, 0f, 1f, 0.3f).setDelay(0.5f).setOnUpdate(OnTextUpdate);

		Invoke("OnInvoke", 3f);
	}

	private	void OnInvoke() {
		LeanTween.value(gameObject, 1f, 0f, 0.2f).setOnUpdate(OnTweenUpdate);
		LeanTween.scale(bubble.rectTransform, new Vector3(0f, 0f, 1f), 0.2f).setDelay(0.2f).setEase(LeanTweenType.easeInBack).setOnComplete(OnTweenComplete);
	}

	private	void OnTextUpdate(float value) {
		textBubble.text = _message.Substring(0, (int)Mathf.Floor((float)_message.Length * value));
		textName.text = _name.Substring(0, (int)Mathf.Floor((float)_name.Length * value));
	}

	private	void OnTweenUpdate(float value) {
		Color temp = textBubble.color;
		temp.a = value;
		textBubble.color = temp;

		temp = textName.color;
		temp.a = value;
		textName.color = temp;
	}

	private	void OnTweenComplete() {
		FageUIManager manager = FageUIManager.Instance;
		manager.Pop();
		manager.Level("adventure_alaska_a", FageUIRoot.Instance.FindUICurtain("blackout"));
//		manager.Push(FageUIRoot.Instance.FindUISet("adventure"));
	}

	public	void OnUIDestroy(FageUIMem mem) {}
	public	void OnSwitchOut(FageUIMem mem) {}
	public	void OnSwitchIn(FageUIMem mem) {}
	public	void OnUIPause(FageUIMem mem) {}
	public	void OnUIResume(FageUIMem mem, params object[] param) {}
	public	void OnUIResumed(FageUIMem mem) {}

	public	GameObject	GetGameObject() {
		return gameObject;
	}
}
