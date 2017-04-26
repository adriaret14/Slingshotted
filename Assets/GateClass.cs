using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateClass : MonoBehaviour {

    public bool Shut = true;
    public bool Locked = true;
    private bool Opening = false;
    private float openTimer;
    private Animator anim;
    [HideInInspector]
    public int deadCount = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        AllDead();
        if (Opening)
        {
            openTimer = 0.6f;
        }
        if(openTimer > 0)
        {
            openTimer -= Time.deltaTime;
        }
        if(Opening && openTimer == 0)
        {
            Opening = false;
            Shut = false;
        }
        if (!Locked)
        {
            Destroy(gameObject);
        }

	}

    private void AllDead()
    {
        if (deadCount == 4)
        {
            Opening = true;
            deadCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject A = GameObject.Find("Jugador/attackArea");
        GameObject P = GameObject.Find("Jugador");
        if (other.gameObject == A && P.GetComponent<InventoryManager>().isInInventory(Item.KEY) && !Shut)
        {
            P.GetComponent<InventoryManager>().deleteFromInventory(Item.KEY);
            Locked = false;
        }
    }
}
