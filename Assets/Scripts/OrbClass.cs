using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OrbClass : MonoBehaviour {

    private PlayerClass jugador;

    [HideInInspector]
    public ORB orbType;

    private float healthValue;
    private float minHealthValue;
    private float maxHealthValue;

    private float pickupTimer = 1.5f;

    private bool pickedUp = false;
    private float pickupAnimTimer = 1.04f;

    private bool started = false;

	// Use this for initialization
	void Start () {
        jugador = GameObject.Find("Jugador").GetComponent<PlayerClass>();
        minHealthValue = jugador.maxHealthPoints * 0.10f;
        maxHealthValue = jugador.maxHealthPoints * 0.25f;
	}

    void Update()
    {
        if (!started && (orbType == ORB.DEATH || orbType == ORB.LIFE))
        {            
            healthValue = Random.Range(minHealthValue, maxHealthValue);
            healthValue = orbType == ORB.DEATH ? healthValue : -healthValue;
            Debug.LogWarning("Orb health damage: " + healthValue);
            if (orbType == ORB.DEATH)
            {
                GetComponent<Animator>().SetBool("isDeath", true);
            }
            started = true;
        }
        if (pickupTimer > 0)
        {
            pickupTimer -= Time.deltaTime;
        }
        if (pickupTimer < 0)
        {
            pickupTimer = 0;
        }
        if (pickedUp && pickupAnimTimer > 0)
        {
            pickupAnimTimer -= Time.deltaTime;
        }
        if (pickupAnimTimer <= 0)
        {            
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickupTimer == 0 && !pickedUp)
        {
            if (collision.gameObject == jugador.gameObject)
            {
                pickedUp = true;
                jugador.takeDamage(healthValue);
                GetComponent<Animator>().SetBool("pickedUp", true);
            }
        }
    }
}
