using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public bool Throw()
    {
        if (m_state == state.inFlight || m_state == state.embedded) return false;

        m_state = state.inFlight;
        trfm.parent = null;
        pickUpDelay = 5;
        return true;
    }
    public bool PickUp(PlayerControls m_Player)
    {
        if (m_state == state.attachedP1 || m_state == state.attachedP2 || pickUpDelay>0) return false;

        if (m_Player.IsP1()) { m_state = state.attachedP1; }
        else { m_state = state.attachedP2; }

        trfm.parent = m_Player.GetAttachPoint();
        trfm.position = trfm.parent.position;
        trfm.rotation = trfm.parent.rotation;

        return true;
    }

    enum state { attachedP1, attachedP2, inFlight, embedded };
    state m_state = state.embedded;
    [SerializeField] Transform trfm;
    [SerializeField] float spd;
    int pickUpDelay;
    private void FixedUpdate()
    {
        if (m_state == state.inFlight)
        {
            trfm.position += trfm.up * spd;
        }
        if (pickUpDelay > 0)
        {
            pickUpDelay--;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 6)
        {
            m_state = state.embedded;
        }
    }
}
