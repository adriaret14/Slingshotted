using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public int slot;
    private InventoryManager inv;
    private Image image;
    //Sprites de todos los items
    

	// Use this for initialization
	void Start () {
        inv = GameObject.Find("Jugador").GetComponent<InventoryManager>();
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        image.sprite = inv.itemSprites[inv.inventory[slot]];
	}
}
