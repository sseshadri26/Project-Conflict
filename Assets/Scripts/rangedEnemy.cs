using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class rangedEnemy : Enemy
{

    [SerializeField] GameObject projectile;
    [SerializeField] GameObject hazardBlob;

    [SerializeField] Tilemap hazardTilemap;
    [SerializeField] TileBase spawnedTile;
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
            moveToPlayer(speed);
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
            if (Random.Range(1, 4) == 1)
            {
                // Attack functions are called through animation events (front facing)
                animator.SetTrigger("isAttackingHazard");
                // Increase ability cooldown so the animation trigger only occurs once
                abilityCooldown = 100000;
            }
            else
            {
                // Attack functions are called through animation events (front facing)
                animator.SetTrigger("isAttacking");
                // Increase ability cooldown so the animation trigger only occurs once
                abilityCooldown = 1000;
            }
        }
    }

    // All functions are called through animation event (front facing)
    public void spawnProjectile()
    {
        Instantiate(projectile, trfm.position, trfm.rotation);
    }
    public void spawnHazard()
    {
        Instantiate(hazardBlob, trfm.position + Vector3.up / 2, Quaternion.identity).GetComponent<blobScript>().setParams(hazardTilemap, path.vectorPath[path.vectorPath.Count / 2]);

        //hazardTilemap.SetTile(Vector3Int.FloorToInt(trfm.position), spawnedTile);
    }
    // Called at the end of the attack animation
    public void resetAbilityCooldown()
    {
        abilityCooldown = Random.Range(100, 200);
    }
}
