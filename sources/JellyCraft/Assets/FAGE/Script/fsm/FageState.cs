using UnityEngine;
using System;

[Serializable]
public	class FageState {
	private string _id;
	
	public	string id {
		get {
			return _id;
		}
	}
	
	public FageState() {
		_id = GetType().FullName;
	}
	
	public	virtual	void BeforeSwitch(FageStateMachine stateMachine, string afterId = null) {
	}
	
	public	virtual	void AfterSwitch(FageStateMachine stateMachine, string beforeId = null) {
		Debug.Log(this);
	}
	
	public	virtual	void Excute(FageStateMachine stateMachine) {
	}
}