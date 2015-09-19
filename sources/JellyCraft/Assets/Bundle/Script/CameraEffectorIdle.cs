using UnityEngine;
using System.Collections;

public class CameraEffectorIdle : FageState {
	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		CameraEffector effector = stateMachine as CameraEffector;

		if (effector.GetRequestCount() > 0) {
			effector.ReserveState("CameraEffectorTween");
		}
	}
}
