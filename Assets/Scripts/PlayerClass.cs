using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [HideInInspector]
    public bool fallenFlag = false;    
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (healthPoints < 0)
        {
            //print("Estoy muerto");
            healthPoints = 0;
            //player.GetComponent<GameOvere>().RestartLvl();
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
            player.GetComponent<GameOvere>().RestartLvl();
        }
        else
        {
            healthPoints -= dmg;
        } 
    } 
}
