using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSortingLayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }

    void SetPosition() {
        Vector3 newPosition = transform.position;
        newPosition.z = transform.position.y;
        transform.position = newPosition;
    }
}
