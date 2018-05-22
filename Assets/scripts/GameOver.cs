using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnGUI()
    {
        GUI.contentColor = Color.red;
        if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2 + 150, 100, 40), "Retry"))
        {
            SceneManager.LoadSceneAsync("tgk", LoadSceneMode.Single);
        }

        if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 200, 100, 40), "Quit"))
        {
            Application.Quit();
        }
    }
}
