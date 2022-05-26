using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public int maxHealth = 100;
	public int currentHealth;

	[SerializeField] Camera cam;
	[SerializeField] bool isPlayerOne;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] float spd;
	Vector2 movement, mousePos;
	KeyCode up, down, left, right;
	Transform trfm;

	[SerializeField] GameObject atkObj;

	public HealthBar healthBar;

	// Start is called before the first frame update
	void Start()
	{
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);

		//assign keybinds depending on player
		if (isPlayerOne) { up = KeyCode.W; down = KeyCode.S; left = KeyCode.A; right = KeyCode.D; }
		else { up = KeyCode.UpArrow; down = KeyCode.DownArrow; left = KeyCode.LeftArrow; right = KeyCode.RightArrow; }
		
		trfm = transform;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			TakeDamageTest(20);
		}
	}

    private void FixedUpdate()
    {
		keyboardMovement();
		rb.velocity = movement;
	}

	void keyboardMovement()
    {
		if (true) // if (isPlayerOne)
        {
			mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
			trfm.rotation = Quaternion.AngleAxis(Mathf.Atan2(trfm.position.y - mousePos.y, trfm.position.x - mousePos.x) * Mathf.Rad2Deg + 90, Vector3.forward);
		}

		if (Input.GetKey(up))
		{
			if (Input.GetKey(left))
			{
				if (!Input.GetKey(right))
				{
					movement.x = -spd * .707f;
					movement.y = spd * .707f;
				}
			}
			else
			if (Input.GetKey(right))
			{
				movement.x = spd * .707f;
				movement.y = spd * .707f;
			}
			else
			{
				movement.x = 0;
				movement.y = spd;
			}
		}
		else
		if (Input.GetKey(down))
		{
			if (Input.GetKey(left))
			{
				if (!Input.GetKey(right))
				{
					movement.x = -spd * .707f;
					movement.y = -spd * .707f;
				}
			}
			else
			if (Input.GetKey(right))
			{
				movement.x = spd * .707f;
				movement.y = -spd * .707f;
			}
			else
			{
				movement.x = 0;
				movement.y = -spd;
			}
		}
		else
		{
			if (Input.GetKey(left))
			{
				if (!Input.GetKey(right))
				{
					movement.x = -spd;
					movement.y = 0;
				}
			}
			else
			if (Input.GetKey(right))
			{
				movement.x = spd;
				movement.y = 0;
			} else
            {
				movement.x = 0;
				movement.y = 0;
            }
		}
	}

    void TakeDamageTest(int damage)
	{
		currentHealth -= damage;

		healthBar.SetHealth(currentHealth);
	}
}