using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass : MonoBehaviour {

    public Item item;
    private float pickupTimer = 4.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (pickupTimer > 0)
        {
            pickupTimer -= Time.deltaTime;
        }
        if (pickupTimer < 0)
        {
            pickupTimer = 0;
        }
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (pickupTimer == 0)
        {
            if (col.gameObject.GetComponent<PlayerClass>() != null)
            {
                if (!col.gameObject.GetComponent<InventoryManager>().isFull())
                {
                    col.gameObject.GetComponent<InventoryManager>().addToInventory(item);
                    Destroy(gameObject);
                }
            }
        }
    }

}
