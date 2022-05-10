using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUpright : MonoBehaviour
{

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.up = Vector3.up;
    }
}
