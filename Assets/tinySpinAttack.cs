using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tinySpinAttack : MonoBehaviour
{
    int tmr;
    [SerializeField] Transform trfm;
    public Transform plyrTrfm;
    Vector3 offset = new Vector3(0,0.55f,0);

    private void Update()
    {
        trfm.position = plyrTrfm.position + offset;
    }

    void FixedUpdate()
    {
        tmr++;
        if (tmr > 12) Destroy(gameObject);
        trfm.Rotate(trfm.forward*-30);
    }
}
