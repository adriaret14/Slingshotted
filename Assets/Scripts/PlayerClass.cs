using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour {

    //Vida del jugador
    public float healthPoints = 100f;
    //Daño del jugador (melee)
    public float damageMelee = 21;
    //Daño del jugador (ranged)
    public float damageRanged = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (healthPoints < 0)
            healthPoints = 0;
    }

    public void takeDamage(float dmg)
    {
        if (healthPoints < dmg)
        {
            healthPoints = 0;
        }
        else
        {
            healthPoints -= dmg;
        } 
    }
}
