using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

 

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI()
    {
       if(GUI.Button(new Rect(800,200,200,50),"Play"))
       {
            Application.LoadLevel("Stage1");
       }

       if(GUI.Button(new Rect(800, 265, 200, 50), "QUIT"))
       {
            Application.Quit();
       }
    }


}
