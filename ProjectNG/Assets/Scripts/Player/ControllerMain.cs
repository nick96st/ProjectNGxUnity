using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class ControllerMain : MonoBehaviour {
    // SCRIPT WILL CONTROL THE FLOW OF BASIC INPUT SUCH AS MOUSECLICKS 
    // 

    // for UI popup
    UITasks.Task giveTask;
    public static bool spawnedUI = false;
    Vector3 mousePos; // to be used where the Ui should appear

    public GameObject ScriptHolder;// reference to other static scripts

	// raycasting 
	RaycastHit hit1;
	Ray mouseRay;

	void Start () {

	}


	void Update () 
	{    mousePos = Input.mousePosition;
		 mouseRay = Camera.main.ScreenPointToRay (mousePos);
			if(Input.GetMouseButtonDown(0))
		{
            Debug.Log(spawnedUI);
			if(!EventSystem.current.IsPointerOverGameObject()) // blocks click on overlaying UI with object
			{

			  ScriptHolder.GetComponent<UITasks>().unEnableThis();// set UI script inactive

			if(Physics.Raycast(mouseRay,out hit1,10000))
				{   UITasks.mousePos = mousePos;
					UITasks.Target   = hit1.transform.gameObject;
			
                    /*
                    set task to needed where task is the the type of object you clicked

                    */
							if(hit1.transform.gameObject.tag=="Terrain")
								{
									giveTask = UITasks.Task.Ground;
									Debug.Log("clicked on ground");
								}
							else if(hit1.transform.gameObject.tag=="Player")
								{
                        giveTask = UITasks.Task.PtoP; 
									Debug.Log("clicked on player");			
								}
							else if(hit1.transform.gameObject.tag=="Lootable")
								{
                        giveTask = UITasks.Task.Lootable;//  task 3 =Lootable 
								Debug.Log("clicked on lootable");
								//ScriptHolder.g
								}
							else if(hit1.transform.gameObject.tag=="NPC")
								{
                        giveTask = UITasks.Task.NPC;
								Debug.Log("clicked on NPC");
								}
							else if(hit1.transform.gameObject.tag=="Door")
								{
                        giveTask = UITasks.Task.Door;
									Debug.Log("Door");
							    }
			                else {
                        giveTask = UITasks.Task.Untagged;	
									Debug.Log("Untagged");
									}
                    //	Debug.Log(UITasks.task.ToString());
                  

                     if (giveTask != UITasks.Task.Untagged)
                    {
                        // if it is interactable
                        spawnedUI = true;
                        ScriptHolder.GetComponent<UITasks>().SpawnUI(giveTask);
                    }
                     // despawn is it doesnt spawn new
                    else if (spawnedUI == true)
                    {
                        spawnedUI = false;
                        ScriptHolder.GetComponent<UITasks>().unEnableThis();

                    }
                }

			}
		}
	}
}
