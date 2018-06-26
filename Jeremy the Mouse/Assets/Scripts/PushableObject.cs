using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour {

	public float pushForce = 1000f;
	[Tooltip("Time, left to the object after the push action to become static again.")]
	public float staticBodyTimeMax = 2f;

	private float currentStaticBodyTime = 0f;
	private bool pushed = false;
	private Vector3 pushDirection;
	private Rigidbody2D rb2D;

	void Awake ()
	{
		rb2D = GetComponent<Rigidbody2D>();

		if (rb2D == null) Debug.LogError("No rigidbody2D component found attached to this gameObject! [PUSHABLE_OBJECT.CS]");
	}

	void Update ()
	{
		if (pushed)
		{
			StaticBodyTimer();
		}
	}

	private void StaticBodyTimer()
	{
		currentStaticBodyTime -= Time.deltaTime;
		if (currentStaticBodyTime <= 0)
		{
			pushed = false;
			rb2D.bodyType = RigidbodyType2D.Static;
		}
	}

	public void Push (Vector3 mousePosition)
	{
		if (!pushed)
		{
			pushed = true;
			pushDirection = (mousePosition.x < transform.position.x) ? Vector3.right : Vector3.left;
			currentStaticBodyTime = staticBodyTimeMax;
			rb2D.bodyType = RigidbodyType2D.Dynamic;
			rb2D.AddForce(pushDirection * pushForce);
		}
	}
}
