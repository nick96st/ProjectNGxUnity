using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class MoveCurveY : MonoBehaviour {


	[SerializeField]
	private float height, width;
	// height sets height .. 
	//the less the witdh the further it jumps
	private Vector3 baseVector;
	public GameObject ActorObj;

	private UnityAction jumpListener ;

	// Use this for initialization
	void Awake () {
		
		Debug.Log (this.gameObject.tag);
		UnityAction jumpListener = new UnityAction (StartJumpCoroutine);
		// checker which this.gameObject refers
	}


	void OnEnable(){
		
		UnityAction jumpListener = new UnityAction (StartJumpCoroutine);
		EventManager.StartListening("jump",jumpListener);
	}

	void OnDisable(){
		UnityAction jumpListener = new UnityAction (StartJumpCoroutine);
		EventManager.StopListening("jump",jumpListener);
	}

	void StartJumpCoroutine(){
		Debug.Log ("starts to jump");
		StartCoroutine(JumpByCurve(new Vector3(5,0 ,0)));//must be invoked with proper value

	}


	IEnumerator JumpByCurve(Vector3 directionJump) {
		//Pre: DirectionJump has the proper direction to jump over wall

		directionJump.Normalize ();// used to shift the object in any x-z direction

		if (ActorObj != null) {
			double x = 0f;
			baseVector = new Vector3 (ActorObj.transform.position.x,
								 ActorObj.transform.position.y, 
					    		 ActorObj.transform.position.z);
			// TODO: set ActorObj.Stats.State = 0;// uninterractable with other inputs

			while (x < 3.1415) { //P radians

				// using sin function to get a leap displacement

				ActorObj.transform.position = baseVector + ((float)x) * directionJump / width + new Vector3(0,height * Mathf.Sin((float)x),0);
				Debug.Log (ActorObj.transform.position);
				x += 0.05f;
				yield return new WaitForFixedUpdate();
			}

			yield return null;

		} else {
			Debug.Log ("Jump asked but cannot be perfomed becasue no gameObject attached");
			yield return null;
		}
	}
}
