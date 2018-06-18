using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckClick : MonoBehaviour {

	public bool checkFromUpdate = false;
	public bool checkFromMethod = true;

	void Update () {
		if (checkFromUpdate)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Debug.Log("Click caught from Update! Timestamp: " + Time.time);
			}
		}
	}

	void OnMouseDown()
	{
		if (checkFromMethod)
		{
			Debug.Log("Click caught from OnMouseDown method! Timestamp: " + Time.time);
		}
	}
}
