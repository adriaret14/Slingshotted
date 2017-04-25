using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour {

    //Vida del enemigo
    public float healthPoints = 100f;
    //daño melee del enemigo
    public float damageMelee = 34f;
    //Velocidad en estado ROAMING
    public float roamingSpeed = 11.2f;
    //Velocidad en estado CHASING
    public float speed = 1.0f/200000.0f;
    //Velocidad en estado FLEEING
    public float fleeingSpeed = 350;
    //Cooldown del ataque del enemigo
    public float attackCD;
    //Regeneracion de vida por segundo
    public float healthRegen = 10;
    public float regenWaitCD = 3;
    private float regenTimer = 0;

    //Drop
    public GameObject Drop;
    public bool drops;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (healthPoints < 100f)
        {
            if (regenTimer <= 0)
            {
                healthPoints += healthRegen * Time.deltaTime;
            }
            
        }
        if (healthPoints > 100f)
        {
            healthPoints = 100f;
        }
        if (healthPoints == 0)
        {
            if (drops)
            {
                Instantiate<GameObject>(Drop, transform.position, transform.rotation);
                
                float angle = Random.Range(0.0f, 2 * Mathf.PI);
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * roamingSpeed * Time.deltaTime;
                Drop.GetComponent<Rigidbody2D>().AddForce(dir);
                
            }
            Destroy(gameObject);
        }
        if (regenTimer > 0)
        {
            regenTimer -= Time.deltaTime;
        }
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
        regenTimer = regenWaitCD;
    }
}
