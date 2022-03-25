using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thunderboltBullet : MonoBehaviour
{
    public float spd;
    Transform trfm;



    private void Start()
    {
        Destroy(gameObject, 2);
        trfm = transform;
    }

    private void FixedUpdate()
    {
        trfm.position += trfm.up * spd;
    }

    void destoryObj()
    {
        //spawn in spark effectgs
        //play spark sound;
        //shake the screen
    }
}
