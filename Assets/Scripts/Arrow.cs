using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    private GameObject jugador;
    public float Speed = 5;
    private Vector2 v=new Vector2(0, 0);
	// Use this for initialization
	void Start () {
        jugador = GameObject.Find("Jugador");

        int LD = jugador.GetComponent<ControlesJugador>().shootDir;
       
        switch (LD)
        {
            case 1:
                this.gameObject.transform.Rotate(0, 0, 90);
                v = new Vector2(0, 1);
                print("Flecha hacia arriba    LD : "+ LD);
                break;
            case 2:
                //this.gameObject.transform.Rotate(0, 0, -90);
                v = new Vector2(1, 0);
                print("Flecha hacia derecha    LD : " + LD);
                break;
            case 3:
                this.gameObject.transform.Rotate(0, 0, -90);
                v = new Vector2(0, -1);
                print("Flecha hacia abajo    LD : " + LD);
                break;
            case 4:
                this.gameObject.transform.Rotate(0, 0, 180);
                v = new Vector2(-1, 0);
                print("Flecha hacia izquierda    LD : " + LD);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1) * Speed * Time.deltaTime;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = v * Speed * Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag=="paredes")
        {
            Destroy(gameObject);
        }
        else if(col.gameObject.GetComponent<EnemyClass>()!=null)
        {
            col.gameObject.GetComponent<EnemyClass>().takeDamage(jugador.GetComponent<PlayerClass>().damageRanged);
            Destroy(gameObject);
        }
    }
}
