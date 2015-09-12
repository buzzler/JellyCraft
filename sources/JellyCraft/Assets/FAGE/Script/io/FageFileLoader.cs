using UnityEngine;
using System.IO;

[AddComponentMenu("Fage/IO/FageFileLoader")]
public class FageFileLoader : FageEventDispatcher {
	private	static FageFileLoader _instance;
	private	static int _countId;
	public	static FageFileLoader Instance { get { return _instance; } }

	void Awake() {
		_instance = this;
		_countId = 0;
	}

	public	int Load(string filePath) {
		byte[] fileData;
		using (FileStream stream = new FileStream (filePath, FileMode.OpenOrCreate)) {
			fileData = new byte[stream.Length];
			stream.BeginRead(fileData, 0, fileData.Length, new System.AsyncCallback(OnLoad), new FageFileState(++_countId, filePath, stream, fileData));
		}
		return _countId;
	}

	private void OnLoad(System.IAsyncResult result) {
		FageFileState state	= result.AsyncState as FageFileState;

		DispatchEvent (new FageFileEvent(FageEvent.COMPLETE, state.requestId, state.filePath, state.fileData));
		state.stream.Close ();
	}

	public	int Save(string filePath, byte[] fileData) {
		using (FileStream stream = new FileStream (filePath, FileMode.OpenOrCreate)) {
			stream.BeginWrite(fileData, 0, fileData.Length, new System.AsyncCallback(OnSave), new FageFileState(++_countId, filePath, stream, fileData));
		}
		return _countId;
	}

	private	void OnSave(System.IAsyncResult result) {
		FageFileState state		= result.AsyncState as FageFileState;

		DispatchEvent (new FageFileEvent(FageEvent.COMPLETE, state.requestId, state.filePath));
		state.stream.Close ();
	}
}

