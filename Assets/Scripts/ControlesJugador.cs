using UnityEngine;
using System.Collections;
using System;
using System.Timers;

public class ControlesJugador : MonoBehaviour {


    public float Speed;

    private Animator anim;
    private bool PlayerMoving;
    private Vector2 lastMove;
    private bool canMove = true;

    private double timer = 0;
    private double meleeCD = 0.4f;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        anim.SetInteger("LD", 1);
    }

    // Update is called once per frame
    void Update() {

        PlayerMoving = false;
        anim.SetFloat("DirY", 0);
        anim.SetFloat("DirX", 0);

        //Movimiento ejes
        if (canMove) { 
        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1) * Speed * Time.deltaTime;
            anim.SetFloat("DirY", 10);
            anim.SetInteger("LD", 1);
            PlayerMoving = true;
            lastMove = new Vector2(0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1) * Speed * Time.deltaTime;
            anim.SetFloat("DirY", -10);
            anim.SetInteger("LD", 3);
            PlayerMoving = true;
            lastMove = new Vector2(0, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * Speed * Time.deltaTime;
            anim.SetFloat("DirX", 10);
            anim.SetInteger("LD", 2);
            PlayerMoving = true;
            lastMove = new Vector2(1, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * Speed * Time.deltaTime;
            anim.SetFloat("DirX", -10);
            anim.SetInteger("LD", 4);
            PlayerMoving = true;
            lastMove = new Vector2(-1, 0);
        }

        //Movimiento en diagonales
        if ((Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.A)))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1) * Speed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.D)))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1) * Speed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.A)))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -1) * Speed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.D)))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1, -1) * Speed * Time.deltaTime;
        }
    }


        anim.SetBool("SeMueve", PlayerMoving);
        anim.SetFloat("LastX", lastMove.x);
        anim.SetFloat("LastY", lastMove.y);   

        if ((!Input.GetKey(KeyCode.W)) && (!Input.GetKey(KeyCode.S)) && (!Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.D)))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            anim.SetFloat("DirY", 0);
            anim.SetFloat("DirX", 0);
            //anim.SetInteger("LD", 0);
        }


        //Ataque
        if (Input.GetKey(KeyCode.Space) && canMove)
        {
            anim.SetBool("Slashing", true);
            PlayerMoving = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * Speed * Time.deltaTime;
            canMove = false;
            timer = meleeCD;
         }

        if (timer > 0)
            timer -= Time.deltaTime;

        if (timer <= 0)
        {
            canMove = true;
            anim.SetBool("Slashing", false);
        }
    }
}
