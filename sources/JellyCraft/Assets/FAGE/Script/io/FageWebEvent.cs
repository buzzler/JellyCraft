using UnityEngine;

public class FageWebEvent : FageEvent {
	public	const string PROGRESS = "progress";

	private	int		_requestId;
	private WWW		_www;
	private	float	_progress;
	
	public	int		requestId	{ get { return _requestId; } }
	public	string	url			{ get { return _www.url; } }
	public	WWW		www			{ get { return _www; } }
	public	float	progress	{ get { return _progress; } }
	
	public	FageWebEvent(string type, int requestId, WWW www) : base(type) {
		_requestId = requestId;
		_www = www;
		_progress = 0f;
	}

	public	FageWebEvent(string type, int requestId, float progress) : base(type) {
		_requestId = requestId;
		_www = null;
		_progress = progress;
	}
}