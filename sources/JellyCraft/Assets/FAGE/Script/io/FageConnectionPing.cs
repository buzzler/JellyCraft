using UnityEngine;

public class FageConnectionPing : FageState {
	private	const string _ADDRESS	= "8.8.8.8";
	private const float _TIMEOUT	= 10;
	private	const float _ITERATE	= 20;

	private	Ping	_ping;
	private float	_start_time;
	private float	_last_time;
	private	bool	_online;
	private	int		_ping_time;

	private	void InitPing() {
		if (_ping != null) {
			_ping.DestroyPing ();
		}
		_ping = null;
		_start_time = 0;
		_last_time = -_ITERATE;
	}

	public	bool IsOnline() {
		return _online;
	}

	public override void AfterSwitch (FageStateMachine stateMachine, string beforeId) {
		base.AfterSwitch (stateMachine, beforeId);
		InitPing ();
	}

	public override void BeforeSwitch (FageStateMachine stateMachine, string afterId) {
		base.BeforeSwitch (stateMachine, afterId);
	}

	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			if (_online) {
				stateMachine.DispatchEvent (new FageEvent (FageEvent.SENSOR_OFFLINE));
			}
			stateMachine.ReserveState ("FageConnectionStart");
			return;
		}

		if (_ping != null) {
			bool last = _online;
			if (_ping.isDone) {
				_ping_time = _ping.time;
				InitPing();
				_last_time = Time.realtimeSinceStartup;
				_online = true;
			} else if ((Time.realtimeSinceStartup - _start_time) > _TIMEOUT) {
				_ping_time = (int)_TIMEOUT * 1000;
				InitPing();
				_last_time = Time.realtimeSinceStartup;
				_online = false;
			} else {
				return;
			}

			if (last != _online) {
				if (_online) {
					stateMachine.DispatchEvent (new FageEvent (FageEvent.SENSOR_ONLINE));
				} else {
					stateMachine.DispatchEvent (new FageEvent (FageEvent.SENSOR_OFFLINE));
				}
			} else {
				stateMachine.DispatchEvent (new FageEvent (FageEvent.SENSOR_PING, _ping_time));
			}
		}

		if (_ping == null) {
			if ((Time.realtimeSinceStartup - _last_time) >= _ITERATE) {
				_ping = new Ping (_ADDRESS);
				_start_time = Time.realtimeSinceStartup;
			}
		}
	}
}
