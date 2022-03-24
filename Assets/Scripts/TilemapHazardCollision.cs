using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class TilemapHazardCollision : MonoBehaviour
{
    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision2D> { }

    [System.Serializable]
    public struct HazardEffect
    {
        public TileBase tile;
        public CollisionEvent effect;
    }

    public TileBase[] tiles;
    public float movementSpeed, damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
