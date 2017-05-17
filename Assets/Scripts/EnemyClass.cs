using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ORB { LIFE, DEATH, NONE };

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
    //Tipo de orbe que dropea y probabilidades
    public GameObject Orb;
    private GameObject OrbClone;
    private ORB orbType;
    private float deathOrbChance;
    private float lifeOrbChance;
    //Probabilidad de orbe perjudicial al maximo de salud (100)
    private float deathOrbMaxChance = 70.0f;
    //probabilidad de orbe perjudicial al minimo de salud (0)
    private float deathOrbMinChance = 5.0f;
    //Probabilidad de orbe beneficioso al maximo de salud (100)
    private float lifeOrbMaxChance = 0.0f;
    //probabilidad de orbe beneficioso al minimo de salud (0)
    private float lifeOrbMinChance = 95.0f;
    //jugador
    private PlayerClass jugador;

    //Drop
    public GameObject Drop;
    private GameObject DropClone;
    public bool drops;

    //Vector unitario vertical
    private Vector2 vertical;
    // Use this for initialization
    void Start () {
		vertical = new Vector2(0, 1);
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 EnemyToPlayer = new Vector2(GameObject.Find("Jugador").transform.position.x - transform.position.x, GameObject.Find("Jugador").transform.position.y - transform.position.y);

        if (Vector2.Angle(vertical, EnemyToPlayer) > 90.0f)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else if (Vector2.Angle(vertical, EnemyToPlayer) < 90.0f)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 2;
        }

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
            jugador = GameObject.Find("Jugador").GetComponent<PlayerClass>();
            deathOrbChance = deathOrbMinChance + (deathOrbMaxChance - deathOrbMinChance) * (jugador.healthPoints / jugador.maxHealthPoints);
            lifeOrbChance = lifeOrbMinChance + (lifeOrbMaxChance - lifeOrbMinChance) * (jugador.healthPoints / jugador.maxHealthPoints);

            float randomNumber = Random.Range(0.0f, 100.0f);

            if (randomNumber > deathOrbChance + lifeOrbChance)
                orbType = ORB.NONE;
            else if (randomNumber > deathOrbChance)
                orbType = ORB.LIFE;
            else
                orbType = ORB.DEATH;

            if (orbType != ORB.NONE)
            {
                OrbClone = Instantiate<GameObject>(Orb, transform.position, transform.rotation);
                /*
                float angle1 = Random.Range(0.0f, 2 * Mathf.PI);
                Vector2 dir = new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * 25 * Time.deltaTime;
                */
                OrbClone.GetComponent<OrbClass>().orbType = orbType;
            }

            if (drops)
            {
                DropClone = Instantiate<GameObject>(Drop, transform.position, transform.rotation);
                /*
                float angle2 = Random.Range(0.0f, 2 * Mathf.PI);
                dir = new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2)) * 25 * Time.deltaTime;
                */
                
            }
            GameObject.Find("FinalGate").GetComponent<GateClass>().deadCount++;
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
