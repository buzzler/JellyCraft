using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AdventureScreen : MonoBehaviour, IFageUIComponent {
	/**
	 * core variables
	 **/
	public	const int		ROW		= 4;
	public	const int		COLUMN	= 4;
	public	Image			imageFrame;
	public	Image[]			imageSlots;
	public	SlotComponent[]	slots;
	public	BoxComponent[]	boxPrefabs;
//	public	EffectType[]	effectBang;
//	public	EffectType[]	effectCoin;
//	public	EffectType[]	effectCombo;
	private Dictionary<string, BoxComponent> boxes;
	private int				count;
	private	int				boxCount;
	private	bool			blocked;

	/**
	 * input variables
	 **/
	private float	thresholdInch;	// in inch
	private float	threshold;		// in pixel
	private float	distance;		// in pixel
	private Vector2	pivot;
	private bool	pressed;

	private	void Init() {
		boxes = new Dictionary<string, BoxComponent>();
		count = 0;
		boxCount = 0;

		sensitivity = 0.4f;
		ClearInput();
	}

	public	void OnUIInstantiate(FageUIMem mem, params object[] param) {
		Init();
	}

	public	void OnUIInstantiated(FageUIMem mem) {
		FageUIManager.Instance.Popup(FageUIRoot.Instance.FindUISet("adventure_standby"), OnStandbyComplete as System.Action);
		Flash(imageFrame, 1f, 0f, 0.25f);
		for (int i = 0 ; i < imageSlots.Length ; i++) {
			Flash(imageSlots[i], 1f);
		}
		Invoke("FlashType"+Random.Range(1,3).ToString(), 1f);
	}

	private	void OnStandbyComplete() {
		RandomNew();
		RandomNew();
		CameraEffector.Instance.SetPreset("adventure", 0.5f);
	}

	public	void OnUIDestroy(FageUIMem mem) {}
	public	void OnSwitchOut(FageUIMem mem) {}
	public	void OnSwitchIn(FageUIMem mem) {}
	public	void OnUIPause(FageUIMem mem) {}

	public	void OnUIResume(FageUIMem mem, params object[] param) {
		Init();
	}

	public	void OnUIResumed(FageUIMem mem) {}

	public	GameObject	GetGameObject() {
		return gameObject;
	}

	private	void FlashType1() {
		float sec = 0.8f;
		float step = 0.05f;
		float delay = 0f;

		for (int i = 0 ; i < 4 ; i++) {
			Flash(imageSlots[i], sec, delay);
			delay+=step;
		}
		for (int i = 7 ; i >= 4 ; i--) {
			Flash(imageSlots[i], sec, delay);
			delay+=step;
		}
		for (int i = 8 ; i < 12 ; i++) {
			Flash(imageSlots[i], sec, delay);
			delay+=step;
		}
		for (int i = 15 ; i >= 12 ; i--) {
			Flash(imageSlots[i], sec, delay);
			delay+=step;
		}
	}

	private	void FlashType2() {
		float sec = 0.8f;
		float step = 0.1f;
		float delay = 0f;

		Flash(imageSlots[3], sec, delay);
		delay+=step;
		for (int i = 0 ; i < 2 ; i++) {
			Flash(imageSlots[2+i*5], sec, delay);
		}
		delay+=step;
		for (int i = 0 ; i < 3 ; i++) {
			Flash (imageSlots[1+i*5], sec, delay);
		}
		delay+=step;
		for (int i = 0 ; i < 4 ; i++) {
			Flash (imageSlots[i*5], sec, delay);
		}
		delay+=step;
		for (int i = 0 ; i < 3 ; i++) {
			Flash (imageSlots[4+i*5], sec, delay);
		}
		delay+=step;
		for (int i = 0 ; i < 2 ; i++) {
			Flash (imageSlots[8+i*5], sec, delay);
		}
		delay+=step;
		Flash (imageSlots[12], sec, delay);
	}

	private	void FlashType3() {
		float sec = 0.8f;
		float step = 0.05f;
		float delay = 0f;

		for (int i = 12 ; i >= 0 ; i-=4) {
			Flash(imageSlots[i], sec, delay);
			delay += step;
		}
		for (int i = 1 ; i < 16 ; i+=4) {
			Flash(imageSlots[i], sec, delay);
			delay += step;
		}
		for (int i = 14 ; i >= 0 ; i-=4) {
			Flash(imageSlots[i], sec, delay);
			delay += step;
		}
		for (int i = 3 ; i < 16 ; i+=4) {
			Flash(imageSlots[i], sec, delay);
			delay += step;
		}
	}

	private	void Flash(Image image, float second, float delay = 0f, float to = 0f) {
		LeanTween.alpha(image.rectTransform, to, second).setDelay(delay).setFrom(1f).setEase(LeanTweenType.easeOutCubic);
	}

	private bool AppendScore(int level) {
		return false;
	}

	private	void Victory() {
		CameraEffector.Instance.SetPreset("victory", 0.5f);
		FageUIManager.Instance.Popup(FageUIRoot.Instance.FindUISet("adventure_victory"));
	}

	private	void NoMoreMove() {
		CameraEffector.Instance.SetPreset("gameover", 1f);
		FageUIManager.Instance.Popup(FageUIRoot.Instance.FindUISet("adventure_gameover"));
	}

	void OnDisable() {
		CancelInvoke ();
	}

	public	int GetBoxCount() {
		return boxCount;
	}
	
	public void RandomNew() {
		New(Random.value<0.05 ? 1:0);
	}
	
	public BoxComponent New(int level = 0, SlotComponent slot = null) {
		level = Mathf.Clamp(level, 0, boxPrefabs.Length-1);
		
		// set position
		if (slot==null) {
			ArrayList list = new ArrayList();
			foreach(SlotComponent s in slots) {
				if (s.IsEmpty) {
					list.Add(s);
				}
			}
			if (list.Count==0) {
				Debug.LogError("nowhere found to create box");
				return null;
			}
			slot = list[(Random.Range(0, list.Count))] as SlotComponent;
		}
		
		// instantiate box
		count++;
		boxCount++;
		BoxComponent box = Instantiate(boxPrefabs[level], slot.transform.position, Quaternion.identity) as BoxComponent;
		box.transform.SetParent(transform, false);
		box.transform.position = slot.transform.position;
		box.Init (count.ToString (), level, slot);
		slot.ReserveHold(box);
		boxes.Add(box.id, box);
		
		return box;
	}

	public void Left() {
		int count = 0;
		if (IsMovable()) {
			for (int x = 0 ; x < COLUMN ; x++) {
				for (int y = 0 ; y < ROW ; y++) {
					BoxComponent box = slots[y*COLUMN+x].box;
					if (box) {
						if (box.Left()) {
							count++;
						}
					}
				}
			}
		}
	}
	
	public void Right() {
		int count = 0;
		if (IsMovable()) {
			for (int x = COLUMN-1 ; x >= 0 ; x--) {
				for (int y = 0 ; y < ROW ; y++) {
					BoxComponent box = slots[y*COLUMN+x].box;
					if (box) {
						if (box.Right()) {
							count++;
						}
					}
				}
			}
		}
	}
	
	public void Up() {
		int count = 0;
		if (IsMovable()) {
			for (int y = 0 ; y < ROW ; y++) {
				for (int x = 0 ; x < COLUMN ; x++) {
					BoxComponent box = slots[y*COLUMN+x].box;
					if (box) {
						if (box.Up()) {
							count++;
						}
					}
				}
			}
		}
	}
	
	public void Down() {
		int count = 0;
		if (IsMovable()) {
			for (int y = ROW-1 ; y >= 0 ; y--) {
				for (int x = COLUMN-1 ; x >= 0 ; x--) {
					BoxComponent box = slots[y*COLUMN+x].box;
					if (box) {
						if (box.Down()) {
							count++;
						}
					}
				}
			}
		}
	}
	
	private bool IsMovable() {
		if (blocked) {
			return false;
		}
		foreach (BoxComponent box in boxes.Values) {
			if (box.moving) {
				return false;
			}
		}
		return true;
	}
	
	public	void OnErase(SlotComponent slot) {
		BoxComponent box = slot.box;
		slot.Clear ();
		boxCount--;
		boxes.Remove(box.id);
		GameObject.DestroyImmediate (box.gameObject);
//		AudioPlayerComponent.Play ("fx_combo");
//		EffectComponent.Show (effectBang [0], slot.transform.position);
		blocked = false;
	}
	
	public void OnMerge(SlotComponent slot) {
		BoxComponent box1 = slot.box;
		BoxComponent box2 = slot.target;
		
		int level = box1.level+1;
		if (level >= boxPrefabs.Length) {
			Debug.LogError("biggest box can't merge");
			return;
		}
		
		slot.Clear();
		boxCount -= 2;
		boxes.Remove(box1.id);
		boxes.Remove(box2.id);
		GameObject.DestroyImmediate(box1.gameObject);
		GameObject.DestroyImmediate(box2.gameObject);
		New(level, slot);
		// insert score increament
		bool isWin = AppendScore (level);
		
//		Vector3 pos = slot.transform.position;
		
		// insert effect 'bang'
//		EffectComponent.Show (effectBang [level], pos);
		
		// insert coin increment
//		if (EffectComponent.Show (effectCoin [level], pos) != null) {
//			AudioPlayerComponent.Play ("fx_coin");
//			game.AppendCoin();
//		}
		
		// insert effect 'combo'
//		if (EffectComponent.Show (effectCombo [(Mathf.Min(combo, effectCombo.Length-1))], pos) != null) {
//			AudioPlayerComponent.Play ("fx_combo");
//		}
		
		if (isWin) {
			Victory();
			blocked = true;
		} else {
			OnMoved(slot);
		}
	}
	
	public void OnMoved(SlotComponent slot) {
		if (IsMovable()) {
			Flash(imageFrame, 0.3f, 0f, 0.25f);
			RandomNew();
			
			bool gameover = true;
			foreach (SlotComponent s in slots) {
				if (s.IsEmpty || s.IsNeighbor()) {
					gameover = false;
					break;
				}
			}
			
			if (gameover) {
				NoMoreMove();
				blocked = true;
			}
		}
	}

	void Update () {
		if (blocked)
			return;

		if (Input.touchCount > 0) {
			Touch t = Input.GetTouch(0);
			switch (t.phase) {
			case TouchPhase.Began:
				if (!pressed) {
					pivot = t.position;
					distance = 0;
					pressed = true;
				}
				break;
			case TouchPhase.Ended:
				if (pressed)
					CheckInput(t.position);
				break;
			case TouchPhase.Moved:
				if (pressed)
					CheckInput(t.position);
				break;
			}
			
		} else {
			ClearInput();
		}

		if (Input.GetKeyDown("left")) {
			Left();
		}
		if (Input.GetKeyDown("right")) {
			Right();
		}
		if (Input.GetKeyDown("up")) {
			Up();
		}
		if (Input.GetKeyDown("down")) {
			Down();
		}
	}

	public	float sensitivity {
		set {
			thresholdInch = value;
			threshold = (Screen.dpi>0) ? Screen.dpi * thresholdInch:300*thresholdInch;
			ClearInput();
		}
		
		get {
			return thresholdInch;
		}
	}
	
	private	void ClearInput() {
		pivot = Vector2.zero;
		distance = 0;
		pressed = false;
	}
	
	private	void CheckInput(Vector2 position) {
		distance = Vector2.Distance(pivot, position);
		if (distance > threshold) {
			Vector2 direct = position - pivot;
			if (Mathf.Abs(direct.x) <= Mathf.Abs(direct.y)) {
				if (direct.y > 0) {
					Up();
				} else {
					Down();
				}
			} else {
				if (direct.x > 0) {
					Right();
				} else {
					Left();
				}
			}
			ClearInput();
		}
	}
}
