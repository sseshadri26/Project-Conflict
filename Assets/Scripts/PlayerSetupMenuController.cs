using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int playerIndex;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private GameObject readyPanel;

    [SerializeField]
    private Button readyButton;

    [SerializeField]
    private Button disconnectButton;

    private float ignoreInputTime = 1.5f;

    private bool inputEnabled;

    public void setPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        readyPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    // public void SelectColor(Material mat)
    // {
    //     if (!inputEnabled) { return; }
    //     PlayerConfigurationManager.Instance.SetPlayerColor(playerIndex, mat);
    //     readyPanel.SetActive(true);
    //     readyButton.interactable = true;
    //     menuPanel.SetActive(false);
    //     readyButton.Select();
    // }
    // public void DisconnectPlayer()
    // {
    //     if (!inputEnabled)
    //     {
    //         return;
    //     }
    //     PlayerConfigurationManager.Instance.DisconnectPlayer (playerIndex);
    //     readyButton.gameObject.SetActive(false);
    // }
    public void ReadyPlayer()
    {
        if (!inputEnabled)
        {
            return;
        }

        PlayerConfigurationManager.Instance.ReadyPlayer (playerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
