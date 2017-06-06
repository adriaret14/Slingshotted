using UnityEngine;
using System.Collections;
using Pathfinding;

enum EnemyState
{
    IDLE,
    ROAMING,
    CHASING,
    ATTACKING,
    FLEEING,
    DEAD
}
enum Direction
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]

public class EnemyAI : MonoBehaviour
{
    //el npc
    private EnemyClass enemy;

    //the room
    public BoxCollider2D room;
    [HideInInspector]
    public bool inRoom = false;

    //el objetivo/jugador
    private GameObject p;
    private Transform target;
    private CircleCollider2D targetCollider;
    private PlayerClass player;
    private Rigidbody2D playerRig;

    //update rate of path
    public float updateRate = 6f;

    //references
    private Seeker seeker;
    private Rigidbody2D rigid2D;

    //calculated path
    public Path path;

    //forcemode
    public ForceMode2D fMode;

    //Wether the ai has reached the end of the path
    [HideInInspector]
    public bool pathIsEnded = false;

    //the waypoint are we going to
    private int _currentWaypoint = 0;

    //how close to a waypoint to continue on
    public float nextWaypointDist = 3;
    public float distanceToTarget;

    //The state of the ai
    EnemyState state = EnemyState.IDLE;

    //The amount of time the ai stays in IDLE state
    private float idleTimer = 0;
    private float idleDuration = 2f;

    //Roaming timer and speed
    private float roamingTimer = 0;
    private float roamingDuration = 2f;

    //The distance within the ai stops moving towards the player.
    private float stopDistance = 0.19f;

    //The distance within the ai keeps chasing if player leaves the room
    public float engageDistance = 1.95f;

    //Velocidades en los distintos estados dinamicos
    private float speed;
    private float roamingSpeed;
    private float fleeingSpeed;

    //Direccion en la que el enemigo ataca
    [HideInInspector]
    Direction attackDirection;
    [HideInInspector]
    private int movementDirection;
    //Cooldown del ataque del enemigo
    private float attackCD;
    private float shootCD;
    private float attackTimer = 0;
    //Collider de ataque
    public BoxCollider2D attackArea;
    public AttackCollider attackColliderScript;
    //Center (offset) of the circlecollider2d
    private Vector2 attackOrigin;
    //Full size of the CircleCollider2D
    private Vector2 attackSize;
    private Vector2 attackReduced = new Vector2(0.01f, 0.01f);
    private Vector2 dir;
    private Vector2 movementNormalizedVector;
    private float speedMultiplier;

    public GameObject flecha;
    [HideInInspector]
    public GameObject flechaClon;

    public GameObject Con;
    [HideInInspector]
    public GameObject ConClon;

    private bool shot = false;
    Direction shotDirection;

    private EnemyState lastState = EnemyState.IDLE;
    private bool pathStarted = false;

    //Variables de animacion
    private Animator anim;

    private float animDuration = 0.8f;
    private float animTimer;
    private bool casting = false;

