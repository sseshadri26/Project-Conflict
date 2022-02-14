using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testManager : MonoBehaviour
{
    public Transform playerOneTrfm, playerTwoTrfm;

    private void Start()
    {
        Enemy.assignPlayerTransforms(playerOneTrfm, playerTwoTrfm);
    }
}
