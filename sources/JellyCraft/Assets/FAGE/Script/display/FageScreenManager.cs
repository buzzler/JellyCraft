using UnityEngine;
using System.Collections;

[AddComponentMenu("Fage/Display/FageScreenManager")]
public class FageScreenManager : FageEventDispatcher {
	private	static FageScreenManager _instance;
	public	static FageScreenManager Instance { get { return _instance; } }
	private	DeviceOrientation	_lastOrientation;
	private	int					_lastWidth;
	private	int					_lastHeight;
	private	float				_lastDpi;

	public	DeviceOrientation	orientation { get { return _lastOrientation; } }
	public	int					width		{ get { return _lastWidth; } }
	public	int					height		{ get { return _lastHeight; } }
	public	float				dpi			{ get { return _lastDpi; } }

	void Awake() {
		_instance = this;
		_lastOrientation = Input.deviceOrientation;
		_lastWidth = Screen.width;
		_lastHeight = Screen.height;
		_lastDpi = Screen.dpi;
	}

	void Update () {
		ArrayList list = new ArrayList ();

		DeviceOrientation orientation = Input.deviceOrientation;
		if (_lastOrientation != orientation) {
			switch (orientation) {
			case DeviceOrientation.LandscapeLeft:
			case DeviceOrientation.LandscapeRight:
			case DeviceOrientation.Portrait:
			case DeviceOrientation.PortraitUpsideDown:
				list.Add (new FageScreenEvent (_lastOrientation, Input.deviceOrientation));
				_lastOrientation = orientation;
				break;
			}
		}
		if ((_lastWidth != Screen.width) || (_lastHeight != Screen.height)) {
			list.Add (new FageScreenEvent (_lastWidth, _lastHeight, Screen.width, Screen.height));
			_lastWidth = Screen.width;
			_lastHeight = Screen.height;
		}
		if (_lastDpi != Screen.dpi) {
			list.Add (new FageScreenEvent (_lastDpi, Screen.dpi));
			_lastDpi = Screen.dpi;
		}

		foreach (FageScreenEvent fsevent in list) {
			DispatchEvent(fsevent);
		}
	}
}