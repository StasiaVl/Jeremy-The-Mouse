using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public bool followPlayer = true;
	public float speed = 10f;
	public Transform player;

	private Transform target;
	private Vector3 mousePos;

	void Awake ()
	{
		if (player == null) Debug.LogError("No target Player found attached to this gameObject! [CAMERA_CONTROLLER.CS]");
	}

	void Start ()
	{
		mousePos = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
	}

	void LateUpdate ()
	{
		mousePos = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = -10;
		if (followPlayer)
			this.transform.position = new Vector3(player.position.x, player.position.y, -10);
		else
			this.transform.position = Vector3.MoveTowards (this.transform.position, (player.position*2.5f + mousePos) /7*2 , speed * Time.deltaTime);
	}
}
