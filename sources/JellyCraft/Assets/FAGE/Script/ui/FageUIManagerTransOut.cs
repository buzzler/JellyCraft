using UnityEngine;
using System.Collections;

public class FageUIManagerTransOut : FageState {
	public	FageUICommonMem current;

	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
		FageUIManager manager = stateMachine as FageUIManager;
		Queue queue = manager.GetRequests ();
		if (queue.Count > 0) {
			FageUIRequest request = queue.Peek () as FageUIRequest;
			switch (request.command) {
			case FageUIRequest.CHANGE:
			case FageUIRequest.POP:
			case FageUIRequest.PUSH:
				ExcutePush (manager, request);
				break;
			case FageUIRequest.POPDOWN:
				ExcutePopdown (manager, request);
				break;
			default:
				throw new UnityException ("unkown command");
			}
		} else {
			throw new UnityException ("request lost");
		}
	}

	private	void ExcutePush(FageUIManager manager, FageUIRequest request) {
		Stack stack = manager.GetStack ();
		if (stack.Count > 0) {
			FageUIMem current = stack.Peek () as FageUIMem;
			current.Destroy ();
			this.current = current;
		} else {
			manager.ReserveState("FageUIManagerSwitch");
		}
	}

	private	void ExcutePopdown(FageUIManager manager, FageUIRequest request) {
		Queue queue = manager.GetQueue ();
		if (queue.Count > 0) {
			FageUIPopupMem current = queue.Peek () as FageUIPopupMem;
			current.Destroy ();
			this.current = current;
		} else {
			manager.ReserveState("FageUIManagerSwitch");
		}
	}
	
	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		if (current != null) {
			if (current.state == FageUICommonMem.DESTROIED) {
				stateMachine.ReserveState("FageUIManagerSwitch");
			}
		}
	}
	
	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId) {
		base.BeforeSwitch (stateMachine, afterId);
	}
}
