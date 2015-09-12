public	class FageFileEvent : FageEvent {
	private	int		_requestId;
	private	string	_filepath;
	private	byte[]	_fileData;
	
	public	int		requestId	{ get { return _requestId; } }
	public	string	filepath	{ get { return _filepath; } }
	public	byte[]	fileData	{ get { return _fileData; } }
	
	public	FageFileEvent(string type, int requestId, string filepath, byte[] fileData = null) : base(type) {
		_requestId = requestId;
		_filepath = filepath;
		_fileData = fileData;
	}
}