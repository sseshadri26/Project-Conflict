using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{

    [SerializeField] protected int health;
    [SerializeField] protected float speed = 0.05f;
    [SerializeField] protected float rotationSpeed = 7f;
    private static Transform playerOneTrfm, playerTwoTrfm;
    [SerializeField] protected Transform targetPlayerTrfm;
    [SerializeField] protected bool hasTargetPlayer = false;
    [SerializeField] GameObject deathFX;
    protected int abilityCooldown, abilityCast, stunTmr;

    int walkTmr, walkThreshold, turnTmr;
    bool turnDirection;

    protected const bool p1 = false, p2 = true;
    protected bool targetedPlayer = p1;
    
    protected Rigidbody2D rb;
    protected Transform trfm;

    private Vector3 knockbackDirection;
    protected float knockbackForce;

    private bool every2;

    // Pathfinding variables
    [SerializeField] protected float inRangeOfPlayer = 1f;    // How far from player should enemy stop?
    private float nextWaypointDistance = .5f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    // Path rememberance variables
    [SerializeField] protected float memoryTime = 10f;      // How long should enemy remember and chase player?
    private bool coroutineRunning = false;

    // Start is called before the first frame update
    protected void EnemyStart()
    {
        trfm = transform;
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        // InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Start calculating enemy's path to player
    public void UpdatePath() {
         //only run this code every other tick to save resources
        if (every2)
        {
            determineTargetPlayer();
            if (targetPlayerTrfm != null && seeker.IsDone()) 
                seeker.StartPath(rb.position, targetPlayerTrfm.position, OnPathComplete);
        }
        every2 = !every2;
    }

    // Set enemy's path when done calculating
    protected void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
        }
    }

    protected void EnemyFixedUpdate()
    {
        UpdatePath();
        if (!hasTargetPlayer)
        {
            idleRoam();
        }
        if (stunTmr>0) { stunTmr--; }
        doKnockback();
    }

    protected void moveToPlayer(float thisSpeed) {
        if (path == null)
            return;
        
        // Reached player?
        if(path.GetTotalLength() < inRangeOfPlayer) {
            reachedEndOfPath = true;
            trfm.up = Vector2.Lerp(trfm.up, ((Vector2)(targetPlayerTrfm.position - trfm.position)).normalized, Time.deltaTime * rotationSpeed);
            return;
        } else {
            reachedEndOfPath = false;
        }

        // Rotate enemy
        trfm.up = Vector2.Lerp(trfm.up, ((Vector2)(path.vectorPath[currentWaypoint] - trfm.position)).normalized, Time.deltaTime * rotationSpeed);
        // Move enemy
        moveForward(thisSpeed);
        // Pick next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
    }

    void determineTargetPlayer()
    {
        if (hasTargetPlayer)
        {
            if (targetedPlayer == p2)
            {
                if (!Physics2D.Linecast(trfm.position, playerOneTrfm.position, 1 << 6))
                {
                    if (Vector2.Distance(playerOneTrfm.position, trfm.position) < Vector2.Distance(playerTwoTrfm.position, trfm.position)) //if player one is closer to this enemy
                    {
                        //target player one   
                        targetPlayerTrfm = playerOneTrfm;
                        targetedPlayer = p1;
                    }

                    if (coroutineRunning) {
                        StopCoroutine(losePlayerTarget());
                        coroutineRunning = false;
                    }
                }
                if (Physics2D.Linecast(trfm.position, playerTwoTrfm.position, 1 << 6))
                {
                    // Player out of sight
                    if (!coroutineRunning) {
                        coroutineRunning = true;
                        StartCoroutine(losePlayerTarget());
                    }
                }
            }
            else
            {
                if (!Physics2D.Linecast(trfm.position, playerTwoTrfm.position, 1 << 6))
                {
                    if (Vector2.Distance(playerTwoTrfm.position, trfm.position) < Vector2.Distance(playerOneTrfm.position, trfm.position)) //if player two is closer to this enemy
                    {
                        //target player two   
                        targetPlayerTrfm = playerTwoTrfm;
                        targetedPlayer = p2;

                    if (coroutineRunning) {
                        StopCoroutine(losePlayerTarget());
                        coroutineRunning = false;
                    }
                    }
                }
                if (Physics2D.Linecast(trfm.position, playerOneTrfm.position, 1 << 6))
                {
                    // Player out of sight
                    if (!coroutineRunning) {
                        coroutineRunning = true;
                        StartCoroutine(losePlayerTarget());
                    }
                }
            }

        } else
        {
            if (!Physics2D.Linecast(trfm.position, playerOneTrfm.position, 1 << 6))
            {
                targetPlayerTrfm = playerOneTrfm;
                targetedPlayer = p1;
                hasTargetPlayer = true;
                Debug.Log("test");

                if (coroutineRunning) {
                    StopCoroutine(losePlayerTarget());
                    coroutineRunning = false;
                }
            }
            if (!Physics2D.Linecast(trfm.position, playerTwoTrfm.position, 1 << 6))
            {
                targetPlayerTrfm = playerTwoTrfm;
                targetedPlayer = p2;
                hasTargetPlayer = true;

                if (coroutineRunning) {
                    StopCoroutine(losePlayerTarget());
                    coroutineRunning = false;
                }
            }
        }
    }

    // Enemy forgets player after memoryTime seconds
    IEnumerator losePlayerTarget() {
        yield return new WaitForSeconds(memoryTime);
        hasTargetPlayer = false;
        targetPlayerTrfm = null;
        coroutineRunning = false;
    }

    void doKnockback()
    {
        if (knockbackForce > 0)
        {
            trfm.position += knockbackDirection * knockbackForce;
            knockbackForce -= .1f;
        }
    }

    void idleRoam()
    {
        turnTmr--;
        if (turnTmr == 20)
        {
            turnDirection = Random.Range(0, 2) == 0;
        }
        if (turnTmr < 20)
        {
            if (turnDirection == true)
            {
                trfm.Rotate(Vector3.forward * -speed * 70);
            }
            else
            {
                trfm.Rotate(Vector3.forward * speed * 70);
            }
            if (turnTmr < 1)
            {
                turnTmr = Random.Range(50, 250);
            }
        }

        walkTmr--;
        if (walkTmr < walkThreshold)
        {
            moveForward(speed / 2);
            if (walkTmr < 1)
            {
                walkTmr = Random.Range(50, 350);
                walkThreshold = Random.Range(1, 150);
            }
        }
    }

    protected void moveForward(float spd)
    {
        trfm.position += trfm.up * spd;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "fork")
        {

        }
    }







    //PUBLIC FUNCTIONS

    public static void assignPlayerTransforms(Transform playerOneTransform, Transform playerTwoTransform)
    {
        playerOneTrfm = playerOneTransform;
        playerTwoTrfm = playerTwoTransform;
    }

    public int takeDamage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            Destroy(gameObject);
            return 0;
        }
        return health;
    }

    public bool stun(float duration)
    {
        int ticks = Mathf.RoundToInt(duration*50);
        if (stunTmr < ticks)
        {
            stunTmr = ticks;
            return true;
        }
        return false;
    }

    public void takeKnockback(float power, Vector2 source)
    {
        Quaternion currentRotation = trfm.rotation;
        trfm.rotation = Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - source.y, trfm.position.x - source.x) * Mathf.Rad2Deg + 90, Vector3.forward);
        knockbackDirection = trfm.up * -1;
        trfm.rotation = currentRotation;

        knockbackForce += power;
    }
}
