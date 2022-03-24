using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform[] Players;

    public float Speed = 2.5f;

    private Rigidbody2D[] playerRigidbody;

    private int activePlayerIndex = 0;

    private int lastActivePlayerIndex = 0;

    private void Awake()
    {
        playerRigidbody = new Rigidbody2D[Players.Length];

        for (int i = 0; i < Players.Length; i++)
        {
            playerRigidbody[i] = Players[i].GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
    }
}
