using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float moveDebuf;

    private Rigidbody2D rb;

    private bool isBeingTurned;

    private Vector2 moveDirection;

    private Vector2 faceDirection;

    [SerializeField] private Animator anim;

    private float angleIdle;

    private float timerStart, timerEnd;

    private bool isStunned;

    private float stunTimer;

    // index 0 - idle hand; index 1 = weapon hand
    [SerializeField]
    Transform[] hands;

    [SerializeField]
    SpriteRenderer sprite;

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
        timerEnd = 3.0f;
        stunTimer = 3.0f;
        //anim = GetComponent<Animator>();
        //hands = this.gameObject.transform.GetChild(1);

        if (isP1) { p1Trfm = transform; }
        else { p2Trfm = transform; }
        angleIdle = -90;


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

        timerEnd += Time.deltaTime;


        if (meleeCD > 0) meleeCD--;

        if (doubleClick > 0) doubleClick--;

        if (thrustAttacking > 0) thrustAttacking--;

        if (spinAttacking > 0) spinAttacking--;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        if (!isBeingTurned) faceDirection = moveDirection;
    }

    bool modifierPressed;

    bool buttonDown = false;
    [SerializeField] bool throwQued;
    [SerializeField] GameObject thrustObj, tinySpinObj, spinObj;
    int doubleClick;
    int spinAttacking, thrustAttacking, meleeCD;
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
                    if (weaponEquipped) { doThrustAttack(); }
                    else { doTinySpinAttack(); }
                    doubleClick = 10;
                }
            }
        }
        else
        {
            buttonDown = false;
        }
    }

    public void OnFire2(InputAction.CallbackContext context)
    {
        if (context.started)
            ThrowFork();
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
    public bool doThrustAttack()
    {
        if (meleeCD < 1 && thrustAttacking < 1 && spinAttacking < 1)
        {
            thrustAttacking = 12;
            m_weapon.doThrustAttack();
            //Debug.Log(isP1);
            Instantiate(thrustObj, m_weapon.trfm.position, m_weapon.trfm.rotation).GetComponent<playerMeleeObj>().heldByP1 = isP1;
            meleeCD = 50;
            return true;
        }
        return false;
    }
    public bool doSpinAttack()
    {
        if (meleeCD < 1 && thrustAttacking < 1 && spinAttacking < 1 && weaponEquipped)
        {
            spinAttacking = 15;
            m_weapon.doSpinAttack();
            Instantiate(spinObj, getPos(isP1).position, Quaternion.identity).GetComponent<playerMeleeObj>().heldByP1 = isP1;
            meleeCD = 50;
            return true;
        }
        return false;
    }

    public bool doTinySpinAttack()
    {
        if (meleeCD < 1 && thrustAttacking < 1 && spinAttacking < 1)
        {
            spinAttacking = 12;
            GameObject tinySpinObjectNew = Instantiate(tinySpinObj, getPos(isP1).position, Quaternion.identity);
            tinySpinObjectNew.GetComponent<tinySpinAttack>().plyrTrfm = getPos(isP1);
            tinySpinObjectNew.GetComponent<playerMeleeObj>().heldByP1 = isP1;
            meleeCD = 50;
            return true;
        }
        return false;
    }

    public void OnFaceDirection(InputAction.CallbackContext context)
    {
        /*Added by vikram*/
        PlayerInput playerInput;
        if (GetComponent<PlayerInputHandler>())
            playerInput = GetComponent<PlayerInputHandler>().playerConfig.Input;
        else
            playerInput = GetComponent<PlayerInput>();
        //Debug.Log(playerInput);
        //Debug.Log(playerInput.gameObject.name);
        //Debug.Log(playerInput.);
        //Debug.Log(context.ReadValue<Vector2>());
        if (playerInput.currentControlScheme == "Gamepad")
        {

            faceDirection = context.ReadValue<Vector2>();
            if (faceDirection == Vector2.zero)
                isBeingTurned = false;
            else
                isBeingTurned = true;
        }
        else if (playerInput.currentControlScheme == "Keyboard&Mouse" || playerInput.currentControlScheme == null)
        {
            //get a vector from the parent's origin to the mouse's
            Vector2 cursorPosition = context.ReadValue<Vector2>();
            Vector2 currentPosition;
            if (vs != null)
            {
                //array of vector2 holding vs.GetPlayerScreenPositions()
                Vector2[] playerScreenPositions = vs.GetPlayerScreenPositions();


                //currentPosition =
                //    playerScreenPositions[playerInput.playerIndex];
                //do the above but check if playerScreenPositions is big enough
                //Debug.Log(playerScreenPositions);
                if (playerScreenPositions.Length >= playerInput.playerIndex)
                {
                    currentPosition =
                        playerScreenPositions[playerInput.playerIndex];
                }
                else
                {
                    //currentPosition = new Vector2(100, 100);
                    currentPosition = cursorPosition;
                }
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
        if (timerEnd - timerStart >= stunTimer)
        {
            rb.velocity =
                new Vector2(moveDirection.x * moveSpeed,
                    moveDirection.y * moveSpeed);
            isStunned = false;
        }
        else if (isStunned)
        {
            // player stepped on ketchup glob and cannot move for stunTimer seconds
            rb.velocity = new Vector2(0.0f, 0.0f);
        }
        else
        {
            // player stepped on mustard glob and is slower by a factor of moveDebuf
            rb.velocity =
                new Vector2(moveDirection.x * moveSpeed * moveDebuf,
                    moveDirection.y * moveSpeed * moveDebuf);
        }
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
        hands[1].localEulerAngles = new Vector3(0, 0, angle);
        if (weaponEquipped)
            m_weapon.Rotate(faceDirection);
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

            hands[0].localEulerAngles = new Vector3(0, 0, angleIdle);
            movement = true;
        }
        else
        {
            anim.SetFloat("angle", angleIdle - 90);
        }
        anim.SetBool("isMoving", movement);
    }

    public void SetSlowdown()
    {
        timerStart = timerEnd;
        StartCoroutine(HitAnimation());
    }

    public void SetStun()
    {
        timerStart = timerEnd;
        isStunned = true;
        StartCoroutine(HitAnimation());
    }

    private void EnemyDamage()
    {
        //Debug.Log("oof");
        timerStart = timerEnd;
        isStunned = true;
        StartCoroutine(HitAnimation());
        if (weaponEquipped)
        {
            weaponEquipped = m_weapon.Drop();
        }
    }

    IEnumerator HitAnimation()
    {
        // loops decrease then increase alpha channel in order to create a blinking effect
        // when hitting a hazard; can potentially extend this to getting hit by enemies
        // if you replace stunTimer with a parameter
        Color tmp = sprite.color;
        while (timerEnd - timerStart < stunTimer)
        {
            for (float alpha = 1f; alpha >= 0f; alpha -= 0.1f)
            {
                tmp.a = alpha;
                sprite.color = tmp;
                yield return new WaitForSeconds(0.05f);
            }
            for (float alpha = 0; alpha <= 1f; alpha += 0.1f)
            {
                tmp.a = alpha;
                sprite.color = tmp;
                yield return new WaitForSeconds(0.05f);
            }
        }

        // reset alpha to normal and quit the coroutine
        tmp.a = 1f;
        sprite.color = tmp;
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "weapon" && (timerEnd - timerStart) > stunTimer)
        {
            if (m_weapon == null)
            {
                m_weapon = col.gameObject.GetComponent<weapon>();
            }
            weaponEquipped = m_weapon.PickUp(this);
        }
        else if (col.gameObject.tag == "enemy" && !isStunned)
        {
            if (col.gameObject.GetComponent<dashingEnemy>())
            {
                if (col.gameObject.GetComponent<dashingEnemy>().abilityStep)
                {
                    //Debug.Log("dash");
                    EnemyDamage();
                }
            }
            else if (col.gameObject.GetComponent<slashingEnemy>())
            {
                if (col.gameObject.GetComponent<Enemy>().abilityCast > 0)
                {
                    //Debug.Log("slash");
                    EnemyDamage();
                }
            }
            else if (col.gameObject.GetComponent<thunderboltBullet>())
            {
                //Debug.Log("projectile");
                EnemyDamage();
                Destroy(col.gameObject);

            }
        }
        if (col.gameObject.GetComponent<tinySpinAttack>())
        {
            if (weaponEquipped) ThrowFork();
        }
        //Debug.Log($"touched {col.name}");
    }
}
