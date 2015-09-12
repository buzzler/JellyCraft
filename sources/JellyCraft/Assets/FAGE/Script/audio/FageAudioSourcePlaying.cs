public	class FageAudioSourcePlaying : FageState {
	private	int _timeSamples;
	
	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
		FageAudioSourceControl fsm = stateMachine as FageAudioSourceControl;
		fsm.SetAudioStatus(FageAudioStatus.PLAYING);
		_timeSamples = fsm.audioSource.timeSamples;
	}
	
	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId){
		base.BeforeSwitch (stateMachine, afterId);
	}
	
	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		FageAudioSourceControl fsm = stateMachine as FageAudioSourceControl;
		if (!fsm.audioSource.isPlaying) {
			if (fsm.audioSource.timeSamples > 0) {
				fsm.ReserveState("FageAudioSourcePaused");
			} else {
				fsm.ReserveState("FageAudioSourceReady");
			}
		} else if (_timeSamples > fsm.audioSource.timeSamples) {
			fsm.NotifyLoop();
		}
		_timeSamples = fsm.audioSource.timeSamples;
	}
}