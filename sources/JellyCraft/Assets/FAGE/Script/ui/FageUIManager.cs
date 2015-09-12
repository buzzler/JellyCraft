using UnityEngine;
using System.Collections;

public class FageUIManager : FageStateMachine {
	private	static FageUIManager _instance;
	public	static FageUIManager Instance { get { return _instance; } }
	public	GameObject[]	globalObjects;

	public	Transform		canvas;
	private	Stack			_stackUI;
	private	Queue			_queueUIPopup;
	private	Queue			_queueRequest;

	private	FageUIMem		_delayedMem;
	private	FageUIPopupMem	_delayedPopupMem;
	private	object[]		_delayedParam;

	void Awake() {
		_instance = this;
		_stackUI = new Stack ();
		_queueUIPopup = new Queue ();
		_queueRequest = new Queue ();
		for (int i = 0 ; i < globalObjects.Length ; i++) {
			DontDestroyOnLoad(globalObjects[i]);
		}
	}

	public	Queue GetRequests() {
		return _queueRequest;
	}

	public	Stack GetStack() {
		return _stackUI;
	}

	public	Queue GetQueue() {
		return _queueUIPopup;
	}

	public	void Level(string levelName, FageUICurtain uiCurtain) {
		_queueRequest.Enqueue(new FageUIRequest(FageUIRequest.LEVEL, levelName, uiCurtain));
	}

	public	void Change(FageUISet uiSet, params object[] param) {
		_queueRequest.Enqueue(new FageUIRequest(FageUIRequest.CHANGE, uiSet, param));
	}

	public	void Push(FageUISet uiSet, params object[] param) {
		_queueRequest.Enqueue(new FageUIRequest(FageUIRequest.PUSH, uiSet, param));
	}

	public	void Pop(params object[] param) {
		_queueRequest.Enqueue(new FageUIRequest(FageUIRequest.POP, param));
	}

	public	void Flush() {
		_queueRequest.Enqueue(new FageUIRequest(FageUIRequest.FLUSH));
	}

	public	void Popup(FageUISet uiSet, params object[] param) {
		_queueRequest.Enqueue(new FageUIRequest(FageUIRequest.POPUP, uiSet, param));
	}

	public	void Popdown(params object[] param) {
		_queueRequest.Enqueue(new FageUIRequest(FageUIRequest.POPDOWN, param));
	}
}