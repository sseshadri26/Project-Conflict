using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] protected int health;
    [SerializeField] protected float speed;
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

    // Start is called before the first frame update
    protected void EnemyStart()
    {
        trfm = transform;
        rb = GetComponent<Rigidbody2D>();
    }

    protected void EnemyFixedUpdate()
    {
        //only run this code every other tick to save resources
        if (every2)
        {
            determineTargetPlayer();
        }
        every2 = !every2;

        if (!hasTargetPlayer)
        {
            idleRoam();
        }
        if (stunTmr>0) { stunTmr--; }
        doKnockback();
    }

    void determineTargetPlayer()
    {
        if (hasTargetPlayer)
        {
            trfm.rotation = Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - targetPlayerTrfm.position.y, trfm.position.x - targetPlayerTrfm.position.x) * Mathf.Rad2Deg + 90, Vector3.forward);

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
                }
                if (Physics2D.Linecast(trfm.position, playerTwoTrfm.position, 1 << 6))
                {
                    hasTargetPlayer = false;
                    targetPlayerTrfm = null;
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
                    }
                }
                if (Physics2D.Linecast(trfm.position, playerOneTrfm.position, 1 << 6))
                {
                    hasTargetPlayer = false;
                    targetPlayerTrfm = null;
                }
            }

        } else
        {
            if (!Physics2D.Linecast(trfm.position, playerOneTrfm.position, 1 << 6))
            {
                targetPlayerTrfm = playerOneTrfm;
                targetedPlayer = p1;
                hasTargetPlayer = true;
            }
            if (!Physics2D.Linecast(trfm.position, playerTwoTrfm.position, 1 << 6))
            {
                targetPlayerTrfm = playerTwoTrfm;
                targetedPlayer = p2;
                hasTargetPlayer = true;
            }
        }
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
