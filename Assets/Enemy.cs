using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private int health;
    [SerializeField]
    private float speed;

    int abilityTmr;

    public Transform playerOneTrfm;
    public Transform playerTwoTrfm;

    public GameObject lightingBolt;

    Rigidbody2D rb;
    Transform trfm;


    // Start is called before the first frame update
    void Start()
    {
        trfm = transform;
        rb = GetComponent<Rigidbody2D>();
    }





    void FixedUpdate()
    {
        
        if (Vector2.Distance(playerOneTrfm.position, trfm.position) < Vector2.Distance(playerTwoTrfm.position, trfm.position)) //if player one is closer to this enemy
        {
            //target player one   
            trfm.rotation = Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - playerOneTrfm.position.y, trfm.position.x - playerOneTrfm.position.x) * Mathf.Rad2Deg + 90, Vector3.forward);
        } else
        {
            //target player two
            trfm.rotation = Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - playerTwoTrfm.position.y, trfm.position.x - playerTwoTrfm.position.x) * Mathf.Rad2Deg + 90, Vector3.forward);
        }
        //trfm.position += trfm.up * speed;
        rb.velocity = trfm.up*speed;
        //trfm.position += trfm.right * .05f;




        if (abilityTmr > 0)
        {
            abilityTmr--;
        } else
        {
            //teleport the enemy forward
            //trfm.position += trfm.up * 5; //dont run 50x per sec

            //shoot
            //parameters: 1: object to create, 2: position to crate at, 3: rotation of the new object
            Instantiate(lightingBolt, trfm.position, trfm.rotation);

            //reset the ability timer (100 ticks = 2 secs)
            abilityTmr = 100;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //equivalent of cout
        Debug.Log(collision.name);

        //if u touch player, player loese 10 hp



        //
        //store ur current rotation
        //rotate to face collision
        //trfm.right * 1;
        //rotate back to ur old rotation
    }


}
