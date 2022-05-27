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
    private GameObject[] playerPrefabs;

    // Start is called before the first frame update


    void Awake()
    {
        //destroy any existing players
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //foreach (GameObject player in players)
        //{
        //    Destroy(player);
        //}
        Destroy(GameObject.Find("Player 1(Clone)"));
        Destroy(GameObject.Find("Player 2(Clone)"));
        var playerConfigurations =
            PlayerConfigurationManager
                .Instance
                .GetPlayerConfigurations()
                .ToArray();
        for (int i = 0; i < 2; i++)
        {
            var player =
                Instantiate(playerPrefabs[i],
                PlayerSpawns[i].position,
                PlayerSpawns[i].rotation,
                gameObject.transform);
            player
                .GetComponent<PlayerInputHandler>()
                .InitializePlayer(playerConfigurations[i]);
            //player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");

        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
