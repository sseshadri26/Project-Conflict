using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class blobScript : MonoBehaviour
{
    public Tilemap hazardTilemap;

    [SerializeField]
    TileBase SpawnedTile;

    //vector3 named finalPos
    Vector3 finalPos;
    Vector3 startPos;

    //make 3 states: moving up, moving down, finalpos. state machine!
    //state machine:
    //0 = moving up
    //1 = moving down
    //2 = finalpos
    int state = 0;
    float distance = 10;

    // Start is called before the first frame update
    void Start()
    {
        //first, find a final position
        //then, rise
        //then, fall
        //then instantiate a splat
        startPos = transform.position;
        distance = Vector3.Distance(startPos, finalPos);

        hazardTilemap = GameObject.Find("tilemap (trigger hazards)").GetComponent<Tilemap>();


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (state == 0)
        {
            //move towards finalPos
            //if at halfway point , change state to 1
            transform.position = Vector3.MoveTowards(transform.position, finalPos, Time.deltaTime * 2);
            //find distance between start and final
            if (Vector3.Distance(transform.position, finalPos) < distance / 2)
            {
                state = 1;
            }

            //make sprite a little bigger every frame
            transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
        }
        else if (state == 1)
        {
            //move towards finalPos
            //if at finalPos, change state to 2
            transform.position = Vector3.MoveTowards(transform.position, finalPos, Time.deltaTime * 2);
            if (transform.position == finalPos)
            {
                state = 2;
            }
            transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);

        }
        else if (state == 2)
        {
            //instantiate a splat
            hazardTilemap.SetTile(Vector3Int.FloorToInt(this.transform.position), SpawnedTile);
            Destroy(this.gameObject);
        }
    }

    public void setParams(Tilemap hazardTilemap, Vector3 finalPos)
    {
        //this.hazardTilemap = hazardTilemap;
        this.finalPos = finalPos;
        if (!hazardTilemap)
        {
            //Debug.Log("hazardTilemap is null");
            //find a tilemap named "tilemap (trigger hazards)"
            //hazardTilemap = GameObject.Find("tilemap (trigger hazards)").GetComponent<Tilemap>();
        }
    }
}
