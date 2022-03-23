using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    Transform playerOneTrfm, playerTwoTrfm;
    [SerializeField] Transform cameraTrfm;
    Vector3 targetPos;

    void Start()
    {
        playerOneTrfm = testManager.plyrOneTrfm; playerTwoTrfm = testManager.plyrTwoTrfm;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetPos.x = (playerOneTrfm.position.x + playerTwoTrfm.position.x)/2;
        targetPos.y = (playerOneTrfm.position.y + playerTwoTrfm.position.y)/2;
        targetPos = (targetPos - cameraTrfm.position)/10;
        targetPos.z = 0;
        cameraTrfm.position += targetPos;
    }
}
