using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeEnemy : Enemy
{
    [SerializeField] protected GameObject attackObj;
    [SerializeField] protected SpriteRenderer attackRend;
    protected Color attackCol;
    
    protected void meleeEnemyStart()
    {
        EnemyStart();
    }

    protected void castAttack(int cooldownMin, int cooldownMax, int castDuration)
    {
        abilityCooldown = Random.Range(cooldownMin, cooldownMax);
        abilityCast = castDuration;
        attackObj.SetActive(true);
    }
    protected void setAttackRendColor(float value)
    {
        attackCol.a = value;
        attackRend.color = attackCol;
    }
}
