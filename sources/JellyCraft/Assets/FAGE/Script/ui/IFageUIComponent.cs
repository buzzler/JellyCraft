using UnityEngine;

public	interface IFageUIComponent {
	void		OnUIInstantiate(FageUIMem mem, params object[] param);
	void		OnUIInstantiated(FageUIMem mem);
	void		OnUIDestroy(FageUIMem mem);
	void		OnSwitchOut(FageUIMem mem);
	void		OnSwitchIn(FageUIMem mem);
	void		OnUIPause(FageUIMem mem);
	void		OnUIResume(FageUIMem mem, params object[] param);
	void		OnUIResumed(FageUIMem mem);
	GameObject	GetGameObject();
}