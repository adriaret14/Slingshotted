using UnityEngine;
using System.Collections;
using System;
using System.Timers;
using UnityEngine.UI;

public class ControlesJugador : MonoBehaviour {

    public GameObject player;
    public float Speed;
    public GameObject flecha;

    private GameObject flechaClon;

    private Animator anim;
    private bool PlayerMoving;
    private Vector2 lastMove;
    private bool canMove = true;

    private double timer = 0;
    private double meleeCD = 0.4f;
    private int LLD;
    //Collider de ataque del jugador
    public GameObject attackCollider;
    private BoxCollider2D attackArea;
    //Direccion en la que ataca el jugador
    private int attackDirection;
    //Distancia a la que el jugador ataca
    private float meleeAttackRange = 0.19f;
    //Enemigos
    public GameObject enemy1;            

    private bool contPause=false;
    private bool contInv = false;
    [SerializeField]
    private GameObject menu, inventario;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        anim.SetInteger("LD", 1);
        LLD = 1;
        menu.SetActive(contPause);
        inventario.SetActive(contInv);
        attackArea = attackCollider.GetComponent<BoxCollider2D>();
        attackArea.size = attackCollider.GetComponent<AttackCollider>().sizeReduced;
    }

    // Update is called once per frame
    void Update() {

        //print(Input.GetAxis("PS4-VER-PAD")+"\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\"+ Input.GetAxis("PS4-HOR-PAD"));

        PlayerMoving = false;
        anim.SetFloat("DirY", 0);
        anim.SetFloat("DirX", 0);

        //Movimiento ejes
        if (canMove) {
        if (Input.GetKey(KeyCode.W) || Input.GetAxis("PS4-VER-PAD") > 0) 
        {
            //print("ARRIBAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1) * Speed * Time.deltaTime;
            anim.SetFloat("DirY", 10);
            anim.SetInteger("LD", 1);
            LLD = anim.GetInteger("LD");
            PlayerMoving = true;
            lastMove = new Vector2(0, 1);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetAxis("PS4-VER-PAD") < 0)
        {
            //print("ABAJOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1) * Speed * Time.deltaTime;
            anim.SetFloat("DirY", -10);
            anim.SetInteger("LD", 3);
            LLD = anim.GetInteger("LD");
            PlayerMoving = true;
            lastMove = new Vector2(0, -1);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetAxis("PS4-HOR-PAD") > 0)
        {
            //print("DERECHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * Speed * Time.deltaTime;
            anim.SetFloat("DirX", 10);
            anim.SetInteger("LD", 2);
            LLD = anim.GetInteger("LD");
            PlayerMoving = true;
            lastMove = new Vector2(1, 0);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetAxis("PS4-HOR-PAD") < 0)
        {
            //print("IZQUIERDAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * Speed * Time.deltaTime;
            anim.SetFloat("DirX", -10);
            anim.SetInteger("LD", 4);
            LLD = anim.GetInteger("LD");
            PlayerMoving = true;
            lastMove = new Vector2(-1, 0);
        }

        //Movimiento en diagonales
        if ((Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.A)))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1).normalized * Speed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.D)))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * Speed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.A)))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -1).normalized * Speed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.D)))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1, -1).normalized * Speed * Time.deltaTime;
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


        //Mirar
        if (Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetFloat("LastY", 1);
            anim.SetFloat("LastX", 0);
            anim.SetInteger("LD", 1);
            //print("Miro arriba");
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetFloat("LastY", -1);
            anim.SetFloat("LastX", 0);
            anim.SetInteger("LD", 3);
            //print("Miro abajo");
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetFloat("LastY", 0);
            anim.SetFloat("LastX", -1);
            anim.SetInteger("LD", 4);
            //print("Miro izquierda");
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetFloat("LastY", 0);
            anim.SetFloat("LastX", 1);
            anim.SetInteger("LD", 2);
            //print("Miro derecha");
        }
        if(!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetInteger("LD", LLD);
            //print(LLD);
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButton("PS4-SQR"))
        {
            //print(player.GetComponent<Bars>().stamina);
            //print(player.GetComponent<Bars>().maxStamina);

            if (player.GetComponent<Bars>().getStamina())
            {
                player.GetComponent<Bars>().useStamina();
                switch (anim.GetInteger("LD"))
                {
                    case 1:
                        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1) * 650 * Time.deltaTime;
                        print("Dash arriba");
                        break;
                    case 2:
                        GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * 650 * Time.deltaTime;
                        print("Dash derecha");
                        break;
                    case 3:
                        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1) * 650 * Time.deltaTime;
                        print("Dash abajo");
                        break;
                    case 4:
                        GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * 650 * Time.deltaTime;
                        print("Dash izquierda");
                        break;
                }

            }

        }


        //Menu
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("PS4-OPTIONS"))
        {
            //print("Menu de pausa");
            contPause = !contPause;
            menu.SetActive(contPause);

            if (contPause == true)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }            
        }


        //Inventario
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("PS4-TACTILE"))
        {
            contInv = !contInv;
            inventario.SetActive(contInv);
            InventorySlot slot = GameObject.Find("Inventario/Contenido/Slot-1/Contenido").GetComponent<InventorySlot>();
            slot.updateSlot();
        }


        //Ataque
        if ((Input.GetKey(KeyCode.Space) && canMove) || Input.GetButtonDown("PS4-X"))
        {
            anim.SetBool("Slashing", true);
            PlayerMoving = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * Speed * Time.deltaTime;
            canMove = false;
            timer = meleeCD;
            attackDirection = anim.GetInteger("LD");
            switch (attackDirection)
            {                
                case 4:
                    {
                        attackArea.offset += new Vector2(-meleeAttackRange, 0);
                        attackArea.size = attackCollider.GetComponent<AttackCollider>().sizeFull2Y;
                        break;
                    }
                case 2:
                    {
                        attackArea.offset += new Vector2(meleeAttackRange, 0);
                        attackArea.size = attackCollider.GetComponent<AttackCollider>().sizeFull2Y;
                        break;
                    }
                case 1:
                    {
                        attackArea.offset += new Vector2(0, meleeAttackRange);
                        attackArea.size = attackCollider.GetComponent<AttackCollider>().sizeFull2X;
                        break;
                    }
                case 3:
                    {
                        attackArea.offset += new Vector2(0, -meleeAttackRange);
                        attackArea.size = attackCollider.GetComponent<AttackCollider>().sizeFull2X;
                        break;
                    }                    
            }
                   
        }

        //Disparar arco
        if (Input.GetKeyDown(KeyCode.Q))
        {
            flechaClon = Instantiate<GameObject>(flecha, transform.position, transform.rotation);
        }


        if (timer > 0)
            timer -= Time.deltaTime;

        if (timer <= 0)
        {
            canMove = true;
            anim.SetBool("Slashing", false);
            attackArea.size = attackCollider.GetComponent<AttackCollider>().sizeReduced;
            attackArea.offset = attackCollider.GetComponent<AttackCollider>().origin;
        }

    }

    public int getLLD()
    {
        return LLD;
    }
}
