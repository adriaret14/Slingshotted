﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause2 : MonoBehaviour
{

    public Button mainMenu;
    public GameObject MM;


    //public 

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        mainMenu.onClick.AddListener(backToMenu);
    }

    void backToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
