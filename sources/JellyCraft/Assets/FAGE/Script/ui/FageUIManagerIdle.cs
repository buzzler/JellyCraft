using UnityEngine;
using System.Collections;

public class FageUIManagerIdle : FageState {
	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
	}

	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		FageUIManager manager = stateMachine as FageUIManager;
		Queue queue = manager.GetRequests ();
		if (queue.Count > 0) {
			FageUIRequest request = queue.Peek() as FageUIRequest;
			switch(request.command) {
			case FageUIRequest.CHANGE:
			case FageUIRequest.POP:
			case FageUIRequest.PUSH:
				ExcutePush(manager, request);
				break;
			case FageUIRequest.FLUSH:
				ExcuteFlush(manager, request);
				break;
			case FageUIRequest.POPUP:
				ExcutePopup(manager, request);
				break;
			case FageUIRequest.POPDOWN:
				ExcutePopdown(manager, request);
				break;
			case FageUIRequest.LEVEL:
				ExcuteLevel(manager, request);
				break;
			default:
				throw new UnityException("unknown commnad");
			}
		}
	}

	private	void ExcutePush(FageUIManager manager, FageUIRequest request) {
		Stack stack = manager.GetStack ();
		if (stack.Count > 0) {
			manager.ReserveState("FageUIManagerTransOut");
		} else {
			manager.ReserveState("FageUIManagerSwitch");
		}
	}

	private void ExcutePopup(FageUIManager manager, FageUIRequest request) {
		Queue queue = manager.GetQueue ();
		FageUIPopupMem mem = new FageUIPopupMem (request.uiSet);
		queue.Enqueue(mem);
		if (queue.Count == 1) {
			manager.ReserveState("FageUIManagerSwitch");
		} else {
			manager.GetRequests().Dequeue();
		}
	}

	private	void ExcutePopdown(FageUIManager manager, FageUIRequest request) {
		Queue queue = manager.GetQueue ();
		if (queue.Count > 0) {
			manager.ReserveState ("FageUIManagerTransOut");
		} else {
			manager.GetRequests ().Dequeue ();
		}
	}

	private void ExcuteFlush(FageUIManager manager, FageUIRequest request) {
		Stack stack = manager.GetStack ();
		FageUIMem current = stack.Peek () as FageUIMem;
		stack.Clear ();
		stack.Push (current);

		manager.GetRequests ().Dequeue ();
	}

	private	void ExcuteLevel(FageUIManager manager, FageUIRequest request) {
		if (request.uiCurtain != null)
			manager.ReserveState ("FageUIManagerCurtClose");
		else
			manager.ReserveState("FageUIManagerLoad");
	}

	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId) {
		base.BeforeSwitch (stateMachine, afterId);
	}
}
