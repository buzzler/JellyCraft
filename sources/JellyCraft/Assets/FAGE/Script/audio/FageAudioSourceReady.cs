public	class FageAudioSourceReady : FageState {
	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
		FageAudioSourceControl fsm = stateMachine as FageAudioSourceControl;
		fsm.SetAudioStatus(FageAudioStatus.READY);
	}
	
	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId){
		base.BeforeSwitch (stateMachine, afterId);
	}
	
	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		FageAudioSourceControl fsm = stateMachine as FageAudioSourceControl;
		if (fsm.audioSource.isPlaying) {
			fsm.ReserveState("FageAudioSourcePlaying");
		}
	}
}