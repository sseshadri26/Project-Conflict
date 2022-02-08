using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private int health;
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
            trfm.position += trfm.up * 5; //dont run 50x per sec

            abilityTmr = 100;
        }
    }


    
    
}
