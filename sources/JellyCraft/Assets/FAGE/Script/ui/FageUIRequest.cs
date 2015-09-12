using UnityEngine;
using System.Collections;

public class FageUIRequest {
	public	const string POPUP		= "popup";
	public	const string POPDOWN	= "popdown";
	public	const string PUSH		= "push";
	public	const string POP		= "pop";
	public	const string CHANGE		= "change";
	public	const string FLUSH		= "flush";
	public	const string LEVEL		= "level";

	private	string			_command;
	private	FageUISet		_uiSet;
	private	string			_levelName;
	private	FageUICurtain	_uiCurtain;
	private	object[]		_param;

	public	string			command		{ get { return _command; } }
	public	FageUISet		uiSet		{ get { return _uiSet; } }
	public	string			levelName	{ get { return _levelName; } }
	public	FageUICurtain	uiCurtain	{ get { return _uiCurtain; } }
	public	object[]		param		{ get { return _param; } }

	public	FageUIRequest(string command, FageUISet uiSet, params object[] param) {
		_command = command;
		_uiSet = uiSet;
		_param = param;
	}

	public	FageUIRequest(string command, string levelName, FageUICurtain uiCurtain) {
		_command = command;
		_levelName = levelName;
		_uiCurtain = uiCurtain;
	}

	public	FageUIRequest(string command, params object[] param) {
		_command = command;
		_uiSet = null;
		_param = param;
	}

	public	FageUIRequest(string command) {
		_command = command;
		_uiSet = null;
		_param = null;
	}
}
