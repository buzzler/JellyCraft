using UnityEngine;
using System.Collections;

public class FageUIManagerCurtClose : FageState {
	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
		FageUIManager manager = stateMachine as FageUIManager;
		FageUIRequest request = manager.GetRequests ().Peek () as FageUIRequest;

		if ((request.uiCurtain == null) || (request.levelName == null))
			throw new UnityException ("parameter (FageUICurtain, levelName) is required");

		GameObject go = GameObject.Instantiate (FageBundleLoader.Instance.Load (request.uiCurtain) as GameObject);
		go.transform.SetParent (manager.canvas, false);
		IFageUICurtainComponent curtain = go.GetComponent<IFageUICurtainComponent> ();
		if (curtain == null)
			throw new UnityException ("no interface found : IFageUICurtainComponent");

		curtain.StartClose (OnCloseComplete);
	}

	private	void OnCloseComplete() {
		FageUIManager.Instance.ReserveState ("FageUIManagerLoad");
	}
}
