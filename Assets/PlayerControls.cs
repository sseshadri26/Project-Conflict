using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Rigidbody2D rb;

    private Rigidbody2D rb2;

    private Vector2 moveDirection;

    private Vector2 faceDirection;

    //Vikram
    GameObject parent;

    [SerializeField] bool weaponEquipped, isP1;
    [SerializeField] weapon m_weapon;
    [SerializeField] Transform attachPoint;
    [SerializeField] Camera m_camera;

    public bool IsP1() { return isP1; }
    public Transform GetAttachPoint() { return attachPoint; }

    private void Awake()
    {
        parent = GameObject.Find("ParentForRotation");
        rb = GetComponent<Rigidbody2D>();
        rb2 = parent.GetComponent<Rigidbody2D>(); 
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
        /*Added by vikram*/
        PlayerInput playerInput = GetComponent<PlayerInput>();
        if (playerInput.currentControlScheme == "Gamepad")
        {
            faceDirection = context.ReadValue<Vector2>();
        }
        else if (playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            //get a vector from the parent's origin to the mouse's 
            Camera cam = Camera.main;
            Vector2 tempVector = context.ReadValue<Vector2>();
            Vector3 cursorPosition = cam.ScreenToWorldPoint(new Vector3(tempVector.x, tempVector.y, 1));
            Vector3 currentPosition = parent.transform.position;
            faceDirection = cursorPosition - currentPosition;
        }

        //print("test");
        //print(faceDirection);
        
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
        rb2.velocity = rb.velocity;
    }

    //turn the player in the direction of the Vector2 faceDirection
    void Turn()
    {
        //this.transform.LookAt(faceDirection);

        /*Added by Vikram*/
       
        float angle;
        if (faceDirection.x != 0)
            angle = Mathf.Atan(faceDirection.y / faceDirection.x);
        else angle = 0;
        angle *= Mathf.Rad2Deg;
        //1st or 3rd quadrant
        if (angle > 0)
        {
            //3rd quadrant
            if (faceDirection.x < 0)
            {
                angle += 180;
            }
        }
        //2nd or 4th quadrant
        else
        {
            //2nd quadrant
            if (faceDirection.x < 0)
            {
                angle += 180;
            }
            //4th quadrant
            else
            {
                angle += 360;
            }
        }
        parent.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "weapon")
        {
            weaponEquipped = m_weapon.PickUp(this);
        }

        Debug.Log($"touched {col.name}");
    }
}
