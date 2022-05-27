using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slashingEnemy : meleeEnemy
{

    private void Start()
    {
        meleeEnemyStart();
        attackCol = new Color(1, .45f, .45f, 1);
        abilityCooldown = Random.Range(100, 200);
    }

    private void FixedUpdate()
    {
        EnemyFixedUpdate();
        if (stunTmr < 1 && hasTargetPlayer)
        {
            moveToPlayer(speed);
            doAbilityCycle();
        }
        doCastAnimation();
    }

    void doCastAnimation()
    {
        if (abilityCast > 0)
        {
            setAttackRendColor(abilityCast / 10f);
            abilityCast--;
            if (abilityCast == 0)
            {
                attackObj.SetActive(false);
            }
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
            // Attack functions are called through animation events in front facing attack animation
            animator.SetTrigger("isAttacking");
            // takeDamage(100);
            // temporarily increase ability cooldown to prevent animator from triggering twice
            abilityCooldown = 1000;
        }
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    TriggerEnter(col);
    //}
}
