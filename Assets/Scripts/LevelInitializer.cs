using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField]
    private Transform[] PlayerSpawns;

    [SerializeField]
    private GameObject playerPrefab;

    private GameObject[] players = new GameObject[2];

    bool isEmpty = true;

    // Start is called before the first frame update
    void Start()
    {
        var playerConfigurations =
            PlayerConfigurationManager
                .Instance
                .GetPlayerConfigurations()
                .ToArray();
        for (int i = 0; i < playerConfigurations.Length; i++)
        {
            isEmpty = false;
            players[i] =
                Instantiate(playerPrefab,
                PlayerSpawns[i].position,
                PlayerSpawns[i].rotation,
                gameObject.transform);

            players[i].SetActive(true);

            //get player input handler
            PlayerInputHandler playerInputHandler =
                players[i].GetComponent<PlayerInputHandler>();

            //initialize player
            playerInputHandler.InitializePlayer(playerConfigurations[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerSpawns[0].transform.position = players[0].transform.position;
        PlayerSpawns[1].transform.position = players[1].transform.position;
    }
}
