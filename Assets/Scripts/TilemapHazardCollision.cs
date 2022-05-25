using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class TilemapHazardCollision : MonoBehaviour
{
    [SerializeField] Tilemap hazardMap;
    public PlayerControls pc;

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("collided with something");
        var grid = hazardMap.layoutGrid;

        // get position of the point of contact with the collision
        var contact = col.GetContact(0);
        Vector3Int gridPoint = grid.WorldToCell(contact.point);
        
        // gets the specific tile at gridPoint and returns if it doesn't exist
        var tile = hazardMap.GetTile(gridPoint);
        if (tile == null)
            return;

        // dictionary invoke isn't working so for now just use the tile names to choose the specific downgrade functions
        // use GameObject.Find to invoke the hazard on the specific player that triggered it
        if (tile.name == "hazard_ketchup")
        {
            Debug.Log("ketchup");
            pc = GameObject.Find(col.gameObject.name).GetComponent<PlayerControls>();
            pc.SetStun();
        } else if (tile.name == "hazard_mustard")
        {
            Debug.Log("mustard");
            pc = GameObject.Find(col.gameObject.name).GetComponent<PlayerControls>();
            pc.SetSlowdown();
        }

        // delete specific tile
        hazardMap.SetTile(gridPoint, null);
    }

}
