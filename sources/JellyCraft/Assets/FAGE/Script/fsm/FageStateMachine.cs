using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

[AddComponentMenu("Fage/FSM/FageStateMachine")]
public	class FageStateMachine : FageEventDispatcher {
	public	string		reserve;
	private string		_id;
	public	FageState	_current;
	
	public	string id {
		get {
			return _id;
		}
	}

	public	FageStateMachine() {
		_id			= GetType().FullName + "(" + GetInstanceID().ToString() + ")";
	}
	
	public	FageState current {
		get {
			return _current;
		}
	}
	
	public	virtual void ReserveState(string id) {
		reserve = id;
	}
	
	public	virtual void SetState(string id) {
		if (_current != null) {
			_current.BeforeSwitch (this, id);
		}
		
		string temp = (_current != null) ? _current.id : null;
		Type stateType = Type.GetType (id, false, true);
		if (stateType == null) {
			throw new UnityException ("unknown state : " + id);
		}
		ConstructorInfo stateConstructor = stateType.GetConstructor (Type.EmptyTypes);
		object stateObject = stateConstructor.Invoke (new object[]{});
		if (stateObject is FageState) {
			_current = (FageState)stateObject;
		} else {
			throw new UnityException ();
		}
		
		if (_current != null) {
			_current.AfterSwitch (this, temp);
		}
	}

	void LateUpdate() {
		if (_current != null) {
			_current.Excute (this);
		}
		if (!String.IsNullOrEmpty (reserve)) {
			string r = reserve;
			reserve = null;
			SetState (r);
		}
	}
}