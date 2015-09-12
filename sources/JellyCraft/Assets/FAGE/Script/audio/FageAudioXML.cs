using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public	class FageAudioRoot : FageAudioBase {
	private	static FageAudioRoot _instance;
	public	static FageAudioRoot Instance { get { return _instance; } }

	private	Dictionary<string, FageAudioXML>	_dictionary;

	public	FageAudioRoot() : base() {
		_instance = this;
		_dictionary = new Dictionary<string, FageAudioXML> ();
	}

	public	FageAudioXML Find(string id) {
		if (_dictionary.ContainsKey (id))
			return _dictionary [id];
		else
			return null;
	}

	public	void Resister(FageAudioBase audioBase) {
		if (_dictionary.ContainsKey (audioBase.id))
			throw new UnityException ("duplicated id : " + audioBase.id);
		else if (audioBase is FageAudioXML)
			_dictionary [audioBase.id] = audioBase as FageAudioXML;
	}

	public	void GenerateControl(AudioListener listener) {
		foreach (FageAudioXML audioXML in _dictionary.Values) {
			audioXML.GenerateControl (listener);
		}
	}

//	public	static void LoadFromText(string text) {
//		var serializer = new XmlSerializer(typeof(FageAudioRoot));
//		_instance = serializer.Deserialize(new StringReader(text)) as FageAudioRoot;
//		_instance.Hashing ();
//	}
}

public	class FageAudioXML : FageAudioBase {
	[XmlAttribute("channels")]
	public	int channels;
	private	List<FageAudioSourceControl> _controls;
	private	int	_index;

	public	FageAudioXML() : base() {
	}

	public	void GenerateControl(AudioListener listener) {
		float newV = GetVolume ();
		_controls = new List<FageAudioSourceControl> ();
		for (int i = 1 ; i <= channels ; i++) {
			GameObject go = new GameObject(this.id+i.ToString(), typeof(AudioSource), typeof(FageAudioSourceControl));
			go.transform.SetParent(listener.transform);
			FageAudioSourceControl control = go.GetComponent<FageAudioSourceControl>();
			_controls.Add(control);
			control.volume = newV;
		}
		_index = 0;
	}

	public	FageAudioSourceControl GetControl() {
		FageAudioSourceControl result = null;
		if (channels > 0) {
			result = _controls[_index];
			_index = (_index + 1) % channels;
		}
		return result;
	}

	public	float GetVolume() {
		float result = this.volume;
		FageAudioBase p = parent;
		while (p != null) {
			result *= p.volume;
			p = p.parent;
		}
		return result;
	}

	public	void SetVolume(float v) {
		this.volume = v;
		float newV = GetVolume ();
		for (int i = 0 ; i < _controls.Count ; i++) {
			_controls[i].volume = newV;
		}
	}
}

public	class FageAudioBase {
	[XmlAttribute("id")]
	public	string			id;
	[XmlAttribute("volume")]
	public	float			volume;
	[XmlElement("AudioXML")]
	public	FageAudioXML[]	children;
	public	FageAudioBase	parent;

	public	FageAudioBase() {
		children = new FageAudioXML[0];
	}

	public	void Hashing(FageAudioBase parent = null) {
		int length = children.Length;
		for (int i = 0 ; i < length ; i++) {
			children[i].Hashing(this);
		}
		this.parent = parent;
		FageAudioRoot.Instance.Resister (this);
	}
}
