using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	/*
	 * Increase gravity scale to 5 (for example) and jump force to 1000.
	 */

	public LayerMask ground;
	public float moveSpeed = 10f;
	public float jumpForce = 1000f;
	public Transform groundCheck;

	private bool grounded = true;
	private bool isJumping = false;
	private bool facingRight = false;
	private Vector3 direction;
	private Rigidbody2D rb2D;


	void Awake ()
	{
		rb2D = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		grounded = Physics2D.Linecast (transform.position, groundCheck.position, ground);

		direction = new Vector3 (0, 0, 0);

		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			transform.localScale = new Vector3(1, 1, 1);
		}

		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow))
		{
			direction += Vector3.right * 0.1f * moveSpeed;
		}

		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}

		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow))
		{
			direction += Vector3.left * 0.1f * moveSpeed;
			facingRight = false;
		}

		if (Input.GetKey (KeyCode.Space) && grounded)
		{
			isJumping = true;
		}
	}

	void FixedUpdate ()
	{
		if (direction != new Vector3 (0, 0, 0))
			transform.Translate (moveSpeed * direction * Time.deltaTime);

		if (isJumping)
		{
			rb2D.AddForce (new Vector2 (0f, jumpForce));
			isJumping = false;
		}
	}
}