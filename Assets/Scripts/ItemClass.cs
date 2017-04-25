using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass : MonoBehaviour {

    Item item;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D col)
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
