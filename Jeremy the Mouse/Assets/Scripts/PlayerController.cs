using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	// ============================== PUBLIC FIELDS ==============================
	[Header("Move settings")]
	public float moveSpeed = 10f;
	public float crouchSpeed = 5f;

	[Space(10)]
	[Header("Jump settings")]
	public LayerMask ground;
	public float minJumpForce = 500f;
	public float maxJumpForce = 1500f;
	public float jumpPrepareTime = 2f;
	public float numberOfTicks = 4f;
	public bool airControl;

	[Space(10)]
	[Header("Object assignment")]
	public CapsuleCollider2D walkCollider;
	public CapsuleCollider2D crouchCollider;
	public PolygonCollider2D hangingCollider;
	public BoxCollider2D groundCollider;
	public LayerMask pushableLayers;

	[Space(10)]
	[Header("Working keys")]
	public KeyCode moveRightKey = KeyCode.D;
	public KeyCode moveRightAlternativeKey = KeyCode.RightArrow;
	public KeyCode moveLeftKey = KeyCode.A;
	public KeyCode moveLeftAlternativeKey = KeyCode.LeftArrow;
	public KeyCode crouchKey = KeyCode.S;
	public KeyCode crouchAlternativeKey = KeyCode.LeftControl;
	public KeyCode jumpKey = KeyCode.W;
	public KeyCode jumpAlternativeKey = KeyCode.Space;
	public KeyCode pushKey = KeyCode.RightShift;
	public KeyCode pushAlternativeKey = KeyCode.Z;
	// ============================== [end of PUBLIC FIELDS] ==============================

	// ============================== PRIVATE FIELDS ==============================
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

	private bool hanging = false;

	private Rigidbody2D rb2D;
	private Animator anim;
	// ============================== [end of PRIVATE FIELDS] ==============================


	/* ====================================== *
	 *                 AWAKE                  *
	 * ====================================== */
	void Awake ()
	{
		rb2D = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		if (rb2D == null) Debug.LogError("No Rigidbody2D component found attached to this gameObject! [PLAYER_CONTROLLER.CS]");
		if (anim == null) Debug.LogError("No Animator component found attached to this gameObject! [PLAYER_CONTROLLER.CS]");
		if (walkCollider == null) Debug.LogError("No walk Collider2D component found attached to this gameObject! [PLAYER_CONTROLLER.CS]");
		if (crouchCollider == null) Debug.LogError("No crouch Collider2D component found attached to this gameObject! [PLAYER_CONTROLLER.CS]");
		if (hangingCollider == null) Debug.LogError("No hanging PolygonCollider2D component found attached to this gameObject! [PLAYER_CONTROLLER.CS]");
	}

	/* ====================================== *
	 *                 START                  *
	 * ====================================== */
	void Start ()
	{
		facingRight = (transform.localScale.x == 1);
		currentJumpForce = minJumpForce;
		currentMoveSpeed = moveSpeed;
		walkCollider.enabled = true;
		crouchCollider.enabled = false;
		hangingCollider.enabled = false;
	}

	/* ====================================== *
	 *                 UPDATE                 *
	 * ====================================== */
	void Update ()
	{
		//grounded = Physics2D.Linecast (transform.position, groundCheck.position, ground);
		//ContactFilter2D filter;
		//filter
		RaycastHit2D[] hitObjects = new RaycastHit2D[20];
		grounded = (groundCollider.Cast(Vector2.zero, hitObjects) > 0) ? true : false;

		direction = new Vector3 (0, 0, 0);

		if ((Input.GetKeyDown(moveRightKey) || Input.GetKeyDown(moveRightAlternativeKey)) && !isPreparingToJump) facingRight = true;
		if ((Input.GetKeyDown(moveLeftKey) || Input.GetKeyDown(moveLeftAlternativeKey)) && !isPreparingToJump) facingRight = false;

		if ((Input.GetKey (moveRightKey) || Input.GetKey (moveRightAlternativeKey)) && !isPreparingToJump) moving = true;
		else if ((Input.GetKey (moveLeftKey) || Input.GetKey (moveLeftAlternativeKey)) && !isPreparingToJump) moving = true;
		else moving = false;

		anim.SetBool("isMoving", moving);
		anim.SetBool("isGrounded", grounded);
		anim.SetBool("isHanging", hanging);

		if ((airControl && !grounded) || grounded)
		{
			transform.localScale = (facingRight ? new Vector3(1,1,1) : new Vector3(-1,1,1));
			direction += (moving ? new Vector3(transform.localScale.x,0,0) : Vector3.zero);
		}

// =============== PUSH DEBUG ===================
		Debug.DrawLine(transform.position, transform.position + Vector3.right * 2f);
// ============ ENDOF PUSH DEBUG ================

		if (Input.GetKey(crouchKey) || Input.GetKey(crouchAlternativeKey))
		{
			currentMoveSpeed = crouchSpeed;
			walkCollider.enabled = false;
			crouchCollider.enabled = true;
		}
		else if (Input.GetKeyDown(pushKey) || Input.GetKeyDown(pushAlternativeKey))
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, (facingRight) ? Vector3.right : Vector3.left, 2f, pushableLayers);
			if (hit != null)
			{
				PushableObject pushObject = hit.transform.gameObject.GetComponent<PushableObject>();

				if (pushObject != null)
				{
					pushObject.Push(transform.position);
					anim.SetTrigger("push");
				}
			}
		}
		else
		{
			currentMoveSpeed = moveSpeed;
			walkCollider.enabled = true;
			crouchCollider.enabled = false;

			if ((Input.GetKeyDown(jumpKey) || Input.GetKeyDown(jumpAlternativeKey)) && grounded)
			{
				isPreparingToJump = true;
				currentJumpForce = minJumpForce;
				currentTime = 0f;
				currentTicks = 0f;
			}

			bool forceJump = false;

			if ((Input.GetKey(jumpKey) || Input.GetKey(jumpAlternativeKey)) && isPreparingToJump)
			{
				currentTime += Time.deltaTime;

				if (currentTime >= (jumpPrepareTime / numberOfTicks))
				{
					currentJumpForce += (maxJumpForce - minJumpForce) / numberOfTicks;
					currentTicks++;
					//Debug.Log("TICK-TACK");
					currentTime = 0f;
				}

				if (currentTicks == numberOfTicks)
				{
					forceJump = true;
				}
			}

			if (((Input.GetKeyUp(jumpKey) || Input.GetKeyUp(jumpAlternativeKey)) && grounded && isPreparingToJump) || forceJump)
			{
				isPreparingToJump = false;
				isJumping = true;
			}
		}
	}

	/* ====================================== *
	 *              FIXED UPDATE              *
	 * ====================================== */
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
}
