using UnityEngine;

public	interface IFageUIPopupComponent {
	void		OnUIInstantiate(FageUIPopupMem mem, params object[] param);
	void		OnUIDestroy(FageUIPopupMem mem);
	void		OnSwitchOut(FageUIPopupMem mem);
	void		OnSwitchIn(FageUIPopupMem mem);
	GameObject	GetGameObject();
}