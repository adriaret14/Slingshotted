using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushToBackgroundClass : MonoBehaviour {
        
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool fallen;
        if (collision.GetComponent<PlayerClass>() != null)
        {
            fallen = collision.GetComponent<PlayerClass>().fallenFlag;
        }
        else if (collision.GetComponent<EnemyClass>() != null)
        {
            fallen = collision.GetComponent<EnemyClass>().fallenFlag;
        }
        else
        {
            fallen = false;
        }
        if (fallen)
        {
            collision.gameObject.layer = 13;
            collision.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }
}
