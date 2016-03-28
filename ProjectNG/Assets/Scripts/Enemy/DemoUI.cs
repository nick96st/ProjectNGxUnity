using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DemoUI : MonoBehaviour 
{
    public GameObject DemoUI_Panel;
    
    void Start()
    {
        Invoke("DisablePanel", 10.0f);
    }

    public void DisablePanel()
    {
        DemoUI_Panel.SetActive(false);
    }
}