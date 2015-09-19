using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[AddComponentMenu("JellyCraft/Title/TitleMenuGroup")]
public	class TitleMenuGroup : MonoBehaviour {
	public	TitleMenuGroup		parentGroup;
	public	TitleMenuButton		parentButton;
	public	TitleMenuButton[]	buttons;
	public	TitleMenuText		textGroup;

	private	int		clicked;
	private	int		offset;
	private	bool	block;

	void Awake() {
		if ((parentGroup!=null) || (parentButton!=null)) {
			FoldImmediately();
		}
	}

	public	LTDescr Fold(System.Action callback) {
		if (textGroup!=null)
			textGroup.Fold(0f);
		return LeanTween.scale(transform as RectTransform, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInBack).setOnComplete(callback);
	}

	public	void FoldImmediately() {
		if (textGroup!=null)
			textGroup.FoldImmediately();
		(transform as RectTransform).localScale = Vector3.zero;
	}

	public	LTDescr Unfold(System.Action callback) {
		if (textGroup!=null)
			textGroup.Unfold(0.3f);
		return LeanTween.scale(transform as RectTransform, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(callback);
	}

	public	void UnfoldImmediately() {
		if (textGroup!=null)
			textGroup.UnfoldImmediately();
		(transform as RectTransform).localScale = Vector3.one;
	}

	public	void BlockButtons(TitleMenuButton except) {
		clicked = Array.IndexOf(buttons, except);
		offset = 0;
		block = true;
		InvokeRepeating("InvokeBlock", 0.1f, 0.1f);
		if ((parentButton!=null) && (parentGroup!=null))
			parentButton.block = true;
	}

	public	void UnblockButtons(TitleMenuButton except) {
		clicked = Array.IndexOf(buttons, except);
		offset = 0;
		block = false;
		InvokeRepeating("InvokeBlock", 0.05f, 0.05f);
		if (parentButton!=null)
			parentButton.block = false;
	}

	private	void InvokeBlock() {
		offset++;
		int left = clicked - offset;
		int right = clicked + offset;
		bool blockL = (left >= 0);
		bool blockR = (right < buttons.Length);
		
		if (blockL)
			buttons[left].block = block;
		if (blockR)
			buttons[right].block = block;
		
		if ((!blockL) && (!blockR))
			CancelInvoke("InvokeBlock");
	}
}