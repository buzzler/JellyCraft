using UnityEngine;

public class FageConnectionStart : FageState {
	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
	}

	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId) {
		base.BeforeSwitch (stateMachine, afterId);
	}

	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		switch (Application.internetReachability) {
		case NetworkReachability.ReachableViaCarrierDataNetwork:
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			stateMachine.ReserveState("FageConnectionPing");
			break;
		}
	}
}
