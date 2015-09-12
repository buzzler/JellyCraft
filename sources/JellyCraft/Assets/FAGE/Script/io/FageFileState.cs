using System.IO;

public	class FageFileState {
	private	int				_requestId;
	private FileStream		_stream;
	private	string			_filePath;
	private	byte[]			_fileData;

	public	int				requestId	{ get { return _requestId; } }
	public	FileStream		stream		{ get { return _stream; } }
	public	string			filePath	{ get { return _filePath; } }
	public	byte[]			fileData	{ get { return _fileData; } }
	
	public	FageFileState(int requestId, string filePath, FileStream stream, byte[] fileData) {
		_requestId = requestId;
		_stream = stream;
		_filePath = filePath;
		_fileData = fileData;
	}
}