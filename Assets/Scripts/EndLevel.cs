using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour {

    Scene escena;
    string nombre;
    string[] cad;


    // Use this for initialization
    void Start () {
        escena = SceneManager.GetActiveScene();
        nombre = escena.name;
        cad = nombre.Split('p');
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameObject.Find("Jugador"))
        {
            if(cad[1]=="1")
            {
                //Debug.LogError("ANDANDO AL LVL 222222");
                GameObject.Find("Jugador").GetComponent<GameOvere>().GoToLvl2();
                
            }
            else if (cad[1] == "2")
            {
               // Debug.LogError("ANDANDO AL LVL 333333");
                GameObject.Find("Jugador").GetComponent<GameOvere2>().GoToLvl2();
                
            }
            else if(cad[1] == "3")
            {
                //Debug.LogError("ANDANDO AL LVL 44444");
                GameObject.Find("Jugador").GetComponent<GameOvere3>().goToMainMenu();
                
            }
        }
    }
}
