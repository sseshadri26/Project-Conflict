using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testManager : MonoBehaviour
{
    public Transform playerOneTrfm, playerTwoTrfm;
    public static Transform plyrOneTrfm, plyrTwoTrfm;

    void Update()
    {
        if (playerOneTrfm == null || playerOneTrfm.gameObject.name.Contains("Dummy"))
        {
            //find a "player 1" object in scene
            if (GameObject.Find("Player 1(Clone)"))
            {
                playerOneTrfm = GameObject.Find("Player 1(Clone)").transform;
            }
        }


        if (playerTwoTrfm == null || playerTwoTrfm.gameObject.name.Contains("Dummy"))
        {
            //find a "player 2" object in scene
            if (GameObject.Find("Player 2(Clone)"))
            {
                playerTwoTrfm = GameObject.Find("Player 2(Clone)").transform;
            }

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
