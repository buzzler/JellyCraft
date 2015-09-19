using UnityEngine;
using System.Collections;

[AddComponentMenu("JellyCraft/Title/TitleScreen")]
public class TitleScreen : MonoBehaviour, IFageUIComponent {
	public	void		OnUIInstantiate(FageUIMem mem, params object[] param){}
	public	void		OnUIInstantiated(FageUIMem mem){}
	public	void		OnUIDestroy(FageUIMem mem){}
	public	void		OnSwitchOut(FageUIMem mem){}
	public	void		OnSwitchIn(FageUIMem mem){}
	public	void		OnUIPause(FageUIMem mem){}
	public	void		OnUIResume(FageUIMem mem, params object[] param){}
	public	void		OnUIResumed(FageUIMem mem){}

	public	GameObject	GetGameObject() {
		return gameObject;
	}

	public	void OnClickAdventure() {
		FageUIManager.Instance.Change(FageUIRoot.Instance.FindUISet("title_adventure"));
	}

	public	void OnClickGallery() {
		FageUIManager.Instance.Push(FageUIRoot.Instance.FindUISet("gallery"));
	}
}