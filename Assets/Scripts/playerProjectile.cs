using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectile : MonoBehaviour
{
    //wait why r we making this script, i already made weapon.cs which does what this does i think  -kaiway

    [SerializeField] float spd;
    [SerializeField] int state;
    const int embedded = 0, inFlight = 1, p1Equipped = 2, p2Equipped = 3;
    Transform trfm;

    private void Start()
    {
        trfm = transform;
    }
    private void FixedUpdate()
    {
        if (state == inFlight)
        {
            trfm.position += trfm.up * spd;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 6)
        {
            if (state == inFlight)
            {
                state = embedded;
            }
        } else
        if (col.gameObject.tag == "player")
        {
            if (state < 2)
            {
                PlayerControls plyrScr = col.GetComponent<PlayerControls>();
                if (plyrScr.IsP1())
                {
                    state = p1Equipped;
                } else
                {

                }
            }
        }
    }
}
