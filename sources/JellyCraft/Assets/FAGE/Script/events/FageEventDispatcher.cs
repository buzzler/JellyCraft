using UnityEngine;
using System.Collections;

[AddComponentMenu("Fage/Events/FageEventDispatcher")]
public	class FageEventDispatcher : MonoBehaviour {
	private	static event FageEventHandler dummy;
	private	Hashtable	event_hash	= new Hashtable ();

	void Awake() {
		event_hash	= new Hashtable ();
	}

	public	void AddEventListener(string type, FageEventHandler func) {
		if (event_hash.ContainsKey (type)) {
			FageEventHandler handler = event_hash [type] as FageEventHandler;
			handler += func;
			event_hash [type] = handler;
		} else {
			dummy += func;
			FageEventHandler handler = dummy.Clone() as FageEventHandler;
			dummy -= func;
			event_hash.Add (type, handler);
		}
	}
	
	public	void RemoveEventListener(string type, FageEventHandler func) {
		if (event_hash.ContainsKey (type)) {
			FageEventHandler handler = event_hash [type] as FageEventHandler;
			handler -= func;
			event_hash [type] = handler;
		}
	}
	
	public	void DispatchEvent(FageEvent fevent) {
		if ((fevent != null) && event_hash.ContainsKey (fevent.type)) {
			FageEventHandler handler = event_hash [fevent.type] as FageEventHandler;
			if (handler!=null) {
				handler (fevent);
			}
		}
	}
}