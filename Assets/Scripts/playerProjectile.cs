using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectile : MonoBehaviour
{
    Transform trfm;
    private void Start()
    {
        trfm = transform;
    }
    private void FixedUpdate()
    {
        trfm.position += trfm.right * 3;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            
        }
    }
}
