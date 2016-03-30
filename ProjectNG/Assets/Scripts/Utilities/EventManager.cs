using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

	private Dictionary <string , UnityEvent> EventDictionary;

	private static EventManager eventManager;

	/*Iskah da go napravq po toq nachin ama neshtata se oburkaha :D 
	 * 
	 * private static EventManager instance 
	 
	{
		get
		{
			if (!eventManager) {
				eventManager = FindObjectOfType (typeof(EventManager))as EventManager;

				if (!eventManager) {

					Debug.Log ("No event manager(script) in scene");
				}
			} else {
				eventManager.Init ();
			}
			return eventManager;
		}
	}*/
	/*void Init (){

	if(EventDictionary == null) EventDictionary = new Dictionary<string,UnityEvent>();
}
*/

	void Awake(){
		eventManager = GameObject.Find ("ScriptHolder").GetComponent<EventManager>();
		eventManager.EventDictionary = new  Dictionary<string,UnityEvent>();
	}



	public static void StartListening(string eventName, UnityAction listener) {
		//Pre: name of event and function pointer 

		    UnityEvent thisEvent = null;
		if (eventManager.EventDictionary.TryGetValue (eventName, out thisEvent)) {
			//tryGetValue (more efficient than GetValue+CatchException)
		
			thisEvent.AddListener (listener);

			} 
		else {
			//if doesn't exist create a new entry 
			thisEvent = new UnityEvent ();
			thisEvent.AddListener (listener);
			//Debug.Log (thisEvent.GetHashCode());
			eventManager.EventDictionary.Add (eventName, thisEvent);
		
			}
    //funcEnd
	}

	public static void StopListening(string eventName, UnityAction listener){
	
		if (eventManager == null)
			return;

		UnityEvent thisEvent = null;

		if (eventManager.EventDictionary.TryGetValue (eventName, out thisEvent)) {
			//tryGetValue (more efficient than GetValue+CatchException)

			thisEvent.RemoveListener (listener);

		} 
	//funcEnd		
	}

	public static void TriggerEvent(string eventName){
	
		UnityEvent thisEvent = null;
		if (eventManager.EventDictionary.TryGetValue (eventName, out thisEvent)) {
			//tryGetValue (more efficient than GetValue+CatchException)
			//if(thisEvent==null)Debug.Log("doesn'tFinnd");
			Debug.Log (thisEvent.GetHashCode());
			thisEvent.Invoke();


		} 
	
	}

}