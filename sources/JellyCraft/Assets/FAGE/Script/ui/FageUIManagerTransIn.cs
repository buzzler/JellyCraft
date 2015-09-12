using UnityEngine;
using System.Collections;

public class FageUIManagerTransIn : FageState {
	private	FageUICommonMem current;

	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
		FageUIManager manager = stateMachine as FageUIManager;
		Queue queue = manager.GetRequests ();
		if (queue.Count > 0) {
			FageUIRequest request = queue.Peek () as FageUIRequest;
			switch (request.command) {
			case FageUIRequest.POP:
				ExcutePop(manager, request);
				break;
			case FageUIRequest.CHANGE:
			case FageUIRequest.PUSH:
				ExcutePush (manager, request);
				break;
			case FageUIRequest.POPUP:
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
			this.current = current;
			current.Instantiate (manager.canvas, request.param);
		} else {
			manager.GetRequests ().Dequeue ();
			manager.ReserveState ("FageUIManagerIdle");
			return;
		}
	}

	private	void ExcutePop(FageUIManager manager, FageUIRequest request) {
		Stack stack = manager.GetStack ();
		if (stack.Count > 0) {
			FageUIMem current = stack.Peek () as FageUIMem;
			this.current = current;
			current.Resume(manager.canvas, request.param);
		} else {
			manager.GetRequests ().Dequeue ();
			manager.ReserveState ("FageUIManagerIdle");
			return;
		}
	}

	private	void ExcutePopdown(FageUIManager manager, FageUIRequest request) {
		Queue queue = manager.GetQueue ();
		if (queue.Count > 0) {
			FageUIPopupMem current = queue.Peek () as FageUIPopupMem;
			this.current = current;
			current.Instantiate (manager.canvas, request.param);
		} else {
			manager.GetRequests ().Dequeue ();
			manager.ReserveState ("FageUIManagerIdle");
			return;
		}
	}

	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		FageUIManager manager = stateMachine as FageUIManager;
		if (current.state == FageUICommonMem.INTANTIATED) {
			manager.GetRequests ().Dequeue ();
			manager.ReserveState ("FageUIManagerIdle");
		}
	}
	
	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId) {
		base.BeforeSwitch (stateMachine, afterId);
	}
}
