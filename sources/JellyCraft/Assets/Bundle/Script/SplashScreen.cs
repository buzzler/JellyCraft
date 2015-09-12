using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {
	void Start() {
		Invoke("OnStart", 3f);
	}

	private	void OnStart() {
		FageUIManager.Instance.Level("title_alpaca", FageUIRoot.Instance.FindUICurtain("blackout"));
		FageUIManager.Instance.Change(FageUIRoot.Instance.FindUISet("title"));
	}
}
