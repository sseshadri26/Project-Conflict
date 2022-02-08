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

    Transform trfm;


    // Start is called before the first frame update
    void Start()
    {
        trfm = transform;
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
        trfm.position += trfm.up * speed;
        //trfm.position += trfm.right * .05f;



        if (abilityTmr > 0)
        {
            abilityTmr--;
        } else
        {
            //teleport the enemy forward
            trfm.position += trfm.up * 5; //dont run 50x per sec

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
