using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Rigidbody2D rb;

    private bool isBeingTurned;

    private Vector2 moveDirection;

    private Vector2 faceDirection;

    private Animator anim;

    private Vector2 animDirection;

    private Transform hands;

    [SerializeField]
    bool

            weaponEquipped,
            isP1;

    [SerializeField]
    weapon m_weapon;

    [SerializeField]
    Transform attachPoint;

    [SerializeField]
    Camera m_camera;

    Camera cam;

    VoronoiSplit vs;

    public bool IsP1()
    {
        return isP1;
    }

    public Transform GetAttachPoint()
    {
        return attachPoint;
    }

    public static Transform getPos(bool p1)
    {
        if (p1) return p1Trfm;
        return p2Trfm;
    }
    static Transform p1Trfm;
    static Transform p2Trfm;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        vs = cam.GetComponent<VoronoiSplit>();
        anim = GetComponent<Animator>();
        hands = this.gameObject.transform.GetChild(1);

        if (isP1) { p1Trfm = transform; }
        else { p2Trfm = transform; }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();
        Turn();
        Animate();





        if (doubleClick > 0)
        {
            doubleClick--;
        }
        
        if (spinAttacking > 0)
        {
            spinAttacking--;
        } else
        {
            if (throwQued)
            {
                ThrowFork();
                throwQued = false;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        if (!isBeingTurned) faceDirection = moveDirection;
    }

    bool modifierPressed;

    bool buttonDown = false;
    [SerializeField] bool throwQued;
    int doubleClick;
    int spinAttacking;
    public void OnFire(InputAction.CallbackContext context)
    {
        modifierPressed = context.performed;
        if (modifierPressed)
        {
            if (!buttonDown)
            {
                buttonDown = true;

                if (doubleClick > 0)
                {
                    throwQued = true;
                    doubleClick = 0;
                }
                else
                {
                    doSpinAttack();
                    doubleClick = 10;
                }
            }
        }
        else
        {
            buttonDown = false;
        }
    }
    public void ThrowFork()
    {
        //Debug.Log("calling throw");
        if (weaponEquipped)
        {
            //Debug.Log("weap eq");
            weaponEquipped = !m_weapon.Throw(isP1);
        }
    }
    public bool doSpinAttack()
    {
        if (spinAttacking < 1)
        {
            spinAttacking = 15;
            m_weapon.doSpinAttack();
            return true;
        }
        return false;
    }

    public void OnFaceDirection(InputAction.CallbackContext context)
    {
        /*Added by vikram*/
        PlayerInput playerInput = GetComponent<PlayerInput>();
        if (playerInput.currentControlScheme == "Gamepad")
        {
            faceDirection = context.ReadValue<Vector2>();
            if (faceDirection == Vector2.zero)
                isBeingTurned = false;
            else
                isBeingTurned = true;
        }
        else if (playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            //get a vector from the parent's origin to the mouse's
            Vector2 cursorPosition = context.ReadValue<Vector2>();
            Vector2 currentPosition;
            if (vs != null)
            {
                //array of vector2 holding vs.GetPlayerScreenPositions()
                Vector2[] playerScreenPositions = vs.GetPlayerScreenPositions();

                currentPosition =
                    playerScreenPositions[playerInput.playerIndex];
            }
            else
            {
                currentPosition = cam.WorldToScreenPoint(transform.position);
            }

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
        // only change the direction if the player moves so that the animation frame stays on the last input
        // (i.e. if you move right, the position will stay at [1, 0] and Player_East.anim will play
        // until there's a different directional input)
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            animDirection = new Vector2(moveDirection.x, moveDirection.y);
        }
    }

    //turn the player in the direction of the Vector2 faceDirection
    void Turn()
    {
        //this.transform.LookAt(faceDirection);
        /*Added by Vikram*/
        float angle;
        if (faceDirection.x != 0)
            angle = Mathf.Atan(faceDirection.y / faceDirection.x);
        else
            angle = 0;
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
        else
        //2nd or 4th quadrant
        {
            //2nd quadrant
            if (faceDirection.x < 0)
            {
                angle += 180;
            }
            else
            //4th quadrant
            {
                angle += 360;
            }
        }
        //this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // use this function to rotate the hands instead
        hands.localEulerAngles = new Vector3(0, 0, angle);
    }

    private void Animate()
    {
        anim.SetFloat("movementX", animDirection.x);
        anim.SetFloat("movementY", animDirection.y);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "weapon")
        {
            weaponEquipped = m_weapon.PickUp(this);
        }

        //Debug.Log($"touched {col.name}");
    }
}
