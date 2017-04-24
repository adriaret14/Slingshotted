using UnityEngine;
using System.Collections;
using Pathfinding;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]

public class EnemyAI : MonoBehaviour
{
    enum EnemyState
    {
        IDLE,
        ROAMING,
        CHASING,
        ATTACKING,
        FLEEING,
        DEAD
    }
    //the room
    public BoxCollider2D room;
    public bool inRoom = false;

    //the target
    public Transform target;
    public CircleCollider2D targetCollider;

    //update rate of path
    public float updateRate = 6f;

    //references
    private Seeker _seeker;
    private Rigidbody2D _rig2D;

    //calculated path
    public Path path;

    //ai's speed
    public float speed = 300;

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
    public float idleTimer = 0;
    public float idleDuration = 2f;

    //Roaming timer and speed
    public float roamingTimer = 0;
    public float roamingDuration = 2f;
    public float roamingSpeed = 30;

    //Fleeing speed
    public float fleeingSpeed = 350;

    //The distance within the ai stops moving towards the player.
    public float stopDistance = 0.195f;

    //The distance within the ai keeps chasing if player leaves the room
    public float engageDistance = 1.95f;


    //hp
    public int healthPoints = 100;
    //hit damage to player
    public int damage = 34;

    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rig2D = GetComponent<Rigidbody2D>();
        if (target == null)
        {
            Debug.Log("No se ha encontrado objetivo.");
            return;
        }
        distanceToTarget = Vector2.Distance(target.position, transform.position);
        //return new path result to onpath complete
        _seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine(UpdatePath());
    }
    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            Debug.Log("No hay objetivo. UpdatePath");
            yield return false;
        }
        _seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1 / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Camino calculado. Error? " + p.error);
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
        if (healthPoints <= 0) 
        {
            state = EnemyState.DEAD;
        }
        else if (healthPoints <= 15)
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
            case EnemyState.DEAD:
                {
                    Destroy(gameObject);
                    break;
                }
        }
    }

    private void Idle()
    {
        _rig2D.velocity = new Vector2(0, 0);
    }
    private void Roaming()
    {
        if (roamingTimer > 0)
            roamingTimer -= Time.deltaTime;
        else
        {
            roamingTimer = roamingDuration;
            float angle = Random.Range(0.0f, 2 * Mathf.PI);
            _rig2D.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle))*roamingSpeed*Time.deltaTime;
        }
    }
    private void Fleeing()
    {
        if (target == null)
        {
            Debug.Log("No hay objetivo. FixedUpdate");
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
            Debug.Log("Fin del camino");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;
        //find direction to next waypoint
        Vector2 dir = (path.vectorPath[_currentWaypoint] - transform.position).normalized;
        //dir *= speed * Time.deltaTime;

        Vector2 velocity = new Vector2(dir.x, dir.y) * speed * Time.deltaTime;
        _rig2D.velocity = -velocity;

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
            Debug.Log("No hay objetivo. FixedUpdate");
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
            Debug.Log("Fin del camino");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;
        //find direction to next waypoint
        Vector2 dir = (path.vectorPath[_currentWaypoint] - transform.position).normalized;
        //dir *= speed * Time.deltaTime;

        Vector2 velocity = new Vector2(dir.x, dir.y) * speed * Time.deltaTime;
        _rig2D.velocity = velocity;

        float dist = Vector2.Distance(transform.position, path.vectorPath[_currentWaypoint]);

        if (dist < nextWaypointDist)
        {
            _currentWaypoint++;
            return;
        }
    }
    private void Attacking()
    {
        //TODO: Implementar mecanica de ataque
    }
}




/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour {

    //Objetivo del enemigo (Jugador)
    public Transform target;

    //Actualizaciones por segundo
    public float updateRate = 2f;

    //Cache
    private Seeker seeker;
    private Rigidbody2D rb;

    //Camino optimo calculado
    public Path path;

    //Velocidad del enemigo
    public float speed = 300f;

    //Modo de movimiento fuerza/impulso
    public ForceMode2D fmode;

    [HideInInspector]
    public bool pathIsEnded = false;

    //Maxima distancia desde el enemigo al siguiente nodo del navmesh
    public float nextWaypointDistance = 3;

    //El nodo hacia el cual se mueve el enemigo
    private int currentWaypoint = 0;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            Debug.LogError("No target found.");
            return;
        }

        seeker.StartPath (transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            //TODO: añadir busqueda de jugador
            yield return false;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete (Path p)
    {
        Debug.Log ("Path found. Error: " + p.error);

        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            //TODO: añadir busqueda de jugador
            return;
        }
        if (path == null)
            return;
        
        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;
            Debug.Log("End of path reached.");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //Direccion al siguiente nodo
        
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
        
        Vector2 velocity = new Vector2((path.vectorPath[currentWaypoint] - transform.position).normalized.x, (path.vectorPath[currentWaypoint] - transform.position).normalized.y) * speed * Time.fixedDeltaTime;
        //Se desplaza el enemigo
        //rb.velocity = dir;
        rb.velocity = -velocity;
        Debug.Log("Moving or at least trying.");

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist > nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }
}
*/
