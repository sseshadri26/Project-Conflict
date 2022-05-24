using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEventIntermediary : MonoBehaviour
{
    // An intermediary function so that Animation Events can call castAttack in enemy parent object
    // This one is for the sausage
    public void castAttack() {
        this.transform.parent.gameObject.GetComponent<meleeEnemy>().castAttack(100, 200, 10);
    }
}