    void Start()
    {
        p = GameObject.Find("Jugador");
        target = p.GetComponent<Transform>();
        targetCollider = p.GetComponent<CircleCollider2D>();
        player = p.GetComponent<PlayerClass>();
        playerRig = p.GetComponent<Rigidbody2D>();

        enemy = GetComponent<EnemyClass>();

        attackColliderScript = attackArea.GetComponent<AttackCollider>();
        
        attackOrigin = attackArea.offset;
        attackSize = attackArea.size;
        attackArea.size = attackReduced;
        attackCD = enemy.attackCD;
        shootCD = enemy.rangedAttackCD;
        speed = enemy.speed;
        roamingSpeed = enemy.roamingSpeed;
        fleeingSpeed = enemy.fleeingSpeed;

        if (enemy.tipo == ENEMY_TYPE.ZMB)
        {
            attackArea.GetComponent<AttackCollider>().cmode = COLLIDERMODE.DETECT;
        }
        //Variables de animacion
        anim = GetComponent<Animator>();
        if (GetComponent<Animator>() == null)
        {
            Debug.LogWarning("No hay animator???");
        }

        seeker = GetComponent<Seeker>();
        rigid2D = GetComponent<Rigidbody2D>();
        if (target == null)
        {
            //Debug.Log("No se ha encontrado objetivo.");
            return;
        }
        distanceToTarget = Vector2.Distance(target.position, transform.position);
        //return new path result to onpath complete        
    }
    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            //Debug.Log("No hay objetivo. UpdatePath");
            yield return false;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1 / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        //Debug.Log("Camino calculado. Error? " + p.error);
        if (!p.error)
        {
            path = p;
            _currentWaypoint = 0;
        }
    }
    void Update()
    {
        //Checks if the player is in the room
        
        //Gets the distance from the player to the ai 
        distanceToTarget = Vector2.Distance(target.position, transform.position);

        //idle timer control
        if (idleTimer > 0)
            idleTimer -= Time.deltaTime;

        //attack timer control
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        else
        {
            if (enemy.tipo == ENEMY_TYPE.SKT || enemy.tipo == ENEMY_TYPE.CON)
            {
                anim.SetBool("Slashing", false);
            }
            if (enemy.tipo == ENEMY_TYPE.SPR)
            {
                anim.SetBool("Thrusting", false);
            }
        }

        //animation timer control
        if (animTimer > 0)
            animTimer -= Time.deltaTime;
        else
        {
            if (enemy.tipo == ENEMY_TYPE.NIG)
            {
                anim.SetBool("Slashing", false);
            }
        }


        //Conditionals that set the ai state
        switch (enemy.tipo)
        {
            case ENEMY_TYPE.SKT:
                inRoom = room.IsTouching(targetCollider);
                if (enemy.healthPoints <= 15)
                {
                    state = EnemyState.FLEEING;
                }
                else if (((distanceToTarget <= engageDistance && distanceToTarget > stopDistance) || (inRoom && distanceToTarget > stopDistance)) && attackTimer <= 0)
                {
                    state = EnemyState.CHASING;
                }
                else if (distanceToTarget <= stopDistance)
                {
                    state = EnemyState.ATTACKING;
                }
                else if (idleTimer <= 0)
                {
                    state = EnemyState.ROAMING;
                }
                else
                {
                    state = EnemyState.IDLE;
                }
                break;
            case ENEMY_TYPE.CON:
                if (enemy.healthPoints <= 15)
                {
                    state = EnemyState.FLEEING;
                }
                else if ((distanceToTarget <= engageDistance && distanceToTarget > stopDistance) && attackTimer <= 0)
                {
                    state = EnemyState.CHASING;
                }
                else if (distanceToTarget <= stopDistance)
                {
                    state = EnemyState.ATTACKING;
                }
                else if (idleTimer <= 0)
                {
                    state = EnemyState.ROAMING;
                }
                else
                {
                    state = EnemyState.IDLE;
                }
                break;
            case ENEMY_TYPE.NIG:
                inRoom = room.IsTouching(targetCollider);
                if (enemy.healthPoints <= 15 || distanceToTarget <= 7*stopDistance)
                {
                    state = EnemyState.FLEEING;
                }
                else if ((inRoom && distanceToTarget > 15 * stopDistance) && attackTimer <= 0)
                {
                    state = EnemyState.CHASING;
                }
                else if (distanceToTarget <= 15 * stopDistance)
                {
                    state = EnemyState.ATTACKING;
                }
                else if (idleTimer <= 0)
                {
                    state = EnemyState.ROAMING;
                }
                else
                {
                    state = EnemyState.IDLE;
                }
                break;
            case ENEMY_TYPE.SPR:
                inRoom = room.IsTouching(targetCollider);
                if (enemy.healthPoints <= 15)
                {
                    state = EnemyState.FLEEING;
                }
                else if (((distanceToTarget <= engageDistance && distanceToTarget > 3*stopDistance) || (inRoom && distanceToTarget > 3*stopDistance)) && attackTimer <= 0)
                {
                    state = EnemyState.CHASING;
                }
                else if (distanceToTarget <= 3*stopDistance)
                {
                    state = EnemyState.ATTACKING;
                }
                else if (idleTimer <= 0)
                {
                    state = EnemyState.ROAMING;
                }
                else
                {
                    state = EnemyState.IDLE;
                }
                break;
        }
        if (enemy.fallenFlag)
        {
            state = EnemyState.IDLE;
        }

        switch (state)
        {
            case EnemyState.IDLE:
                {
                    if (pathStarted)
                    {
                        StopAllCoroutines();
                        pathStarted = false;
                    }
                    lastState = EnemyState.IDLE;
                    Idle();
                    break;
                }
            case EnemyState.CHASING:
                {
                    if (lastState != EnemyState.CHASING && lastState != EnemyState.FLEEING)
                    {
                        seeker.StartPath(transform.position, target.position, OnPathComplete);
                        StartCoroutine(UpdatePath());
                    }
                    lastState = EnemyState.CHASING;
                    pathStarted = true;
                    Chasing();
                    break;
                }
            case EnemyState.ROAMING:
                {
                    if (pathStarted)
                    {
                        StopAllCoroutines();
                        pathStarted = false;
                    }
                    lastState = EnemyState.ROAMING;
                    Roaming();
                    break;
                }
            case EnemyState.ATTACKING:
                {
                    if (pathStarted)
                    {
                        StopAllCoroutines();
                        pathStarted = false;
                    }
                    lastState = EnemyState.ATTACKING;
                    Attacking();
                    break;
                }
            case EnemyState.FLEEING:
                {
                    if (lastState != EnemyState.CHASING && lastState != EnemyState.FLEEING)
                    {
                        seeker.StartPath(transform.position, target.position, OnPathComplete);
                        StartCoroutine(UpdatePath());
                    }
                    if (attackTimer <= 0.0f && enemy.tipo == ENEMY_TYPE.SKT)
                    {
                        anim.SetBool("Slashing", false);
                    }
                    if (attackTimer <= 0.0f && enemy.tipo == ENEMY_TYPE.SPR)
                    {
                        anim.SetBool("Thrusting", false);
                    }
                    lastState = EnemyState.FLEEING;
                    pathStarted = true;
                    Fleeing();
                    break;
                }
        }
    }

    private void Idle()
    {
        rigid2D.velocity = new Vector2(0, 0);
        anim.SetBool("SeMueve", false);
        anim.SetFloat("DirX", 0);
        anim.SetFloat("DirY", 0);
    }
    private void Roaming()
    {
        if (roamingTimer > 0)
        {            
            roamingTimer -= Time.deltaTime;
            if (roamingTimer <= 1)
            {
                anim.SetBool("SeMueve", false);
                anim.SetFloat("DirX", 0);
                anim.SetFloat("DirY", 0);
            }
        }
        else
        {
            anim.SetBool("SeMueve", true);
            roamingTimer = roamingDuration;
            float angle = Random.Range(0.0f, 2 * Mathf.PI);
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * roamingSpeed * Time.deltaTime;
            if (dir.x < 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
            {
                anim.SetInteger("LD", 4);
                anim.SetFloat("DirX", -1);
                anim.SetFloat("DirY", 0);
                anim.SetFloat("LastX", -1);
                anim.SetFloat("LastY", 0);
            }
            else if (dir.x > 0 && Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                anim.SetInteger("LD", 2);
                anim.SetFloat("DirX", 1);
                anim.SetFloat("DirY", 0);
                anim.SetFloat("LastX", 1);
                anim.SetFloat("LastY", 0);
            }
            else if (dir.y > 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            {
                anim.SetInteger("LD", 1);
                anim.SetFloat("DirX", 0);
                anim.SetFloat("DirY", 1);
                anim.SetFloat("LastX", 0);
                anim.SetFloat("LastY", 1);
            }
            else if (dir.y < 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            {
                anim.SetInteger("LD", 3);
                anim.SetFloat("DirX", 0);
                anim.SetFloat("DirY", -1);
                anim.SetFloat("LastX", 0);
                anim.SetFloat("LastY", -1);
            }
            rigid2D.AddForce(dir, fMode);
        }
    }
    private void Fleeing()
    {
        if (attackTimer > 0)
            return;
        anim.SetBool("SeMueve", true);
        anim.SetBool("Slashing", false);
        if (target == null)
        {
            //Debug.Log("No hay objetivo. FixedUpdate");
            return;
        }
        //TODO: look at target function

        if (path == null)
        {
            return;
        }
        if (_currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }
            //Debug.Log("Fin del camino");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;
        //find direction to next waypoint
        Vector2 direction = (path.vectorPath[_currentWaypoint] - transform.position).normalized;
        //dir *= speed * Time.deltaTime;

        Vector2 dir = new Vector2(direction.x, direction.y) * speed * Time.deltaTime * -1;
        rigid2D.AddForce(dir, fMode);
        if (dir.x < 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
        {
            anim.SetInteger("LD", 4);
            anim.SetFloat("DirX", -1);
            anim.SetFloat("DirY", 0);
            anim.SetFloat("LastX", -1);
            anim.SetFloat("LastY", 0);
        }
        else if (dir.x > 0 && Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            anim.SetInteger("LD", 2);
            anim.SetFloat("DirX", 1);
            anim.SetFloat("DirY", 0);
            anim.SetFloat("LastX", 1);
            anim.SetFloat("LastY", 0);
        }
        else if (dir.y > 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            anim.SetInteger("LD", 1);
            anim.SetFloat("DirX", 0);
            anim.SetFloat("DirY", 1);
            anim.SetFloat("LastX", 0);
            anim.SetFloat("LastY", 1);
        }
        else if (dir.y < 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            anim.SetInteger("LD", 3);
            anim.SetFloat("DirX", 0);
            anim.SetFloat("DirY", -1);
            anim.SetFloat("LastX", 0);
            anim.SetFloat("LastY", -1);
        }
        float dist = Vector2.Distance(transform.position, path.vectorPath[_currentWaypoint]);

        if (dist < nextWaypointDist)
        {
            _currentWaypoint++;
            return;
        }
    }
    private void Chasing()
    {
        if (attackTimer > 0)
            return;
        anim.SetBool("SeMueve", true);
        anim.SetBool("Slashing", false);
        if (target == null)
        {
            //Debug.Log("No hay objetivo. FixedUpdate");
            return;
        }
        //TODO: look at target function

        if (path == null)
        {
            return;
        }
        if (_currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }
            //Debug.Log("Fin del camino");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;
        //find direction to next waypoint
        Vector2 direction = (path.vectorPath[_currentWaypoint] - transform.position).normalized;
        //dir *= speed * Time.deltaTime;

        Vector2 dir = new Vector2(direction.x, direction.y) * speed * Time.deltaTime;
        rigid2D.AddForce(dir, fMode);
        
        if (dir.x < 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
        {
            anim.SetInteger("LD", 4);
            anim.SetFloat("DirX", -1);
            anim.SetFloat("DirY", 0);
            anim.SetFloat("LastX", -1);
            anim.SetFloat("LastY", 0);
        }
        else if (dir.x > 0 && Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            anim.SetInteger("LD", 2);
            anim.SetFloat("DirX", 1);
            anim.SetFloat("DirY", 0);
            anim.SetFloat("LastX", 1);
            anim.SetFloat("LastY", 0);
        }
        else if (dir.y > 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            anim.SetInteger("LD", 1);
            anim.SetFloat("DirX", 0);
            anim.SetFloat("DirY", 1);
            anim.SetFloat("LastX", 0);
            anim.SetFloat("LastY", 1);
        }
        else if (dir.y < 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            anim.SetInteger("LD", 3);
            anim.SetFloat("DirX", 0);
            anim.SetFloat("DirY", -1);
            anim.SetFloat("LastX", 0);
            anim.SetFloat("LastY", -1);
        }

        float dist = Vector2.Distance(transform.position, path.vectorPath[_currentWaypoint]);

        if (dist < nextWaypointDist)
        {
            _currentWaypoint++;
            return;
        }
    }
    private void Attacking()
    {          
            dir = (target.position - transform.position).normalized;
            switch (enemy.tipo)
            {
                case ENEMY_TYPE.SKT:
                    anim.SetBool("SeMueve", false);
                    attackArea.size = attackReduced;
                    attackArea.offset = attackOrigin;

                    if (dir.x < 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
                    {
                        attackDirection = Direction.LEFT;
                        anim.SetInteger("LD", 4);
                        anim.SetFloat("LastX", -1);
                        anim.SetFloat("LastY", 0);
                    }
                    else if (dir.x > 0 && Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                    {
                        attackDirection = Direction.RIGHT;
                        anim.SetInteger("LD", 2);
                        anim.SetFloat("LastX", 1);
                        anim.SetFloat("LastY", 0);
                    }
                    else if (dir.y > 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
                    {
                        attackDirection = Direction.UP;
                        anim.SetInteger("LD", 1);
                        anim.SetFloat("LastX", 0);
                        anim.SetFloat("LastY", 1);
                    }
                    else if (dir.y < 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
                    {
                        attackDirection = Direction.DOWN;
                        anim.SetInteger("LD", 3);
                        anim.SetFloat("LastX", 0);
                        anim.SetFloat("LastY", -1);
                    }
                    if (attackTimer <= 0)
                    {
                        anim.SetBool("Slashing", true);
                        attackArea.size = attackSize;
                        switch (attackDirection)
                        {
                            case Direction.LEFT:
                                {
                                    attackArea.offset += new Vector2(-stopDistance, 0);
                                    break;
                                }
                            case Direction.RIGHT:
                                {
                                    attackArea.offset += new Vector2(stopDistance, 0);
                                    break;
                                }
                            case Direction.UP:
                                {
                                    attackArea.offset += new Vector2(0, stopDistance);
                                    break;
                                }
                            case Direction.DOWN:
                                {
                                    attackArea.offset += new Vector2(0, -stopDistance);
                                    break;
                                }
                        }
                        attackTimer = attackCD;
                    }
                    break;
            case ENEMY_TYPE.CON:
                anim.SetBool("SeMueve", false);
                attackArea.size = attackReduced;
                attackArea.offset = attackOrigin;

                if (dir.x < 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
                {
                    attackDirection = Direction.LEFT;
                    anim.SetInteger("LD", 4);
                    anim.SetFloat("LastX", -1);
                    anim.SetFloat("LastY", 0);
                }
                else if (dir.x > 0 && Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                {
                    attackDirection = Direction.RIGHT;
                    anim.SetInteger("LD", 2);
                    anim.SetFloat("LastX", 1);
                    anim.SetFloat("LastY", 0);
                }
                else if (dir.y > 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
                {
                    attackDirection = Direction.UP;
                    anim.SetInteger("LD", 1);
                    anim.SetFloat("LastX", 0);
                    anim.SetFloat("LastY", 1);
                }
                else if (dir.y < 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
                {
                    attackDirection = Direction.DOWN;
                    anim.SetInteger("LD", 3);
                    anim.SetFloat("LastX", 0);
                    anim.SetFloat("LastY", -1);
                }
                if (attackTimer <= 0)
                {
                    anim.SetBool("Slashing", true);
                    attackArea.size = attackSize;
                    switch (attackDirection)
                    {
                        case Direction.LEFT:
                            {
                                attackArea.offset += new Vector2(-stopDistance, 0);
                                break;
                            }
                        case Direction.RIGHT:
                            {
                                attackArea.offset += new Vector2(stopDistance, 0);
                                break;
                            }
                        case Direction.UP:
                            {
                                attackArea.offset += new Vector2(0, stopDistance);
                                break;
                            }
                        case Direction.DOWN:
                            {
                                attackArea.offset += new Vector2(0, -stopDistance);
                                break;
                            }
                    }
                    attackTimer = attackCD;
                }
                break;

            /*case ENEMY_TYPE.ZMB:
                    if (!shot)
                {
                    if (dir.x < 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y) && dir.y >= 0)
                    {
                        attackDirection = Direction.LEFT;
                        movementDirection = 0;
                    }
                    else if (dir.x < 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y) && dir.y < 0)
                    {
                        attackDirection = Direction.LEFT;
                        movementDirection = 1;
                    }
                    else if (dir.x > 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y) && dir.y >= 0)
                    {
                        attackDirection = Direction.RIGHT;
                        movementDirection = 1;
                    }
                    else if (dir.x > 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y) && dir.y < 0)
                    {
                        attackDirection = Direction.RIGHT;
                        movementDirection = 0;
                    }
                    else if (dir.y > 0 && Mathf.Abs(dir.y) >= Mathf.Abs(dir.x) && dir.x >= 0)
                    {
                        attackDirection = Direction.UP;
                        movementDirection = 0;
                    }
                    else if (dir.y > 0 && Mathf.Abs(dir.y) >= Mathf.Abs(dir.x) && dir.x < 0)
                    {
                        attackDirection = Direction.UP;
                        movementDirection = 1;
                    }
                    else if (dir.y < 0 && Mathf.Abs(dir.y) >= Mathf.Abs(dir.x) && dir.x >= 0)
                    {
                        attackDirection = Direction.DOWN;
                        movementDirection = 1;
                    }
                    else if (dir.y < 0 && Mathf.Abs(dir.y) >= Mathf.Abs(dir.x) && dir.x < 0)
                    {
                        attackDirection = Direction.DOWN;
                        movementDirection = 0;
                    }
                    switch (attackDirection)
                    {
                        case Direction.LEFT:
                            {
                                if (movementDirection == 0)
                                {
                                    anim.SetInteger("LD", 1);
                                    anim.SetFloat("LastX", 0);
                                    anim.SetFloat("LastY", 1);
                                }
                                else
                                {
                                    anim.SetInteger("LD", 3);
                                    anim.SetFloat("LastX", 0);
                                    anim.SetFloat("LastY", -1);
                                }
                                attackArea.offset = new Vector2(-5 * stopDistance, 0);
                                attackArea.size = new Vector2((2 * 0.095f + 10 * stopDistance), 0.095f);
                                break;
                            }
                        case Direction.RIGHT:
                            {
                                if (movementDirection == 0)
                                {
                                    anim.SetInteger("LD", 3);
                                    anim.SetFloat("LastX", 0);
                                    anim.SetFloat("LastY", -1);
                                }
                                else
                                {
                                    anim.SetInteger("LD", 1);
                                    anim.SetFloat("LastX", 0);
                                    anim.SetFloat("LastY", 1);
                                }
                                attackArea.offset = new Vector2(5 * stopDistance, 0);
                                attackArea.size = new Vector2((2 * 0.095f + 10 * stopDistance), 0.095f);
                                break;
                            }
                        case Direction.UP:
                            {
                                if (movementDirection == 0)
                                {
                                    anim.SetInteger("LD", 2);
                                    anim.SetFloat("LastX", 1);
                                    anim.SetFloat("LastY", 0);
                                }
                                else
                                {
                                    anim.SetInteger("LD", 4);
                                    anim.SetFloat("LastX", -1);
                                    anim.SetFloat("LastY", 0);
                                }
                                attackArea.offset = new Vector2(0, 5 * stopDistance);
                                attackArea.size = new Vector2(0.095f, (2 * 0.095f + 10 * stopDistance));
                                break;
                            }
                        case Direction.DOWN:
                            {
                                if (movementDirection == 0)
                                {
                                    anim.SetInteger("LD", 4);
                                    anim.SetFloat("LastX", -1);
                                    anim.SetFloat("LastY", 0);
                                }
                                else
                                {
                                    anim.SetInteger("LD", 2);
                                    anim.SetFloat("LastX", 1);
                                    anim.SetFloat("LastY", 0);
                                }
                                attackArea.offset = new Vector2(0, -5 * stopDistance);
                                attackArea.size = new Vector2(0.095f, (2 * 0.095f + 10 * stopDistance));
                                break;
                            }
                    }
                    anim.SetBool("SeMueve", true);
                    movementNormalizedVector = (movementDirection == 0 ? new Vector2(dir.y, -dir.x) : new Vector2(-dir.y, dir.x)).normalized;
                    rigid2D.AddForce(movementNormalizedVector * speed * Time.deltaTime);
                }
                    if (attackColliderScript.canShoot)
                    {
                        switch (attackDirection)
                        {
                            case Direction.LEFT:
                                {
                                    anim.SetInteger("LD", 4);
                                    anim.SetFloat("LastX", -1);
                                    anim.SetFloat("LastY", 0);
                                    break;
                                }
                            case Direction.RIGHT:
                                {
                                    anim.SetInteger("LD", 2);
                                    anim.SetFloat("LastX", 1);
                                    anim.SetFloat("LastY", 0);
                                    break;
                                }
                            case Direction.UP:
                                {
                                    anim.SetInteger("LD", 1);
                                    anim.SetFloat("LastX", 0);
                                    anim.SetFloat("LastY", 1);
                                    break;
                                }
                            case Direction.DOWN:
                                {
                                    anim.SetInteger("LD", 3);
                                    anim.SetFloat("LastX", 0);
                                    anim.SetFloat("LastY", -1);
                                    break;
                                }
                        }
                        rigid2D.velocity = new Vector2(0, 0);
                        attackTimer = shootCD;
                        anim.SetBool("SeMueve", false);
                        anim.SetBool("Shooting", true);
                        shot = true;
                        shotDirection = attackDirection;                        
                        anim.SetBool("Shooting", false);
                    }
                    if (attackTimer <= 0 && shot)
                    {
                    switch (shotDirection)
                    {
                        case Direction.LEFT:
                            {
                                anim.SetInteger("LD", 4);
                                anim.SetFloat("LastX", -1);
                                anim.SetFloat("LastY", 0);
                                break;
                            }
                        case Direction.RIGHT:
                            {
                                anim.SetInteger("LD", 2);
                                anim.SetFloat("LastX", 1);
                                anim.SetFloat("LastY", 0);
                                break;
                            }
                        case Direction.UP:
                            {
                                anim.SetInteger("LD", 1);
                                anim.SetFloat("LastX", 0);
                                anim.SetFloat("LastY", 1);
                                break;
                            }
                        case Direction.DOWN:
                            {
                                anim.SetInteger("LD", 3);
                                anim.SetFloat("LastX", 0);
                                anim.SetFloat("LastY", -1);
                                break;
                            }
                    }
                    flechaClon = Instantiate<GameObject>(flecha, transform.position, transform.rotation);
                    shot = false;
                    }
                    break;
                    */
            case ENEMY_TYPE.NIG:
                if (animTimer <= 0 && casting)
                {
                    casting = false;
                    ConClon = Instantiate<GameObject>(Con, new Vector2(target.transform.position.x + 4*stopDistance, target.transform.position.y), target.transform.rotation);
                    ConClon = Instantiate<GameObject>(Con, new Vector2(target.transform.position.x - 4*stopDistance, target.transform.position.y), target.transform.rotation);
                    ConClon = Instantiate<GameObject>(Con, new Vector2(target.transform.position.x, target.transform.position.y + 4*stopDistance), target.transform.rotation);
                    ConClon = Instantiate<GameObject>(Con, new Vector2(target.transform.position.x, target.transform.position.y - 4*stopDistance), target.transform.rotation);
                }
                if (attackTimer <= 0)
                {
                    casting = true;
                    if (dir.x < 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
                    {
                        anim.SetInteger("LD", 4);
                        anim.SetFloat("LastX", -1);
                        anim.SetFloat("LastY", 0);
                    }
                    else if (dir.x > 0 && Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                    {
                        anim.SetInteger("LD", 2);
                        anim.SetFloat("LastX", 1);
                        anim.SetFloat("LastY", 0);
                    }
                    else if (dir.y > 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
                    {
                        anim.SetInteger("LD", 1);
                        anim.SetFloat("LastX", 0);
                        anim.SetFloat("LastY", 1);
                    }
                    else if (dir.y < 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
                    {
                        anim.SetInteger("LD", 3);
                        anim.SetFloat("LastX", 0);
                        anim.SetFloat("LastY", -1);
                    }
                    anim.SetBool("Slashing", true);
                    animTimer = animDuration;
                    attackTimer = enemy.conjuringCD;
                }

                break;
                case ENEMY_TYPE.SPR:
                    anim.SetBool("SeMueve", false);
                    attackArea.size = attackReduced;
                    attackArea.offset = attackOrigin;

                    if (dir.x < 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
                    {
                        attackDirection = Direction.LEFT;
                        anim.SetInteger("LD", 4);
                        anim.SetFloat("LastX", -1);
                        anim.SetFloat("LastY", 0);
                    }
                    else if (dir.x > 0 && Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                    {
                        attackDirection = Direction.RIGHT;
                        anim.SetInteger("LD", 2);
                        anim.SetFloat("LastX", 1);
                        anim.SetFloat("LastY", 0);
                    }
                    else if (dir.y > 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
                    {
                        attackDirection = Direction.UP;
                        anim.SetInteger("LD", 1);
                        anim.SetFloat("LastX", 0);
                        anim.SetFloat("LastY", 1);
                    }
                    else if (dir.y < 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
                    {
                        attackDirection = Direction.DOWN;
                        anim.SetInteger("LD", 3);
                        anim.SetFloat("LastX", 0);
                        anim.SetFloat("LastY", -1);
                    }
                    if (attackTimer <= 0)
                    {
                        anim.SetBool("Thrusting", true);
                        switch (attackDirection)
                        {
                            case Direction.LEFT:
                                {
                                    attackArea.size = new Vector2(6 * 0.095f, 2 * 0.095f);                                    
                                    attackArea.offset += new Vector2(-stopDistance * 2, 0);
                                    break;
                                }
                            case Direction.RIGHT:
                                {
                                    attackArea.size = new Vector2(6 * 0.095f, 2 * 0.095f);
                                    attackArea.offset += new Vector2(stopDistance * 2, 0);
                                    break;
                                }
                            case Direction.UP:
                                {
                                    attackArea.size = new Vector2(2 * 0.095f, 6 * 0.095f);
                                    attackArea.offset += new Vector2(0, stopDistance * 2);
                                    break;
                                }
                            case Direction.DOWN:
                                {
                                    attackArea.size = new Vector2(2 * 0.095f, 6 * 0.095f);
                                    attackArea.offset += new Vector2(0, -stopDistance * 2);
                                    break;
                                }
                        }
                        attackTimer = attackCD;
                    }
                    break;
            
                                    
        }        
    }

    public float map(float x, float in_min, float in_max, float out_on_min, float out_on_max)
    {
        return (x - in_min) * (out_on_max - out_on_min) / (in_max - in_min) + out_on_min;
    }
}