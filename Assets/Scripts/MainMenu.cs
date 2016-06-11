using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void LoadStartScene ()
    {
        Application.LoadLevel("Start");
    }

    public void LoadOptionsScene()
    {
        Application.LoadLevel("Options");
    }

}
