using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOvere : MonoBehaviour {

    public GameObject player;

    public GameObject menu;
    public Button restart;


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
        print("Muerteeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
        menu.SetActive(true);
        int flag = 0;
        restart.onClick.AddListener(RestartScene); 
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("Proyecto Mov");
    }
}





