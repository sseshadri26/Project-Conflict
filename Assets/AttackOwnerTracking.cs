using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOwnerTracking : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject owner;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if parent object is of layer "player", update it to be owner
        if (gameObject.transform.root.gameObject.layer == 7)
        {
            owner = gameObject.transform.root.gameObject;
        }
    }
}
