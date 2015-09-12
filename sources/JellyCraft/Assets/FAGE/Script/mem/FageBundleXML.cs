using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class FageBundleRoot {
	private	static FageBundleRoot _instance;
	public	static FageBundleRoot Instance { get { return _instance; } }

	[XmlAttribute("url")]
	public	string							url;
	[XmlElement("Bundle")]
	public	FageBundle[]					bundles;
	private	Dictionary<string, FageBundle>	_dictionary;

	public	FageBundleRoot() {
		_instance = this;
		bundles = new FageBundle[0];
		_dictionary = new Dictionary<string, FageBundle>();
	}

	public	void Hashing() {
		_dictionary.Clear();
		foreach (FageBundle bundle in bundles) {
			_dictionary.Add(bundle.id, bundle);
		}
	}

//	public	static void LoadFromText(string text) {
//		var serializer = new XmlSerializer(typeof(FageBundleRoot));
//		_instance = serializer.Deserialize(new StringReader(text)) as FageBundleRoot;
//		_instance.Hashing();
//	}

	public	FageBundle FindBundle(string id) {
		if (_dictionary.ContainsKey(id))
			return _dictionary[id];
		else
			return null;
	}
}

public	class FageBundle {
	[XmlAttribute("id")]
	public	string	id;
	[XmlAttribute("dynamic")]
	public	bool	isDynamic;
	[XmlAttribute("ios")]
	public	string	ios;
	[XmlAttribute("android")]
	public	string	android;
	[XmlAttribute("version")]
	public	int		version;
}