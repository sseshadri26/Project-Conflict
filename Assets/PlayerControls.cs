using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Rigidbody2D rb;

    private Vector2 moveDirection;

    private Vector2 faceDirection;

    [SerializeField] bool weaponEquipped, isP1;
    [SerializeField] weapon m_weapon;
    [SerializeField] Transform attachPoint;
    [SerializeField] Camera m_camera;

    public bool IsP1() { return isP1; }
    public Transform GetAttachPoint() { return attachPoint; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    bool modifierPressed;
    bool buttonDown = false;
    public void OnFire(InputAction.CallbackContext context)
    {
        modifierPressed = context.performed;
        if (modifierPressed)
        {
            if (!buttonDown)
            {
                buttonDown = true;

                if (weaponEquipped)
                {
                    weaponEquipped = !m_weapon.Throw();
                }
            }
        } else
        {
            buttonDown = false;
        }
    }

    public void OnFaceDirection(InputAction.CallbackContext context)
    {
        faceDirection = context.ReadValue<Vector2>();
    }

    // void ProcessInputs()
    // {
    //     float moveX = Input.GetAxisRaw("Horizontal");
    //     float moveY = Input.GetAxisRaw("Vertical");
    //     moveDirection = new Vector2(moveX, moveY).normalized;
    // }
    void Move()
    {
        rb.velocity =
            new Vector2(moveDirection.x * moveSpeed,
                moveDirection.y * moveSpeed);
    }

    //turn the player in the direction of the Vector2 faceDirection
    void Turn()
    {
        // this.transform.LookAt(faceDirection);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "weapon")
        {
            weaponEquipped = m_weapon.PickUp(this);
        }
    }
}
