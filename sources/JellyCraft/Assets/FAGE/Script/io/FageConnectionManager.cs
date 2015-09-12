using UnityEngine;
using System.Collections;

[AddComponentMenu("Fage/Net/ConnectionStateMachine")]
public class FageConnectionManager : FageStateMachine {
	private	static FageConnectionManager _instance;
	public	static FageConnectionManager Instance { get { return _instance; } }

	void Awake() {
		_instance = this;
	}

	public	bool IsOnline() {
		if (this.current is FageConnectionPing) {
			return (current as FageConnectionPing).IsOnline ();
		} else {
			return false;
		}
	}
}
