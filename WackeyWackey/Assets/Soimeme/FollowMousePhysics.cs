using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMousePhysics : MonoBehaviour
{
	[SerializeField] Camera cam = default;
    Rigidbody2D rb2d;

    Vector2 toMouse;

    Vector2 mousePosition;
    public float toMouseMinDistance;

    public float maximumVelocity;

    public float parameter;


    private void Awake()
	{
		cam = FindObjectOfType<Camera>();
        rb2d = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		mousePosition = cam.ScreenToWorldPoint(Input.mousePosition).WithZ(0);
        toMouse = mousePosition - (Vector2)transform.position;
        if(toMouse.magnitude > toMouseMinDistance)
        {
            //rb2d.AddForce(toMouse * toMouse.magnitude * parameter);
            rb2d.MovePosition(mousePosition);
        }
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, toMouseMinDistance);
        Gizmos.DrawSphere(mousePosition, 0.05f);
    }
}

public static class Extension
{
	public static Vector3 WithZ(this Vector3 v, float value)
		=> new Vector3(v.x, v.y, value);
}