using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerConfiguration playerConfig;

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

    private void Input_onActionTriggered(CallbackContext obj)
    {
        //print the action name
        //Debug.Log(obj.action.name);

        if (obj.action.name == controls.Player.Move.name)
        {
            OnMove(obj);
        }

        if (obj.action.name == controls.Player.Look.name)
        {
            OnLook(obj);
        }

        if (obj.action.name == controls.Player.Fire.name)
        {
            OnAttack(obj);
        }

        if (obj.action.name == controls.Player.Fire2.name)
        {
            OnAttack2(obj);
        }
    }

    public void OnMove(CallbackContext context)
    {
        if (playerControls != null) playerControls.OnMove(context);
    }


    public void OnLook(CallbackContext context)
    {
        if (playerControls != null) playerControls.OnFaceDirection(context);
    }

    public void OnAttack(CallbackContext context)
    {
        if (playerControls != null) playerControls.OnFire(context);
    }

    public void OnAttack2(CallbackContext context)
    {
        if (playerControls != null) playerControls.OnFire2(context);
    }
}
