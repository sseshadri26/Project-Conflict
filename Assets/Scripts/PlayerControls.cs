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

    [SerializeField] private Animator anim;

    private float angleIdle;

    // index 0 - idle hand; index 1 = weapon hand
    [SerializeField] private Transform[] hands;

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

    [SerializeField] Transform hand;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        vs = cam.GetComponent<VoronoiSplit>();
        //anim = GetComponent<Animator>();
        //hands = this.gameObject.transform.GetChild(1);

        if (isP1) { p1Trfm = transform; }
        else { p2Trfm = transform; }
        angleIdle = -90;
        /*
        hands = new Transform[2];
        hands[0] = this.gameObject.transform.GetChild(1); // child objects are offset by 1 due to attachpoint
        hands[1] = this.gameObject.transform.GetChild(2);
        */ 
        //sorry commented our your code trying to get stuff to compile :|   -kaiway
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();
        // only change the direction if the player moves;
        // faceDirection is for the 360-degree weapon hand movement
        // moveDirection is for 8-directional movement with animations and the idle hand
        if (faceDirection.x != 0 || faceDirection.y != 0)
        {
            Turn();
        }
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
    }

    //turn the player in the direction of the Vector2 faceDirection
    void Turn()
    {
        //this.transform.LookAt(faceDirection);
        /*Added by Vikram*/
        float angle;

        // calculates arctangent to get the angle between [-180, 180] instead of just the 1st and 4th quadrants
        // Note: This function takes account of the cases where x is zero and
        // returns the correct angle rather than throwing a division by zero exception. (re: unity doc)
        angle = Mathf.Atan2(faceDirection.y, faceDirection.x) * Mathf.Rad2Deg;

        //this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // use this function to rotate the weapon hand instead
        //hands[1].localEulerAngles = new Vector3(0, 0, angle);
    }

    private void Animate()
    {
        bool movement = false;
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            // movementX/Y are the parameters used to pick the precise sprite in
            // the blend tree inside the Animator window
            anim.SetFloat("movementX", moveDirection.x);
            anim.SetFloat("movementY", moveDirection.y);

            // logic for turning the idle hand by the player
            angleIdle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            // offset so that the hand is next to the player instead of in front
            angleIdle += 90;

            //hands[0].localEulerAngles = new Vector3(0, 0, angleIdle);
            movement = true;
        }
        else
        {
            anim.SetFloat("angle", angleIdle - 90);
        }
        anim.SetBool("isMoving", movement);
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
