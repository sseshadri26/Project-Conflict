using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KtAttackEventIntermediary : MonoBehaviour
{
    // An intermediary function so that Animation Events can call castAttack in enemy parent object
    // This one is for the ketchup/mustard
    public void castAttackProjectile() {
        this.transform.parent.gameObject.GetComponent<rangedEnemy>().spawnProjectile();
    }

    public void castAttackHazard() {
        this.transform.parent.gameObject.GetComponent<rangedEnemy>().spawnHazard();
    }

    public void resetAbilityCooldown() {
        this.transform.parent.gameObject.GetComponent<rangedEnemy>().resetAbilityCooldown();
    }
}
