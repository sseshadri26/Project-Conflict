using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class self_dest : MonoBehaviour
{
    [SerializeField] int life;
    private void FixedUpdate()
    {
        if (life > 0) { life--; } else
        {
            Destroy(gameObject);
        }
    }
}
