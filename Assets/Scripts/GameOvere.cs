using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOvere : MonoBehaviour {

    public GameObject player;

    public GameObject menu;
    public GameObject menu2;
    public Button restart1;
    public Button restart2;


	// Use this for initialization
	void Start () {
        menu.SetActive(false);
        menu2.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        //restart.onClick.AddListener(RestartScene);
	}

    public void RestartLvl()
    {
        menu.SetActive(true);
        restart1.onClick.AddListener(RestartScene);
    }

    public void RestartLvl2()
    {
        menu2.SetActive(true);
        restart2.onClick.AddListener(RestartScene);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("Map1");
    }
}





