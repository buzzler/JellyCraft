using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public	class FageUIRoot {
	private	static FageUIRoot _instance;
	public	static FageUIRoot Instance { get { return _instance; } }

	[XmlAttribute("url")]
	public	string				url;
	[XmlElement("UITransition")]
	public	FageUITransition[]	transitions;
	[XmlElement("UIDetail")]
	public	FageUIDetail[]		details;
	[XmlElement("UISet")]
	public	FageUISet[]			sets;
	[XmlElement("UICurtain")]
	public	FageUICurtain[]		curtains;

	private	Dictionary<string, FageUITransition>	_dicTransition;
	private	Dictionary<string, FageUIDetail>		_dicDetail;
	private	Dictionary<string, FageUISet>			_dicSet;
	private	Dictionary<string, FageUICurtain>		_dicCurtain;

	public	FageUIRoot() {
		_instance		= this;
		transitions		= new FageUITransition[0];
		details			= new FageUIDetail[0];
		sets			= new FageUISet[0];
		_dicTransition	= new Dictionary<string, FageUITransition> ();
		_dicDetail		= new Dictionary<string, FageUIDetail> ();
		_dicSet			= new Dictionary<string, FageUISet> ();
		_dicCurtain		= new Dictionary<string, FageUICurtain> ();
	}

	public	void Hashing() {
		_dicTransition.Clear ();
		_dicDetail.Clear ();
		_dicSet.Clear ();
		_dicCurtain.Clear ();

		foreach (FageUITransition trans in transitions) {
			_dicTransition.Add (trans.id, trans);
		}
		foreach (FageUIDetail detail in details) {
			_dicDetail.Add (detail.id, detail);
		}
		foreach (FageUISet set in sets) {
			_dicSet.Add (set.id, set);
		}
		foreach (FageUICurtain curtain in curtains) {
			_dicCurtain.Add (curtain.id, curtain);
		}
	}

	public	FageUITransition FindUITransition(string id) {
		if (_dicTransition.ContainsKey(id))
			return _dicTransition[id];
		else
			return null;
	}

	public	FageUIDetail FindUIDetail(string id) {
		if (_dicDetail.ContainsKey(id))
			return _dicDetail[id];
		else
			return null;
	}

	public	FageUISet FindUISet(string id) {
		if (_dicSet.ContainsKey(id))
			return _dicSet[id];
		else
			return null;
	}

	public	FageUICurtain FindUICurtain(string id) {
		if (_dicCurtain.ContainsKey (id))
			return _dicCurtain [id];
		else
			return null;
	}

//	public	static void LoadFromText(string text) {
//		var serializer = new XmlSerializer(typeof(FageUIRoot));
//		_instance = serializer.Deserialize(new StringReader(text)) as FageUIRoot;
//		_instance.Hashing();
//	}
}

public	class FageUICurtain {
	[XmlAttribute("id")]
	public	string id;
	[XmlAttribute("asset")]
	public	string asset;
	[XmlAttribute("bundle")]
	public	string bundle;
}

public	class FageUISet {
	[XmlAttribute("id")]
	public	string id;
	[XmlAttribute("onPortrait")]
	public	string onPortrait;
	[XmlAttribute("onLandscape")]
	public	string onLandscape;

	public	FageUIDetail GetCurrentUIDetail () {
		switch (FageScreenManager.Instance.orientation) {
		case DeviceOrientation.Portrait:
		case DeviceOrientation.PortraitUpsideDown:
			return FageUIRoot.Instance.FindUIDetail(onPortrait);
		case DeviceOrientation.LandscapeLeft:
		case DeviceOrientation.LandscapeRight:
			return FageUIRoot.Instance.FindUIDetail(onLandscape);
		default:
			return FageUIRoot.Instance.FindUIDetail(onPortrait);
		}
	}

	public	List<string> GetBundles() {
		List<string> result = new List<string>();
		FageUIRoot root = FageUIRoot.Instance;
		FageUIDetail uiDetail = root.FindUIDetail(onPortrait);
		if ((uiDetail!=null) && (!result.Contains(uiDetail.bundle))) {
			result.Add(uiDetail.bundle);
		}
		return result;
	}
}

public	class FageUIDetail {
	[XmlAttribute("id")]
	public	string		id;
	[XmlAttribute("asset")]
	public	string		asset;
	[XmlAttribute("bundle")]
	public	string		bundle;
	[XmlAttribute("posX")]
	public	float		posX;
	[XmlAttribute("posY")]
	public	float		posY;
	[XmlAttribute("posZ")]
	public	float		posZ;
	[XmlAttribute("rotX")]
	public	float		rotX;
	[XmlAttribute("rotY")]
	public	float		rotY;
	[XmlAttribute("rotZ")]
	public	float		rotZ;
	[XmlAttribute("scaleX")]
	public	float		scaleX;
	[XmlAttribute("scaleY")]
	public	float		scaleY;
	[XmlAttribute("scaleZ")]
	public	float		scaleZ;
	[XmlAttribute("colorR")]
	public	float		colorR;
	[XmlAttribute("colorG")]
	public	float		colorG;
	[XmlAttribute("colorB")]
	public	float		colorB;
	[XmlAttribute("colorA")]
	public	float		colorA;
	[XmlAttribute("onInstantiate")]
	public	string		onInstantiate;
	[XmlAttribute("onDestroy")]
	public	string		onDestroy;
	[XmlAttribute("onPause")]
	public	string		onPause;
	[XmlAttribute("onResume")]
	public	string		onResume;
	[XmlAttribute("onSwitchIn")]
	public	string		onSwitchIn;
	[XmlAttribute("onSwitchOut")]
	public	string		onSwitchOut;

