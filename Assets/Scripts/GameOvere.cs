using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOvere : MonoBehaviour {

    public GameObject player;

    public GameObject menu;
    public Button restart1;
    public Button mainMenu;


	// Use this for initialization
	void Start () {
        menu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        //restart.onClick.AddListener(RestartScene);
	}

    public void RestartLvl()
    {
        menu.SetActive(true);
        restart1.onClick.AddListener(RestartScene);
        mainMenu.onClick.AddListener(goToMainMenu);
    }

    public void GoToLvl2()
    {
        SceneManager.LoadScene("Map2");
    }

    public void GoToLvl3()
    {
        SceneManager.LoadScene("Map3");
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("Map1");
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}





