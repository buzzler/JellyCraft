using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("JellyCraft/Title/TitleMenuText")]
public class TitleMenuText : MonoBehaviour {
	public	Text[]	texts;

	public	void Fold(float delay) {
		LeanTween.value(gameObject, Color.white, new Color(1f,1f,1f,0f), 0.2f).setDelay(delay).setOnUpdate(OnUpdateColor);
	}

	public	void FoldImmediately() {
		OnUpdateColor(new Color(1f,1f,1f,0f));
	}

	public	void Unfold(float delay) {
		LeanTween.value(gameObject, new Color(1f,1f,1f,0f), Color.white, 0.2f).setDelay(delay).setOnUpdate(OnUpdateColor);
	}

	public	void UnfoldImmediately() {
		OnUpdateColor(Color.white);
	}

	private	void OnUpdateColor(Color value) {
		for (int i = 0 ; i < texts.Length ; i++) {
			texts[i].color = value;
		}
	}
}