	public	Vector3 GetPosition() {
		return new Vector3 (posX, posY, posZ);
	}

	public	Quaternion GetRotation() {
		return new Quaternion (rotX, rotY, rotZ, 1f);
	}

	public	Vector3 GetScale() {
		return new Vector3 (scaleX, scaleY, scaleZ);
	}

	public	Color	GetColor() {
		return new Color(colorR, colorG, colorB, colorA);
	}

	private	byte WhichTransitionNeed(FageUITransition t) {
		byte result = FageUITransition.NONE;
		if (t!=null) {
			if (t.posX!=posX || t.posY!=posY || t.posZ!=posZ) {
				result |= FageUITransition.POSITION;
			}
			if (t.rotX!=rotX || t.rotY!=rotY || t.rotZ!=rotZ) {
				result |= FageUITransition.ROTATION;
			}
			if (t.scaleX!=scaleX || t.scaleY!=scaleY || t.scaleZ!=scaleZ) {
				result |= FageUITransition.SCALE;
			}
			if (t.colorR!=colorR || t.colorG!=colorG || t.colorB!=colorB) {
				result |= FageUITransition.COLOR;
			} else if (t.colorA!=colorA) {
				result |= FageUITransition.ALPHA;
			}
		}
		return result;
	}

	public	FageUITransition GetTransitionOnInstantiate() {
		return FageUIRoot.Instance.FindUITransition (onInstantiate);
	}

	public	byte WhichTransitionOnInstantiate() {
		return WhichTransitionNeed(GetTransitionOnInstantiate());
	}

	public	FageUITransition GetTransitionOnDestroy() {
		return FageUIRoot.Instance.FindUITransition (onDestroy);
	}

	public	byte WhichTransitionOnDestroy() {
		return WhichTransitionNeed(GetTransitionOnDestroy());
	}

	public	FageUITransition GetTransitionOnPause() {
		return FageUIRoot.Instance.FindUITransition (onPause);
	}

	public	byte WhichTransitionOnPause() {
		return WhichTransitionNeed(GetTransitionOnPause());
	}

	public	FageUITransition GetTransitionOnResume() {
		return FageUIRoot.Instance.FindUITransition (onResume);
	}

	public	byte WhichTransitionOnResume() {
		return WhichTransitionNeed(GetTransitionOnResume());
	}

	public	FageUITransition GetTransitionOnSwitchIn() {
		return FageUIRoot.Instance.FindUITransition (onSwitchIn);
	}

	public	byte WhichTransitionOnSwitchIn() {
		return WhichTransitionNeed(GetTransitionOnSwitchIn());
	}

	public	FageUITransition GetTransitionOnSwitchOut() {
		return FageUIRoot.Instance.FindUITransition (onSwitchOut);
	}

	public	byte WhichTransitionOnSwitchOut() {
		return WhichTransitionNeed(GetTransitionOnSwitchOut());
	}
}

public	class FageUITransition {
	public	const byte NONE		= 0x00;
	public	const byte POSITION = 0x01;
	public	const byte ROTATION = 0x02;
	public	const byte SCALE	= 0x04;
	public	const byte COLOR	= 0x08;
	public	const byte ALPHA	= 0x10;

	[XmlAttribute("id")]
	public	string		id;
	[XmlAttribute("posX")]
	public	float		posX;
	[XmlAttribute("posY")]
	public	float		posY;
	[XmlAttribute("posZ")]
	public	float		posZ;
	[XmlAttribute("rotX")]
	public	float		rotX;
	[XmlAttribute("rotY")]
	public	float		rotY;
	[XmlAttribute("rotZ")]
	public	float		rotZ;
	[XmlAttribute("scaleX")]
	public	float		scaleX;
	[XmlAttribute("scaleY")]
	public	float		scaleY;
	[XmlAttribute("scaleZ")]
	public	float		scaleZ;
	[XmlAttribute("colorR")]
	public	float		colorR;
	[XmlAttribute("colorG")]
	public	float		colorG;
	[XmlAttribute("colorB")]
	public	float		colorB;
	[XmlAttribute("colorA")]
	public	float		colorA;
	[XmlAttribute("time")]
	public	float		time;
	[XmlAttribute("delay")]
	public	float		delay;
	[XmlAttribute("easetype")]
	public	LeanTweenType ease;

	public	FageUITransition() {
		posX = posY = posZ = 0f;
		rotX = rotY = rotZ = 1f;
		scaleX = scaleY = scaleZ = 1f;
		time = delay = 0f;
		ease = LeanTweenType.linear;
	}

	public	Vector3 GetPosition() {
		return new Vector3 (posX, posY, posZ);
	}
	
	public	Quaternion GetRotation() {
		return new Quaternion (rotX, rotY, rotZ, 1f);
	}
	
	public	Vector3 GetScale() {
		return new Vector3 (scaleX, scaleY, scaleZ);
	}

	public	Color GetColor() {
		return new Color(colorR, colorG, colorB, colorA);
	}
}