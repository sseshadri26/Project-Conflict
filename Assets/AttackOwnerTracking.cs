using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOwnerTracking : MonoBehaviour
{
    // Start is called before the first frame update

    public bool owner;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if parent object is of layer "player", update it to be owner
        if (GetComponent<weapon>())
        {
            bool ownerIsP1 = GetComponent<weapon>().lastOwnerWasP1;


        }
    }
}
