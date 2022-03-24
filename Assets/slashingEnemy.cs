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
        if (abilityCooldown > 0)
        {
            abilityCooldown--;
        }
        else
        {
            castAttack(100, 200, 10);
            setAttackRendColor(1);
        }
    }
}
