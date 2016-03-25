using UnityEngine;
using System.Collections;

public class ResultPopup : MonoBehaviour, IFageUIPopupComponent {
	public	void OnUIInstantiate(FageUIPopupMem mem, params object[] param) {}

	public	void OnUIInstantiated(FageUIPopupMem mem) {
		Invoke("OnResultComplete", 2f);
	}

	private	void OnResultComplete() {
		FageUIManager manager = FageUIManager.Instance;
		FageUIRoot root = FageUIRoot.Instance;

		manager.Popdown();
		manager.Pop();
		manager.Level("title_horse", root.FindUICurtain("blackout"));
		manager.Change(root.FindUISet("title"));
	}

	public	void OnUIDestroy(FageUIPopupMem mem) {
		CameraEffector.Instance.SetPreset("default", 1f);
	}

	public	void OnSwitchOut(FageUIPopupMem mem) {}
	public	void OnSwitchIn(FageUIPopupMem mem) {}

	public	GameObject GetGameObject() {
		return gameObject;
	}
}
