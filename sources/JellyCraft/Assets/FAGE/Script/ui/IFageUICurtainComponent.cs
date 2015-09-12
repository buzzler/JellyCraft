using UnityEngine;
using System.Collections;

public interface IFageUICurtainComponent {
	void StartClose(System.Action callback);
	void StartOpen(System.Action callback);
	void SetProgress(float progress);
	GameObject	GetGameObject();
}
