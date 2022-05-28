using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{

    [SerializeField] protected int health = 100;
    [SerializeField] protected float speed = 0.05f;
    [SerializeField] protected float rotationSpeed = 7f;
    private static Transform playerOneTrfm, playerTwoTrfm;
    [SerializeField] protected Transform targetPlayerTrfm;
    [SerializeField] protected bool hasTargetPlayer = false;
    protected int abilityCooldown, stunTmr;
    public int abilityCast;

    int walkTmr, walkThreshold, turnTmr;
    bool turnDirection;

    protected const bool p1 = false, p2 = true;
    protected bool targetedPlayer = p1;

    protected Rigidbody2D rb;
    protected Transform trfm;

    private Vector3 knockbackDirection;
    protected float knockbackForce;

    private bool every2;

    // Animator variables
    protected Animator animator;
    private SpriteRenderer sprite;
    protected bool isStill; // set to false if enemy walks
    [SerializeField] int stunAnimationRepetitions = 1;

    // Pathfinding variables
    [SerializeField] protected float inRangeOfPlayer = 1f;    // How far from player should enemy stop?
    [SerializeField] protected float maxChaseDistance = 15f;    // How far before enemy stops chasing player?
    private float nextWaypointDistance = .5f;
    protected Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    protected bool tooFar = false;
    // Path rememberance variables
    [SerializeField] protected float memoryTime = 8f;      // How long should enemy remember and chase player?
    private bool coroutineRunning = false;

    // Start is called before the first frame update
    protected void EnemyStart()
    {
        trfm = transform;
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        animator = GetComponentInChildren<Animator>();
        sprite = this.transform.Find("sprite").GetComponent<SpriteRenderer>();
    }

    // Start calculating enemy's path to player
    public void UpdatePath()
    {
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
    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected void EnemyFixedUpdate()
    {
        isStill = true;
        if (path == null || path.GetTotalLength() > maxChaseDistance)
        {
            tooFar = true;
        }
        else
        {
            tooFar = false;
        }
        UpdatePath();
        if (!hasTargetPlayer)
        {
            idleRoam();
        }
        if (stunTmr > 0) { stunTmr--; }
        doKnockback();
    }

    void LateUpdate()
    {
        // Update animator variables
        animator.SetBool("isMoving", !isStill);
        animator.SetFloat("forwardX", trfm.up.x);
        animator.SetFloat("forwardY", trfm.up.y);
    }

    protected void moveToPlayer(float thisSpeed)
    {
        if (path == null || path.GetTotalLength() > maxChaseDistance)
            return;

        // Reached player?
        if (path.GetTotalLength() < inRangeOfPlayer || currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            trfm.up = Vector2.Lerp(trfm.up, ((Vector2)(targetPlayerTrfm.position - trfm.position)).normalized, Time.deltaTime * rotationSpeed);
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        // Rotate enemy
        trfm.up = Vector2.Lerp(trfm.up, ((Vector2)(path.vectorPath[currentWaypoint] - trfm.position)).normalized, Time.deltaTime * rotationSpeed);
        // Move enemy
        moveForward(thisSpeed);
        // Pick next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
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

                    if (coroutineRunning)
                    {
                        StopCoroutine(losePlayerTarget());
                        coroutineRunning = false;
                    }
                }
                if (Physics2D.Linecast(trfm.position, playerTwoTrfm.position, 1 << 6))
                {
                    // Player out of sight
                    if (!coroutineRunning)
                    {
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

                        if (coroutineRunning)
                        {
                            StopCoroutine(losePlayerTarget());
                            coroutineRunning = false;
                        }
                    }
                }
                if (Physics2D.Linecast(trfm.position, playerOneTrfm.position, 1 << 6))
                {
                    // Player out of sight
                    if (!coroutineRunning)
                    {
                        coroutineRunning = true;
                        StartCoroutine(losePlayerTarget());
                    }
                }
            }

        }
        else if (playerOneTrfm)
        {
            if (!Physics2D.Linecast(trfm.position, playerOneTrfm.position, 1 << 6))
            {
                targetPlayerTrfm = playerOneTrfm;
                targetedPlayer = p1;
                hasTargetPlayer = true;

                if (coroutineRunning)
                {
                    StopCoroutine(losePlayerTarget());
                    coroutineRunning = false;
                }
            }
            if (!Physics2D.Linecast(trfm.position, playerTwoTrfm.position, 1 << 6))
            {
                targetPlayerTrfm = playerTwoTrfm;
                targetedPlayer = p2;
                hasTargetPlayer = true;

                if (coroutineRunning)
                {
                    StopCoroutine(losePlayerTarget());
                    coroutineRunning = false;
                }
            }
        }
    }

    // Enemy forgets player after memoryTime seconds
    IEnumerator losePlayerTarget()
    {
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

    protected virtual void moveForward(float spd)
    {
        trfm.position += trfm.up * spd;
        isStill = false;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        {


            takeDamageFromSource(col.gameObject);


            if (col.GetComponent<playerMeleeObj>() && col.GetComponent<playerMeleeObj>().heldByP1) { takeKnockback(.2f, playerOneTrfm.position); }
            else { takeKnockback(.2f, playerTwoTrfm.position); }
        }
    }

    // Copied and modified from player's hit animation
    private IEnumerator HitAnimation()
    {
        // loops decrease then increase alpha channel in order to create a blinking effect
        // when hitting a hazard; can potentially extend this to getting hit by enemies
        // if you replace stunTimer with a parameter
        Color tmp = sprite.color;
        int timeLeft = stunAnimationRepetitions;
        while (timeLeft > 0)
        {
            for (float alpha = 1f; alpha >= 0f; alpha -= 0.1f)
            {
                tmp.a = alpha;
                sprite.color = tmp;
                yield return new WaitForSeconds(0.01f);
            }
            for (float alpha = 0; alpha <= 1f; alpha += 0.1f)
            {
                tmp.a = alpha;
                sprite.color = tmp;
                yield return new WaitForSeconds(0.01f);
            }
            timeLeft--;
        }

        // reset alpha to normal and quit the coroutine
        tmp.a = 1f;
        sprite.color = tmp;
        yield break;
    }







    //PUBLIC FUNCTIONS

    public static void assignPlayerTransforms(Transform playerOneTransform, Transform playerTwoTransform)
    {
        playerOneTrfm = playerOneTransform;
        playerTwoTrfm = playerTwoTransform;
    }

    public int setHealth(int h)
    {
        health = h;
        return health;
    }

    public int takeDamage(int amount, GameObject col)
    {
        //Debug.Log(amount);
        health -= amount;
        if (health < 1)
        {
            //Debug.Log(col.gameObject.transform.root.gameObject.name);
            GameObject score = GameObject.Find("Score");

            if (col.gameObject.GetComponent<playerMeleeObj>())
            {
                if (col.gameObject.GetComponent<playerMeleeObj>().heldByP1)
                {
                    //p1.score += 1;
                    //Debug.Log("Player 1 killed enemy");
                    score.GetComponent<SetScore>().addP1Score(1);
                }
                else
                {
                    //p2.score += 1;
                    //Debug.Log("Player 2 killed enemy");
                    score.GetComponent<SetScore>().addP2Score(1);
                }
                Destroy(gameObject);
                return 0;

            }

            bool ownerIsP1 = col.gameObject.transform.root.gameObject.GetComponent<weapon>().lastOwnerWasP1;

            //if name has the digit 1 in it, then it is player one
            if (ownerIsP1)
            {
                //find the "score" object in scene

                //add to the score
                score.GetComponent<SetScore>().addP1Score(1);
                //Debug.Log("Player 1 killed enemy");
            }
            else
            {
                score.GetComponent<SetScore>().addP2Score(1);
                //Debug.Log("Point for player 2");
            }
            Destroy(gameObject);
            return 0;

        }

        // Enemy blinks a few times when damaged
        StartCoroutine(HitAnimation());
        return health;
    }

    public void takeDamageFromSource(GameObject col)
    {
        //Debug.Log(col.name);

        //tinyspinattack
        if (col.name.Contains("tinySpinSttackObj"))
        {
            takeDamage(1, col);
        }

        //fork stab
        else if (col.name.Contains("thrustAttackObj"))
        {
            takeDamage(2, col);
        }

        //fork spin
        else if (col.name == "spinSttackObj")
        {
            takeDamage(5, col);
        }

        //fork throw
        else if (col.name.Contains("throwBlur"))
        {
            takeDamage(10, col);
        }
        else
        {
            takeDamage(1, col);
        }

    }

    public bool stun(float duration)
    {
        int ticks = Mathf.RoundToInt(duration * 50);
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

    public void TriggerEnter(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        {


            takeDamageFromSource(col.gameObject);


            if (col.GetComponent<playerMeleeObj>() && col.GetComponent<playerMeleeObj>().heldByP1) { takeKnockback(.2f, playerOneTrfm.position); }
            else { takeKnockback(.2f, playerTwoTrfm.position); }
        }
    }
}
