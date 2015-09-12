using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour, IFageUIComponent {
	public	void		OnUIInstantiate(FageUIMem mem, params object[] param){}
	public	void		OnUIDestroy(FageUIMem mem){}
	public	void		OnSwitchOut(FageUIMem mem){}
	public	void		OnSwitchIn(FageUIMem mem){}
	public	void		OnUIPause(FageUIMem mem){}
	public	void		OnUIResume(FageUIMem mem, params object[] param){}

	public	GameObject	GetGameObject() {
		return gameObject;
	}
}
