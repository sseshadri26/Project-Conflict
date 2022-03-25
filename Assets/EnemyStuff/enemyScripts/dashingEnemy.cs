using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashingEnemy : meleeEnemy
{

    private void Start()
    {
        meleeEnemyStart();
        abilityCooldown = Random.Range(200, 400);
    }
    private void FixedUpdate()
    {
        EnemyFixedUpdate();
        if (stunTmr < 1 && hasTargetPlayer)
        {
            moveToPlayer(speed);
            doAbilityCycle();
        }
        doAbilityCast();
    }

    void doAbilityCast()
    {
        if (abilityCast > 0)
        {
            moveToPlayer(speed * 5);
            abilityCast--;
            if (abilityCast == 0) { attackObj.SetActive(false); }
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
            castAttack(200, 400, 15);
        }
    }
}
