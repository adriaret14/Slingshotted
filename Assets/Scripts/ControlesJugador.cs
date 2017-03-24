using UnityEngine;
using System.Collections;

public class ControlesJugador : MonoBehaviour {


    public float Speed;

    private Animator anim;
    private bool PlayerMoving;
    private Vector2 lastMove;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        PlayerMoving = false;
        anim.SetFloat("DirY", 0);
        anim.SetFloat("DirX", 0);
        anim.SetFloat("SeeY", 0);
        anim.SetFloat("SeeX", 0);

        //Movimiento ejes

        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1)* Speed * Time.deltaTime;
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

        //Vista estatica
        if (Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetFloat("SeeY", 10);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetFloat("SeeY", -10);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetFloat("SeeX", -10);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetFloat("SeeX", 10);
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
    }
}
