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

    void OnCollisionEnter(Collision collision) {
        Debug.Log("hit");
        rb.velocity = new Vector2(0,0);
    }
}
