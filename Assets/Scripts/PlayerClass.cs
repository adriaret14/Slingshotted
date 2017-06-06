using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerClass : MonoBehaviour {

    public GameObject player;
    //Vida del jugador
    public float healthPoints = 100f;
    public float maxHealthPoints = 100;
    //Daño del jugador (melee)
    public float damageMelee = 21;
    //Daño del jugador (ranged)
    public float damageRanged = 100;
    //Tier de la armadura
    public int armorTier = 0;
        
    public bool fallenFlag = false;    
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        

        if (healthPoints <= 0)
        {
            healthPoints = 0;
            restartLevel();
            
        }
        
            
        
    }

    public void takeDamage(float dmg)
    {
        player.GetComponent<Bars>().updateHealth(dmg, 1);
        if (dmg < 0 && healthPoints - maxHealthPoints < dmg)
            healthPoints = 100;
        if (healthPoints < dmg)
        {
            healthPoints = 0;
            restartLevel();
        }
        else
        {
            healthPoints -= dmg;
        } 
    }
    
    public void restartLevel()
    {
        Scene escena = SceneManager.GetActiveScene();
        string cad = escena.name;
        string[] subcad = cad.Split('p');

        if (subcad[1] == "1")
        {
            player.GetComponent<GameOvere>().RestartLvl();
        }
        else if (subcad[1] == "2")
        {
            player.GetComponent<GameOvere2>().RestartLvl();
        }
        else if (subcad[1] == "3")
        {
            player.GetComponent<GameOvere3>().RestartLvl();
        }
        else if (subcad[1] == "4")
        {
            //player.GetComponent<GameOvere4>().RestartLvl();
        }
    } 
}
