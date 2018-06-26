using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingPlayerTest : MonoBehaviour {

	public float hangRotatePower = 100f;

	private Rigidbody2D rb2D;

	void Awake ()
	{
		rb2D = GetComponent<Rigidbody2D> ();

		if (rb2D == null) Debug.LogError("No Rigidbody2D component found assigned to this gameObject! [HANGING_PLAYER_TEST.CS]");
	}

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey(KeyCode.A))
		{
			rb2D.AddTorque(-hangRotatePower);
		} else if (Input.GetKey(KeyCode.D))
		{
			rb2D.AddTorque(hangRotatePower);
		}
	}
}
