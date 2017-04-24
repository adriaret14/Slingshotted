using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour {

    //Vida del enemigo
    public float healthPoints = 100f;
    //daño melee del enemigo
    public float damageMelee = 34f;
    //Velocidad en estado ROAMING
    public float roamingSpeed = 30;
    //Velocidad en estado CHASING
    public float speed = 100;
    //Velocidad en estado FLEEING
    public float fleeingSpeed = 350;
    //Cooldown del ataque del enemigo
    public float attackCD;
    //Regeneracion de vida por segundo
    public float healthRegen = 10;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (healthPoints < 100f)
        {
            healthPoints += healthRegen * Time.deltaTime;
        }
        if (healthPoints > 100f)
        {
            healthPoints = 100f;
        }
        if (healthPoints < 0)
        {
            healthPoints = 0;
            Destroy(gameObject);
        }
    }
}
