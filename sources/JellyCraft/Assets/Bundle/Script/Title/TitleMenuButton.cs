using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("JellyCraft/Title/TitleMenuButton")]
[RequireComponent(typeof(Button))]
public	class TitleMenuButton : MonoBehaviour {
	public	TitleMenuGroup	parent;
	public	TitleMenuGroup	child;
	public	string			effectFold;
	public	string			effectUnfold;
	private	bool			_fold;
	private	bool			_folding;
	private	bool			_block;
	private	bool			_unavailable;
	private	Button			_button;
	
	void Awake() {
		_fold = true;
		_folding = false;
		_block = false;
		_unavailable = false;
		_button = GetComponent<Button>();
	}

	public	bool			block {
		get { return _block; }
		set {
			_block = value;
			_button.interactable = (!_block) && (!_unavailable);
		}
	}

	public	bool			unavailable {
		get { return _unavailable; }
		set {
			_unavailable = value;
			_button.interactable = (!_block) && (!_unavailable);
		}
	}

	public	void OnClick() {
		if (!_folding)
			Toggle();
	}
	
	public	void Toggle() {
		if (_fold) {
			Unfold();
		} else {
			Fold();
		}
	}
	
	public	void Fold() {
		if (child!=null) {
			child.Fold(OnFoldComplete);
		}

		if (parent!=null) {
			parent.UnblockButtons(this);
		}
		_folding = true;
	}

	private	void OnFoldComplete() {
		_fold = true;
		_folding = false;

		if (!string.IsNullOrEmpty(effectFold))
			CameraEffector.Instance.SetPreset(effectFold, 0.3f);
	}
	
	public	void Unfold() {
		if (child!=null) {
			child.Unfold(OnUnfoldComplete);
		}

		if (parent!=null) {
			parent.BlockButtons(this);
		}
		_folding = true;
	}

	private	void OnUnfoldComplete() {
		_fold = false;
		_folding = false;

		if (!string.IsNullOrEmpty(effectUnfold))
			CameraEffector.Instance.SetPreset(effectUnfold, 0.6f);
	}
}