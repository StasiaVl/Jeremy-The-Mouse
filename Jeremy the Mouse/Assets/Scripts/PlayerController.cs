using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public LayerMask ground;
	public float moveSpeed = 10f;
	public float crouchSpeed = 5f;
	public float minJumpForce = 500f;
	[Range(500f, 2000f)]
	public float maxJumpForce = 1000f;
	public float jumpPrepareTime = 2f;
	public float numberOfTicks = 4f;
	public bool airControl;
	public Transform groundCheck;

	private bool moving = false;
	private Vector3 direction;
	private bool facingRight = false;
	private float currentMoveSpeed;

	private bool grounded = true;
	private bool isPreparingToJump = false;
	private bool isJumping = false;
	private float currentJumpForce;
	private float currentTime = 0f;
	private float currentTicks = 0f;

	private Rigidbody2D rb2D;
	private Animator anim;

	void Awake ()
	{
		rb2D = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		if (rb2D == null) Debug.LogError("No Rigidbody2D component found attached to this gameObject! [PLAYER_CONTROLLER.CS]");
		if (anim == null) Debug.LogError("No Animator component found attached to this gameObject! [PLAYER_CONTROLLER.CS]");
	}

	void Start ()
	{
		facingRight = (transform.localScale.x == 1);
		currentJumpForce = minJumpForce;
		currentMoveSpeed = moveSpeed;
	}

	void Update ()
	{
		grounded = Physics2D.Linecast (transform.position, groundCheck.position, ground);

		direction = new Vector3 (0, 0, 0);

		if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !isPreparingToJump) facingRight = true;
		if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !isPreparingToJump) facingRight = false;

		if ((Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) && !isPreparingToJump) moving = true;
		else if ((Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) && !isPreparingToJump) moving = true;
		else moving = false;

		anim.SetBool("isMoving", moving);
		anim.SetBool("isGrounded", grounded);

		if ((airControl && !grounded) || grounded)
		{
			transform.localScale = (facingRight ? new Vector3(1,1,1) : new Vector3(-1,1,1));
			direction += (moving ? new Vector3(transform.localScale.x,0,0) : Vector3.zero);
		}

		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftControl))
		{
			currentMoveSpeed = crouchSpeed;
		}
		else
		{
			currentMoveSpeed = moveSpeed;

			if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && grounded)
			{
				isPreparingToJump = true;
				currentJumpForce = minJumpForce;
				currentTime = 0f;
				currentTicks = 0f;
			}

			bool forceJump = false;

			if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isPreparingToJump)
			{
				currentTime += Time.deltaTime;

				if (currentTime >= (jumpPrepareTime / numberOfTicks))
				{
					currentJumpForce += (maxJumpForce - minJumpForce) / numberOfTicks;
					currentTicks++;
					Debug.Log("TICK-TACK");
					currentTime = 0f;
				}

				if (currentTicks == numberOfTicks)
				{
					forceJump = true;
				}
			}

			if (((Input.GetKeyUp(KeyCode.Space)  || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) && grounded && isPreparingToJump) || forceJump)
			{
				isPreparingToJump = false;
				isJumping = true;
			}
		}
	}

	void FixedUpdate ()
	{
		if (direction != new Vector3 (0, 0, 0))
		{
			transform.Translate (currentMoveSpeed * direction * Time.deltaTime);
		}

		if (isJumping)
		{
			rb2D.AddForce (new Vector2 (currentJumpForce * transform.localScale.x * 0.3f, currentJumpForce));
			isJumping = false;
			currentTime = 0f;
			currentTicks = 0f;
			currentJumpForce = minJumpForce;
		}
	}

	private void ChangeObjectDirection(Vector3 newScale)
	{
		transform.localScale = newScale;
	}
}
