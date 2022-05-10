using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashingEnemy : meleeEnemy
{
    float startTime;    // Track time for step cycle
    [SerializeField] protected AnimationCurve stepCurve;    // Custom curve to control forward steps
    bool abilityStep = false;   // True if current step uses ability

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
        if (abilityCooldown > 0)
        {
            abilityCooldown--;
        }
        else
        {
            castAttack(200, 400, 1);
        }
    }

    protected override void moveForward(float spd) {
        float t = (Time.time - startTime)/2.0f;
        if (t >= 1) {
            startTime = Time.time; 
            t = 0;

            // Set next step to use ability?
            if (abilityCast > 0) {
                abilityStep = true;
                abilityCast--;
            } else {
                abilityStep = false;
                attackObj.SetActive(false);
            }
        }
        if (abilityStep) {
            spd = spd * 5.0f;
        }
        trfm.position += trfm.up * stepCurve.Evaluate(t) * spd;
    }
}
