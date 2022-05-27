using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testManager : MonoBehaviour
{
    public Transform playerOneTrfm, playerTwoTrfm;
    public static Transform plyrOneTrfm, plyrTwoTrfm;

    void Update()
    {
        if (playerOneTrfm == null)
        {
            //find a "player 1" object in scene
            playerOneTrfm = GameObject.Find("Player 1").transform;
        }


        if (playerTwoTrfm == null)
        {
            playerTwoTrfm = GameObject.Find("Player 2").transform;
        }

        if (playerTwoTrfm != null && playerOneTrfm != null)
        {
            plyrOneTrfm = playerOneTrfm; plyrTwoTrfm = playerTwoTrfm;
            Enemy.assignPlayerTransforms(playerOneTrfm, playerTwoTrfm);
        }

        gameObject.GetComponent<VoronoiSplit>().Players[0] = playerOneTrfm;
        gameObject.GetComponent<VoronoiSplit>().Players[1] = playerTwoTrfm;

    }
}
