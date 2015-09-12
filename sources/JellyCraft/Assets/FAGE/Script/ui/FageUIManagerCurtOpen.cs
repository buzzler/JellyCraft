using UnityEngine;
using System.Collections;

public class FageUIManagerCurtOpen : FageState {
	private	IFageUICurtainComponent _curtain;

	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
		FageUIManager manager = stateMachine as FageUIManager;
		_curtain = manager.canvas.GetComponentInChildren<IFageUICurtainComponent>();
		if (_curtain == null)
			throw new UnityException("no interface found : IFageUICurtainComponent");

		_curtain.StartOpen(OnOpenComplete);
	}

	private	void OnOpenComplete() {
		GameObject.Destroy(_curtain.GetGameObject());
		FageUIManager manager = FageUIManager.Instance;
		manager.GetRequests().Dequeue();
		manager.ReserveState ("FageUIManagerIdle");
	}

	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId) {
		base.BeforeSwitch (stateMachine, afterId);
		_curtain = null;
	}
}
