using UnityEngine;

public	class FageWebState {
	private	int		_requestId;
	private	string	_url;
	private int		_version;
	private	WWWForm	_wwwForm;
	
	public	int		requestId	{ get { return _requestId; } }
	public	string	url			{ get { return _url; } }
	public	int		version		{ get { return _version; } }
	public	WWWForm	wwwForm		{ get { return _wwwForm; } }
	
	public	FageWebState(int requestId, string url, int version = -1, WWWForm wwwForm = null) {
		_requestId = requestId;
		_url = url;
		_version = version;
		_wwwForm = wwwForm;
	}
}