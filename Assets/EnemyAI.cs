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
    public bool inRoom = false;

    //el objetivo/jugador
    public GameObject p;
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
    //Cooldown del ataque del enemigo
    private float attackCD;
    private float attackTimer = 0;
    //Collider de ataque
    public BoxCollider2D attackArea;
    //Center (offset) of the circlecollider2d
    private Vector2 attackOrigin;
    //Full size of the CircleCollider2D
    private Vector2 attackSize;
    private Vector2 attackReduced = new Vector2(0.01f, 0.01f);

    void Start()
    {
        target = p.GetComponent<Transform>();
        targetCollider = p.GetComponent<CircleCollider2D>();
        player = p.GetComponent<PlayerClass>();
        playerRig = p.GetComponent<Rigidbody2D>();
        
        attackOrigin = attackArea.offset;
        attackSize = attackArea.size;
        attackArea.size = attackReduced;
        enemy = GetComponent<EnemyClass>();
        attackCD = enemy.attackCD;
        speed = enemy.speed;
        roamingSpeed = enemy.roamingSpeed;
        fleeingSpeed = enemy.fleeingSpeed;

        seeker = GetComponent<Seeker>();
        rigid2D = GetComponent<Rigidbody2D>();
        if (target == null)
        {
            //Debug.Log("No se ha encontrado objetivo.");
            return;
        }
        distanceToTarget = Vector2.Distance(target.position, transform.position);
        //return new path result to onpath complete
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine(UpdatePath());
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
        inRoom = room.IsTouching(targetCollider);
        //Gets the distance from the player to the ai 
        distanceToTarget = Vector2.Distance(target.position, transform.position);

        //idle timer control
        if (idleTimer > 0)        
            idleTimer -= Time.deltaTime;       

        //Conditionals that set the ai state 
        if (enemy.healthPoints <= 15)
        {
            state = EnemyState.FLEEING;
        }
        else if ((distanceToTarget <= engageDistance && distanceToTarget > stopDistance) || (inRoom && distanceToTarget > stopDistance))
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


        switch (state)
        {
            case EnemyState.IDLE:
                {
                    Idle();
                    break;
                }
            case EnemyState.CHASING:
                {
                    Chasing();
                    break;
                }
            case EnemyState.ROAMING:
                {
                    Roaming();
                    break;
                }
            case EnemyState.ATTACKING:
                {
                    Attacking();
                    break;
                }
            case EnemyState.FLEEING:
                {
                    Fleeing();
                    break;
                }
        }
    }

    private void Idle()
    {
        rigid2D.velocity = new Vector2(0, 0);
    }
    private void Roaming()
    {
        if (roamingTimer > 0)
            roamingTimer -= Time.deltaTime;
        else
        {
            roamingTimer = roamingDuration;
            float angle = Random.Range(0.0f, 2 * Mathf.PI);
            rigid2D.AddForce(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle))*roamingSpeed*Time.deltaTime, fMode);
        }
    }
    private void Fleeing()
    {
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
        Vector2 dir = (path.vectorPath[_currentWaypoint] - transform.position).normalized;
        //dir *= speed * Time.deltaTime;

        Vector2 velocity = new Vector2(dir.x, dir.y) * speed * Time.deltaTime;
        rigid2D.AddForce(-velocity, fMode);

        float dist = Vector2.Distance(transform.position, path.vectorPath[_currentWaypoint]);

        if (dist < nextWaypointDist)
        {
            _currentWaypoint++;
            return;
        }
    }
    private void Chasing()
    {
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
        Vector2 dir = (path.vectorPath[_currentWaypoint] - transform.position).normalized;
        //dir *= speed * Time.deltaTime;

        Vector2 velocity = new Vector2(dir.x, dir.y) * speed * Time.deltaTime;
        rigid2D.AddForce(velocity, fMode);

        float dist = Vector2.Distance(transform.position, path.vectorPath[_currentWaypoint]);

        if (dist < nextWaypointDist)
        {
            _currentWaypoint++;
            return;
        }
    }
    private void Attacking()
    {
        attackArea.size = attackReduced;
        attackArea.offset = attackOrigin;

        Vector2 dir = (target.position - transform.position).normalized;

        if (dir.x < 0 && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
        {
            attackDirection = Direction.LEFT;
        }
        else if (dir.x > 0 && Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            attackDirection = Direction.RIGHT;
        }
        else if (dir.y > 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            attackDirection = Direction.UP;
        } 
        else if (dir.y < 0 && Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            attackDirection = Direction.DOWN;
        }        
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
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
    }
}