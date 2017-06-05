using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesClass : MonoBehaviour {

    public GameObject dmgCollider;
    public TriggerZoneClass triggerZone;
    private GameObject player;

    public int startingPosition = 1;
    public float damage;

    public float highCD;
    public float lowCD;
    private float highTimer;
    private float lowTimer;

    private int State = 0;
    private int prevState = 0;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Jugador");
        dmgCollider.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (lowTimer > 0)
            lowTimer -= Time.deltaTime;
        if (highTimer > 0)
            highTimer -= Time.deltaTime;

        if (lowTimer <= 0 && State == 1)
        {
            State = 2;
            highTimer = highCD;
            dmgCollider.SetActive(true);
        }

        if (highTimer <= 0 && State == 2)
        {
            State = 1;
            lowTimer = lowCD;
            dmgCollider.SetActive(false);
        }

        if (triggerZone.inArea && State == 0)
        {
            State = startingPosition;
            lowTimer = lowCD;
            dmgCollider.SetActive(true);
        }

        else if (!triggerZone.inArea)
        {
            State = 0;
            dmgCollider.SetActive(false);
        }

		if (State != prevState)
        {
            GetComponent<Animator>().SetInteger("State", State);
            prevState = State;
        }
	}
}
