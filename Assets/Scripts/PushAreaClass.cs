using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAreaClass : MonoBehaviour {

    public float pushX;
    public float pushY;
    private Vector2 forceVector;
	void Start () {
        forceVector = new Vector2(pushX, pushY);
	}
    private void OnTriggerStay2D(Collider2D collision)
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
            collision.GetComponent<Rigidbody2D>().AddForce(forceVector * 0.8f * Time.deltaTime);
        }        
    }
}
