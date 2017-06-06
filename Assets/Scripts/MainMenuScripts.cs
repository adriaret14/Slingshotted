using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour {

    public Button playButton;
    public Button ecred;
    public Button econtr;
    public Button cred;
    public Button contr;
    public Button exit;

    public GameObject PB;
    public GameObject CE;
    public GameObject CO;
    public GameObject ECE;
    public GameObject ECO;
    public GameObject E;

    public Image background;
    public Image controles;
    public Image creditos;

	// Use this for initialization
	void Start () {
        creditos.enabled = false;
        controles.enabled = false;
        ECE.SetActive(false);
        ECO.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        playButton.onClick.AddListener(startGame);
        cred.onClick.AddListener(showCredits);
        contr.onClick.AddListener(showControls);
        ecred.onClick.AddListener(hideCredits);
        econtr.onClick.AddListener(hideControls);
        exit.onClick.AddListener(endApp);

	}

    void startGame()
    {
        SceneManager.LoadScene("Map1");
    }

    void showCredits()
    {
        PB.SetActive(false);
        CE.SetActive(false);
        CO.SetActive(false);
        ECO.SetActive(false);
        ECE.SetActive(true);
        E.SetActive(false);
        background.enabled = false;
        controles.enabled = true;
       

    }

    void showControls()
    {
        PB.SetActive(false);
        CE.SetActive(false);
        CO.SetActive(false);
        ECO.SetActive(true);
        ECE.SetActive(false);
        E.SetActive(false);
        background.enabled = false;
        creditos.enabled = true;
    }

    void hideCredits()
    {
        PB.SetActive(true);
        CE.SetActive(true);
        CO.SetActive(true);
        ECO.SetActive(false);
        ECE.SetActive(false);
        E.SetActive(true);
        background.enabled = true;
        controles.enabled = false;
    }

    void hideControls()
    {
        PB.SetActive(true);
        CE.SetActive(true);
        CO.SetActive(true);
        ECO.SetActive(false);
        ECE.SetActive(false);
        E.SetActive(true);
        background.enabled = true;
        creditos.enabled = false;
    }

    void endApp()
    {
        Application.Quit();
    }
}
