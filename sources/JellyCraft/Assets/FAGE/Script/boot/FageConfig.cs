using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Config")]
public class FageConfig {
	private	static FageConfig _instance;
	public	static FageConfig Instance	{ get { return _instance; } }

	[XmlAttribute("url")]
	public	string			url;
	[XmlElement("BundleRoot")]
	public	FageBundleRoot	bundleRoot;
	[XmlElement("UIRoot")]
	public	FageUIRoot		uiRoot;
	[XmlElement("AudioRoot")]
	public	FageAudioRoot	audioRoot;

	public	static void LoadFromText(string text) {
		var serializer = new XmlSerializer(typeof(FageConfig));
		_instance = serializer.Deserialize(new StringReader(text)) as FageConfig;
		_instance.bundleRoot.Hashing();
		_instance.uiRoot.Hashing();
		_instance.audioRoot.Hashing();
	}
}
