using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomlessPitClass : MonoBehaviour {
    
    public float gravity = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerClass>() != null || collision.GetComponent<EnemyClass>() != null)
        {
            if (collision.GetComponent<PlayerClass>() != null)
            {
                collision.GetComponent<PlayerClass>().fallenFlag = true;
            }
            else
            {
                collision.GetComponent<EnemyClass>().fallenFlag = true;
            }
            collision.GetComponent<Rigidbody2D>().gravityScale = gravity;
           /* if (GameObject.Find("Mapa/Mapa2_Navmesh") != null) GameObject.Find("Mapa/Mapa2_Navmesh").SetActive(false);
            else if (GameObject.Find("Mapa/Mapa3_Navmesh") != null) GameObject.Find("Mapa/Mapa3_Navmesh").SetActive(false);
            else if (GameObject.Find("Mapa/Mapa4_Navmesh") != null) GameObject.Find("Mapa/Mapa4_Navmesh").SetActive(false);*/
        }
    }

}
