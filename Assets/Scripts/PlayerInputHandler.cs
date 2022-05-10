using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerConfiguration playerConfig;

    private PlayerControls playerControls;

    private ProjectConflictControls controls;

    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        controls = new ProjectConflictControls();
    }

    public void InitializePlayer(PlayerConfiguration config)
    {
        playerConfig = config;
        config.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(InputAction.CallbackContext context)
    {
        // Debug.Log (controls);
        // print the action that was triggered
        // Debug.Log(context.action.name + " was triggered");
        // Debug.Log (controls);
        if (context.action.name == controls.Player.Move.name)
        {
            if (playerControls != null)
            {
                playerControls.OnMove (context);
            }
        }
    }
}
