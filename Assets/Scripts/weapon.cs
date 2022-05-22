using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public bool Throw(bool p1)
    {
        if (inFlight() || m_state == state.embedded) return false;

        trfm.parent = null;
        if (p1) { m_state = state.threwP1; trfm.LookAt(PlayerControls.getPos(true)); } else { m_state = state.threwP2; trfm.LookAt(PlayerControls.getPos(false)); }
        throwBlurBack.SetActive(true);
        throwBlurFront.SetActive(true);
        return true;
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

        return true;
    }

    int spinAttacking;
    public bool doSpinAttack()
    {
        if (inHand())
        {
            spinAttack.SetActive(true);
            spinAttacking = 15;
            return true;
        }
        return false;
    }

    enum state { attachedP1, attachedP2, threwP1, threwP2, embedded };
    state m_state = state.embedded;
    [SerializeField] GameObject spinAttack, throwBlurFront, throwBlurBack;
    [SerializeField] Transform trfm;
    [SerializeField] float spd;
    int pickUpDelay;
    private void FixedUpdate()
    {
        if (inFlight())
        {
            trfm.position += trfm.up * spd;
        }
        if (spinAttacking > 0)
        {
            spinAttacking--;
            trfm.Rotate(Vector3.forward * -24);
            if (spinAttacking == 0) spinAttack.SetActive(false);
        }
    }

    bool inFlight()
    {
        return m_state == state.threwP1 || m_state == state.threwP2;
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 6 && !inHand())
        {
            m_state = state.embedded;
            exitFlight();
        }
    }
}
