using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

    [HideInInspector]
    public Transform trans;
    [HideInInspector]
    public BoxCollider2D attackArea;
    [HideInInspector]
    public Vector2 sizeFull;
    [HideInInspector]
    public Vector2 sizeReduced = new Vector2(0.01f, 0.01f);
    [HideInInspector]
    public Vector2 sizeFull2Y = new Vector2(0.188839f, 0.188839f * 2);
    [HideInInspector]
    public Vector2 sizeFull2X = new Vector2(0.188839f * 2, 0.188839f);
    [HideInInspector]
    public Vector2 origin;
    public ForceMode2D fmode;

    private GameObject parent;

	// Use this for initialization
	void Start () {
        parent = GetComponent<Transform>().parent.gameObject;
        attackArea = GetComponent<BoxCollider2D>();
        sizeFull = attackArea.size;
        origin = attackArea.offset;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject != parent)
        {            
            if (collider.gameObject.GetComponent<EnemyClass>() != null)
            {
                collider.gameObject.GetComponent<EnemyClass>().takeDamage(parent.GetComponent<PlayerClass>().damageMelee);
                Vector2 dir = (collider.gameObject.GetComponent<Transform>().position - parent.GetComponent<Transform>().position).normalized;
                collider.gameObject.GetComponent<Rigidbody2D>().AddForce(dir/100, fmode);
            }
            else if (collider.gameObject.GetComponent<PlayerClass>() != null)
            {
                collider.gameObject.GetComponent<PlayerClass>().takeDamage(parent.GetComponent<EnemyClass>().damageMelee);
                Vector2 dir = (collider.gameObject.GetComponent<Transform>().position - parent.GetComponent<Transform>().position).normalized;
                collider.gameObject.GetComponent<Rigidbody2D>().AddForce(dir/100, fmode);
            }
        }
    }
}
