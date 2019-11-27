using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMousePhysics : MonoBehaviour
{
	[SerializeField] Camera cam = default;

	private void Awake()
	{
		cam = FindObjectOfType<Camera>();
	}

	private void Update()
	{
		transform.position = cam.ScreenToWorldPoint(Input.mousePosition).WithZ(0);
	}
}

public static class Extension
{
	public static Vector3 WithZ(this Vector3 v, float value)
		=> new Vector3(v.x, v.y, value);
}