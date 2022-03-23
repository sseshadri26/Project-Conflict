using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testManager : MonoBehaviour
{
    public Transform playerOneTrfm, playerTwoTrfm;
    public static Transform plyrOneTrfm, plyrTwoTrfm;

    private void Awake()
    {
        plyrOneTrfm = playerOneTrfm; plyrTwoTrfm = playerTwoTrfm;
        Enemy.assignPlayerTransforms(playerOneTrfm, playerTwoTrfm);
    }
}
