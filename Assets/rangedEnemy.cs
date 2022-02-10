using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedEnemy : Enemy
{

    [SerializeField] GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        EnemyStart();
        abilityCooldown = Random.Range(100, 200);
    }

    private void FixedUpdate()
    {
        EnemyFixedUpdate();
        if (stunTmr < 1 && hasTargetPlayer)
        {
            doAbilityCycle();
            moveForward(speed);
        }
    }

    void doAbilityCycle()
    {
        if (abilityCooldown > 0)
        {
            abilityCooldown--;
        } else
        {
            Instantiate(projectile, trfm.position, trfm.rotation);
            abilityCooldown = Random.Range(100, 200);
        }
    }
}
