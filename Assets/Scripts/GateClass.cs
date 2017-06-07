using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateClass : MonoBehaviour {

    private int fase = 0;
    private bool Open = false;
    private float openTimer;
    private Animator anim;
    [HideInInspector]
    public int deadCount = 0;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (deadCount == 5)
        {
            fase = 1;
            deadCount = 0;
            openTimer = 0.6f;
        }
        if(openTimer > 0)
        {
            openTimer -= Time.deltaTime;
        }
        if(openTimer < 0)
        {
            openTimer = 0;
        }
        if(fase == 1 && openTimer == 0)
        {
            fase = 2;
        }
        if (fase == 3)
        {
            Destroy(gameObject);
        }

        if (anim != null) anim.SetInteger("Fase", fase);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject A = GameObject.Find("Jugador/attackArea");
        GameObject P = GameObject.Find("Jugador");
        if (other.gameObject == A && P.GetComponent<InventoryManager>().isInInventory(Item.KEY) && (anim != null && fase == 2) || other.gameObject == A && P.GetComponent<InventoryManager>().isInInventory(Item.KEY) && anim == null)
        {
            P.GetComponent<InventoryManager>().deleteFromInventory(Item.KEY);
            if (anim != null) fase = 3;
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
