using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{


    public bool Throw(bool p1)
    {
        if (inFlight() || m_state == state.embedded || inWall) return false;

        trfm.parent = null;
        //move back 1 units
        trfm.position = trfm.position + trfm.up * -1.5f;
        //gameObject.transform.

        if (p1) { m_state = state.threwP1; } else { m_state = state.threwP2; }
        throwBlurBack.SetActive(true);
        throwBlurFront.SetActive(true);
        return true;
    }

    public bool Drop()
    {
        trfm.parent = null;
        boxCol.enabled = true;
        m_state = state.embedded;
        return false;
    }

    public bool PickUp(PlayerControls m_Player)
    {
        if (inHand()) return false;

        if (m_Player.IsP1()) { m_state = state.attachedP1; }
        else { m_state = state.attachedP2; }

        trfm.parent = m_Player.GetAttachPoint();
        trfm.position = trfm.parent.position;
        trfm.rotation = trfm.parent.rotation;
        exitFlight();
        boxCol.enabled = false;

        return true;
    }

    int spinAttacking, thrustAttacking;
    public bool doThrustAttack()
    {
        if (inHand())
        {
            thrustObj.SetActive(true);
            thrustAttacking = 12;
            return true;
        }
        return false;
    }
    public bool doSpinAttack()
    {
        if (inHand())
        {
            spinObj.SetActive(true);
            spinAttacking = 15;
            return true;
        }
        return false;
    }

    enum state { attachedP1, attachedP2, threwP1, threwP2, embedded };
    state m_state = state.embedded;
    [SerializeField] GameObject spinObj, thrustObj, throwBlurFront, throwBlurBack;
    public Transform trfm;
    [SerializeField] float spd;
    [SerializeField] BoxCollider2D boxCol;
    [SerializeField] BoxCollider2D wallCol;

    [SerializeField] SpriteRenderer thrustRend;
    [SerializeField] Color thrustCol;
    int pickUpDelay;
    public bool lastOwnerWasP1;

    public bool inWall = false;
    //getter functions to determine which player threw the weapon last


    private void FixedUpdate()
    {

        //if (wallCol.IsTouchingLayers())
        //{
        //    inWall = true;
        //}
        //else
        //{
        //    inWall = false;
        //}

        if (m_state == state.threwP1)
        {
            lastOwnerWasP1 = true;
        }
        else if (m_state == state.threwP2)
        {
            lastOwnerWasP1 = false;
        }

        if (inFlight())
        {
            trfm.position += trfm.up * spd;
        }
        if (spinAttacking > 0)
        {
            spinAttacking--;
            trfm.Rotate(Vector3.forward * -24);
            if (spinAttacking == 0) spinObj.SetActive(false);
        }
        if (thrustAttacking > 0)
        {
            thrustAttacking--;
            if (thrustAttacking > 5)
            {
                trfm.position += trfm.up * .15f;
            }
            else
            {
                trfm.position += trfm.up * -.15f;
                thrustObj.transform.position += trfm.up * .3f;
                if (thrustAttacking < 6)
                {
                    thrustRend.color -= thrustCol;
                }
                if (thrustAttacking == 0)
                {
                    thrustObj.transform.position = trfm.position;
                    thrustObj.SetActive(false);
                    thrustRend.color = new Color(1, 1, 1, 1);
                }
            }
        }
    }

    bool inFlight()
    {
        return m_state == state.threwP1 || m_state == state.threwP2;
    }

    void embed()
    {
        m_state = state.embedded;
        boxCol.enabled = true;
        exitFlight();
    }

    void exitFlight()
    {
        throwBlurBack.SetActive(false);
        throwBlurFront.SetActive(false);
    }

    bool inHand()
    {
        return m_state == state.attachedP1 || m_state == state.attachedP2;
    }

    public void Rotate(Vector2 direction)
    {
        if (spinAttacking == 0)
        {
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            trfm.localEulerAngles = new Vector3(0, 0, -90);
        }
    }

    public int heldBy()
    {
        if (m_state == state.attachedP1) return 1;
        if (m_state == state.attachedP2) return 2;
        return 0;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //is state is being held, return


        if (col.gameObject.layer == 6 && !inHand())
        {
            // layer 6 = walls
            //trfm.position += trfm.up * -0.8f;
            embed();
        }
        else if (col.gameObject.layer == 7 && inFlight())
        {
            // layer 7 = player; this mistakenly triggers for the player that threw the fork too
            // also this doesn't address weaponEquipped in PlayerControls.cs
            Debug.Log("hit another player");
            Debug.Log("Threw p1: " + (m_state == state.threwP1) + " Threw p2: " + (m_state == state.threwP2) + "Name: " + col.gameObject.name);
            if (m_state == state.threwP1 && col.gameObject.name.Contains("2"))
            {
                embed();
            }
            else if (m_state == state.threwP2 && col.gameObject.name.Contains("1"))
            {
                embed();
            }
            //PickUp(col.gameObject.GetComponent<PlayerControls>());
        }
    }

}
