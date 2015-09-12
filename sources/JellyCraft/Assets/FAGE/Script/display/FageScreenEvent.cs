using UnityEngine;
using System.Collections;

public class FageScreenEvent : FageEvent {
	public	const string ORIENTATION	= "screenOrientation";
	public	const string SIZE			= "screenSize";
	public	const string PPI			= "screenPPI";

	public	DeviceOrientation	lastOrientation;
	public	DeviceOrientation	currentOrientation;
	public	int					lastWidth;
	public	int					lastHeight;
	public	int					currentWidth;
	public	int					currentHeight;
	public	float				lastPPI;
	public	float				currentPPI;

	public	FageScreenEvent(DeviceOrientation lastOrientation, DeviceOrientation currentOrientation) : base(ORIENTATION) {
		this.lastOrientation = lastOrientation;
		this.currentOrientation = currentOrientation;
	}

	public	FageScreenEvent(int lastWidth, int lastHeight, int currentWidth, int currentHeight) : base(SIZE) {
		this.lastWidth = lastWidth;
		this.lastHeight = lastHeight;
		this.currentWidth = currentWidth;
		this.currentHeight = currentHeight;
	}

	public	FageScreenEvent(float lastPPI, float currentPPI) : base(PPI) {
		this.lastPPI = lastPPI;
		this.currentPPI = currentPPI;
	}
}
