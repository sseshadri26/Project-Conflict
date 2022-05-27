using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashingEnemy : meleeEnemy
{
    float startTime;    // Track time for step cycle
    [SerializeField] protected AnimationCurve stepCurve;    // Custom curve to control forward steps
    [SerializeField] protected AnimationCurve offsetCurve;    // Custom curve to control upward offset during steps
    [SerializeField] protected float step_period_time = 2.0f;
    public bool abilityStep = false;   // True if current step uses ability
    bool animationTriggered = false; // Used to ensure animation only triggers once

    private void Start()
    {
        meleeEnemyStart();
        abilityCooldown = Random.Range(200, 400);
        startTime = Time.time;
    }
    private void FixedUpdate()
    {
        EnemyFixedUpdate();
        if (stunTmr < 1 && hasTargetPlayer)
        {
            moveToPlayer(speed);
            doAbilityCycle();
        }
    }

    void doAbilityCycle()
    {
        if (abilityCooldown > 0 || tooFar)
        {
            abilityCooldown--;
        }
        else
        {
            // takeDamage(100);
            castAttack(200, 400, 1);
        }
    }

    protected override void moveForward(float spd) {
        float t = (Time.time - startTime)/step_period_time;
        if (t >= 1) {
            startTime = Time.time; 
            t = 0;

            // Set next step to use ability?
            if (abilityCast > 0) {
                abilityStep = true;
                abilityCast--;
                animationTriggered = false;
            } else {
                abilityStep = false;
            }
        }
        if (abilityStep) {
            if (stepCurve.Evaluate(t) > 0 && !animationTriggered) {
                // trigger animation when hamburger starts dashing forward
                animator.SetTrigger("isAttacking");
                animationTriggered = true; 
            }
            spd = spd * 5.0f;
        }
        trfm.position += trfm.up * stepCurve.Evaluate(t) * spd + (new Vector3(0, offsetCurve.Evaluate(t) * .01f, 0));
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    TriggerEnter(col);
    //}
}
