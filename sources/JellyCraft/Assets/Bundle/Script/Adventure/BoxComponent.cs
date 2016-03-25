using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class BoxComponent : MonoBehaviour {
	
	public	string id;
	public	int level;
	private	SlotComponent current;
	private	Animator animator;
	private	bool tween;
	
	void Awake() {
		animator = GetComponent(typeof(Animator)) as Animator;
	}
	
	void OnEnable() {
		animator.SetTrigger("trigger_init");
	}
	
	public	void Init(string id, int level, SlotComponent slot) {
		this.id = id;
		this.level = level;
		this.current = slot;
	}
	
	public bool moving {
		get {
			return tween;
		}
	}
	
	private bool Move(SlotComponent slot, int step) {
		if (current!=slot) {
			if (slot.IsEmpty) {
				slot.ReserveHold(this);
				SetSlot(slot);
			} else if (slot.IsMergable(this)) {
				slot.ReserveMerger(this);
				SetSlot(slot);
			} else {
				Debug.LogError("maybe wrong box moving logic (id:" + id + ")");
				return false;
			}
			float t = Mathf.Clamp(Mathf.Abs(step), 0, 3) * 0.05f;
			tween = true;
			LeanTween.move(gameObject, slot.transform.position, t).setEase(LeanTweenType.linear).setOnComplete(HandleMove);
			return true;
		}
		return false;
	}
	
	public bool Left() {
		if (current) {
			SlotComponent mLeft = current.MostLeft(this);
			if (Move (mLeft, mLeft.x - current.x)) {
				animator.SetTrigger("trigger_left");
			} else {
				return false;
			}
		} else {
			Debug.LogError("slot reference missing (id:" + id + ")");
		}
		return true;
	}
	
	public bool Right() {
		if (current) {
			SlotComponent mRight = current.MostRight(this);
			if (Move (mRight, mRight.x - current.x)) {
				animator.SetTrigger("trigger_right");
			} else {
				return false;
			}
		} else {
			Debug.LogError("slot reference missing (id:" + id + ")");
		}
		return true;
	}
	
	public bool Up() {
		if (current) {
			SlotComponent mUp = current.MostUp(this);
			if (Move (mUp, mUp.y - current.y)) {
				animator.SetTrigger("trigger_up");
			} else {
				return false;
			}
		} else {
			Debug.LogError("slot reference missing (id:" + id + ")");
		}
		return true;
	}
	
	public bool Down() {
		if (current) {
			SlotComponent mDown = current.MostDown(this);
			if (Move (mDown, mDown.y - current.y)) {
				animator.SetTrigger("trigger_down");
			} else {
				return false;
			}
		} else {
			Debug.LogError("slot reference missing (id:" + id + ")");
		}
		return true;
	}
	
	public void SetSlot(SlotComponent slot) {
		if (current!=null) {
			current.Clear();
		}
		current = slot;
	}
	
	public void HandleMove() {
		animator.SetTrigger ("trigger_stop");
		tween = false;
		current.HandleMove(this);
	}
	
	public	void OnClick() {
		SendMessageUpwards ("OnErase", current);
	}
}
