using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    bool dialogActive = false;
    [SerializeField] private GameObject quitDialog;

    // Use this for initialization
    void Start()
    {
        quitDialog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            dialogActive = !dialogActive;
            quitDialog.SetActive(dialogActive);
        }
    }

    public void ResumeGame()
    {
        dialogActive = false;
        quitDialog.SetActive(dialogActive);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
