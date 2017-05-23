using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ORB { LIFE, DEATH, NONE };
public enum ENEMY_TYPE { SKT, ZMB, NIG, SPR, CON };
public class EnemyClass : MonoBehaviour {

    //Tipo de enemigo
    public ENEMY_TYPE tipo;
    //Vida del enemigo
    public float healthPoints;

    //daño melee del enemigo
    public float damageMelee;
    public float damageRanged;

    //Velocidad en estado ROAMING
    public float roamingSpeed;

    //Velocidad en estado CHASING
    public float speed;

    //Velocidad en estado FLEEING
    public float fleeingSpeed;

    //Cooldown del ataque del enemigo
    public float attackCD;
    public float rangedAttackCD;
    public float conjuringCD;

    //Regeneracion de vida por segundo
    public float healthRegen;
    public float regenWaitCD;
    private float regenTimer;

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

        switch (tipo)
        {
            case ENEMY_TYPE.SKT:

                healthPoints = 115f;
                damageMelee = 12f;
                damageRanged = 0f;

                roamingSpeed = 0.112f;
                speed = 0.05f;
                fleeingSpeed = 0.112f;

                attackCD = 0.5f;
                rangedAttackCD = -1.0f;
                conjuringCD = -1.0f;

                healthRegen = 10f;
                regenWaitCD = 3f;
                regenTimer = 0f;

                break;

            case ENEMY_TYPE.ZMB:

                healthPoints = 100f;
                damageMelee = 0f;
                damageRanged = 60f;

                roamingSpeed = 0.112f;
                speed = 0.05f;
                fleeingSpeed = 0.112f;

                attackCD = -1.0f;
                rangedAttackCD = 0.0f;
                conjuringCD = -1.0f;

                healthRegen = 10f;
                regenWaitCD = 3f;
                regenTimer = 0f;

                break;

            case ENEMY_TYPE.NIG:

                healthPoints = 90f;
                damageMelee = 12f;

                roamingSpeed = 0.102f;
                speed = 0.05f;
                fleeingSpeed = 0.102f;

                attackCD = 0.0f;
                rangedAttackCD = -1.0f;
                conjuringCD = 0.0f;

                healthRegen = 10f;
                regenWaitCD = 3f;
                regenTimer = 0f;

                break;

            case ENEMY_TYPE.SPR:

                healthPoints = 150f;
                damageMelee = 15f;

                roamingSpeed = 0.102f;
                speed = 0.04f;
                fleeingSpeed = 0.102f;

                attackCD = 0.0f;
                rangedAttackCD = -1.0f;
                conjuringCD = -1.0f;

                healthRegen = 10f;
                regenWaitCD = 3f;
                regenTimer = 0f;

                break;

            case ENEMY_TYPE.CON:

                healthPoints = 50f;
                damageMelee = 7f;

                roamingSpeed = 0.112f;
                speed = 0.03f;
                fleeingSpeed = 0.112f;

                attackCD = 0.5f;
                rangedAttackCD = -1.0f;
                conjuringCD = -1.0f;

                healthRegen = 10f;
                regenWaitCD = 3f;
                regenTimer = 0f;

                break;

        }
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
