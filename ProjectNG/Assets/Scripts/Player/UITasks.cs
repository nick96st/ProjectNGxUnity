using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/*  this script instanciates the buttons and apply everything they need to be able to open close
      

*/ 

public class UITasks : MonoBehaviour {
	public enum Task{
		Lootable = 1, // controls for loot : (Loot)/(Unlock)/(SearchDeeply)  1/3
		Ground   = 2, //move/move and shoot  (Move)/Run&Gun                  1/2   
		NPC  = 3,     //NPC interaction UI(shoot,talk,daze(if close))        2/3
		Door = 4,     // control doors     (Lock/unlock)/(Close/Open)        2/2
		Moveable = 5, // block path or free path Pull*/Push/Left/Right       3/4
		PtoP     = 6, //Player to Pl trading/treat wounds/Comfort/Encourage  1/4  
		Untagged = 0 
	}


	//public static Task task;
	public static GameObject Target;
	public static Vector3 mousePos;


	public GameObject ButtonInstance;
	RectTransform UIRect=new RectTransform();
	Rect UIRect1;
	GUILayout LayoutUI;
	byte buttonNeeded;
	string[] ButtonText = new string[4];              //sets text to be filled when buttons get spawned
	GameObject[] ActionUI = new GameObject[4]; 
	Button[] ActionUIButton=new Button[4];

	public void unEnableThis()
	{   
		for(int j=0;j<4;j++)
			if(ActionUI[j]!=null)
		Destroy(ActionUI[j]);

		GameObject.Find ("Main Camera").GetComponent <CameraMovement>().enabled = true;     // allows camera movement
		this.GetComponent<UITasks> ().enabled = false;
		}


	void Awake () {
		

		UIRect1.x = mousePos.x;
		UIRect1.y = mousePos.y;
		UIRect1.width = Screen.width  / 10;
		UIRect1.height = Screen.height/ 10;

	

			//ActionUI[i].enabled=false;
			//ActionUI[i].GetComponent<RectTransform>().=UIRect;


		}


    public void SpawnUI(Task task)
	{
            //block camera movement so it doesnt have to move the UI respectively 
            GameObject.Find("Main Camera").GetComponent<CameraMovement>().enabled = false;
            Debug.Log("tASK: " + task.ToString());


        /*
         way to construct 
         buttonNeeded = number it will need for sure;
         set those button to their respective names like
                ButtonText = "text";
        add ifs for each possible but not sure
        if(){
        ButtonText[buttonNeeded] = "text";
        buttonNeeded++;      
        } 
        */

        //examples given
        if (task == Task.PtoP)
        {
            buttonNeeded = 1;
            ButtonText[buttonNeeded - 1] = "Move";
        }
        else if (task == Task.Lootable)
        {
            ButtonText[0] = "Loot";
            ButtonText[1] = "Unlock";
            ButtonText[2] = "Inspect";
            buttonNeeded = 3;
        }


        /*   if (STATICS.ScreenResX == 1080)
           {   

               }*/
        // Set Size according to Res and ratio;
        UIRect1.width = Screen.width / 10;
        UIRect1.height = Screen.height / 100 * 5 + Screen.height % 10;

        // instanciate buttons 
        for (int i = 0; i < buttonNeeded; i++)
            {
              // CARE: this might force garbage collection from time to time since we make new objects
                ActionUI[i] = GameObject.Instantiate(ButtonInstance, new Vector3(mousePos.x, mousePos.y - UIRect1.height * i, 100), Quaternion.identity) as GameObject;
                ActionUI[i].transform.SetParent(GameObject.Find("Canvas").transform);
                //	fixes bugs with SetParent.transform
                ActionUI[i].GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                ActionUI[i].GetComponent<RectTransform>().sizeDelta = new Vector2(UIRect1.width, UIRect1.height);

                // setup listener for Onclick to despawn + act
                ActionUIButton[i] = ActionUI[i].GetComponent<Button>();
                ActionUIButton[i].GetComponentInChildren<Text>().text = (ButtonText[i]);
                ActionUIButton[i].onClick.RemoveAllListeners();
                ActionUIButton[i].onClick.AddListener(() => unEnableThis());

            }

	}
 

}
