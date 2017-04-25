using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    NONE,
    KEY,
    ARROW,
}

public class InventoryManager : MonoBehaviour {

    //Inventario
    //Array de items (Tamaño maximo 16)
    public const int inventorySize = 16;
    
    public Item[] inventory;

    [HideInInspector]
    public Dictionary<Item, Sprite> itemSprites;

    public Sprite none;
    public Sprite key;

    // Use this for initialization
    void Start () {
        inventory = new Item[inventorySize]
        {
            Item.NONE, Item.NONE, Item.NONE, Item.NONE,
            Item.NONE, Item.NONE, Item.NONE, Item.NONE,
            Item.NONE, Item.NONE, Item.NONE, Item.NONE,
            Item.NONE, Item.NONE, Item.NONE, Item.NONE
        };
        //Mapa item -> sprite para el inventario
        itemSprites = new Dictionary<Item, Sprite>();
        itemSprites.Add(Item.NONE, none);
        itemSprites.Add(Item.KEY, key);       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Retorna true si el item pasado como parametro existe al menos 1 vez en el inventario
    public bool isInInventory(Item item)
    {
        bool res = false;
        int i = 0;
        while (i < inventorySize && !res)
        {
            res = inventory[i] == item;
            i++;
        }
        return res;
    }

    //Retorna true si el item pasado por parametro existe al menos n veces en el inventario
    public bool isInInventoryMul(Item item, int n)
    {
        bool res = false;
        int count = n;
        int i = 0;
        while (i < inventorySize && !res)
        {
            if (inventory[i] == item)
            {
                count--;
                if (count == 0)
                    res = true;                
            }
            i++;
        }
        return res;
    }

    //Elimina el item pasado como parametro del inventario
    public void deleteFromInventory(Item item)
    {
        bool found = false;
        int i = 0;
        while (i < inventorySize && !found)
        {
            if (inventory[i] == item)
            {
                inventory[i] = Item.NONE;
                found = true;
            }
            i++;
        }
    }

    //Añade un item al inventario si no esta lleno
    public void addToInventory(Item item)
    {
        if (!isFull())
        {
            bool added = false;
            int i = 0;
            while (i < inventorySize && !added)
            {
                if (inventory[i] == Item.NONE)
                {
                    inventory[i] = item;
                    added = true;
                }
                i++;
            }
            Debug.LogWarning("Objeto añadido al inventario." + inventory[i-1]);
        }
        else
        {
            Debug.LogError("Inventario lleno. No se ha podido añadir el objeto.");
        }
    }

    //Retorna true si el inventario esta lleno
    public bool isFull()
    {
        bool res = true;
        int i = inventorySize - 1;
        while (i >= 0 && res)
        {
            if (inventory[i] == Item.NONE)
                res = false;            
        }
        return res;
    }


}
