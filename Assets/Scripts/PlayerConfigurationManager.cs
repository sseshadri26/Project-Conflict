using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigurations;

    private int MaxPlayers = 2;

    //singleton pattern
    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        //make sure only one instance of this class exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad (Instance);
            playerConfigurations = new List<PlayerConfiguration>();
        }
        else
        {
            Debug
                .LogError("Singleton - Trying to create a second instance of PlayerConfigurationManager");
            Destroy (gameObject);
        }
    }

    // SetPlayerColor takes in a player index and a color and sets the player's color to the color
    public void SetPlayerColor(int playerIndex, Material color)
    {
        playerConfigurations[playerIndex].PlayerMaterial = color;
    }

    // ReadyPlayer takes in a player index and sets the player's ready status to true
    public void ReadyPlayer(int playerIndex)
    {
        playerConfigurations[playerIndex].isReady = true;

        //if all player have joined and all are ready, start the game
        if (
            playerConfigurations.Count == MaxPlayers &&
            playerConfigurations.All(p => p.isReady)
        )
        {
            // GameManager.Instance.StartGame();
            //Load Local Co-Op Scene
            SceneManager.LoadScene("Local Co-Op");
        }
    }

    //HandlePlayerJoin takes in playerinput pi. adds pi to the list of players
    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player " + pi.playerIndex + " joined");

        //check that we havent already added this player
        if (!playerConfigurations.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            //the player is now a child of this object
            pi.transform.SetParent (transform);
            playerConfigurations.Add(new PlayerConfiguration(pi));
        }
        else
        {
            Debug.LogError("Player " + pi.playerIndex + " already joined");
            return;
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigurations;
    }
}

public class PlayerConfiguration
{
    //constructor that takes in Player input pi
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }

    public PlayerInput Input { get; set; }

    public int PlayerIndex { get; set; }

    public bool isReady { get; set; }

    public Material PlayerMaterial { get; set; }
}
