using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneClass : MonoBehaviour {
    
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
            if (collision.GetComponent<PlayerClass>() != null)
            {
                collision.GetComponent<PlayerClass>().takeDamage(collision.GetComponent<PlayerClass>().healthPoints);
                collision.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            else
            {
                collision.GetComponent<EnemyClass>().takeDamage(collision.GetComponent<EnemyClass>().healthPoints);
            }
        }
    }
}
