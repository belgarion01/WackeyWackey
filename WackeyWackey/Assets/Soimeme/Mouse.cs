using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Camera cam;

    float mouseX;
    float mouseY;

    [HideInInspector]
    public Vector2 mousePosition;

    [HideInInspector]
    public Vector2 cursorPosition => transform.position;

    public float mouseSpeed;

    Rigidbody2D rb2d;

    Vector2 refVelocity;

    Vector2 futurPosition;

    private float maxSpeed = 10f;

    private float smoothTime = 0.01f;

    private void Start()
    {
        cam = Camera.main;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        futurPosition = Vector2.SmoothDamp(transform.position, rb2d.position + new Vector2(mouseX, mouseY) * mouseSpeed * Time.deltaTime, ref refVelocity, smoothTime, maxSpeed);

        //rb2d.MovePosition(rb2d.position + new Vector2(mouseX, mouseY) * mouseSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //rb2d.MovePosition(rb2d.position + new Vector2(mouseX, mouseY) * mouseSpeed * Time.deltaTime);
        rb2d.MovePosition(futurPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(cursorPosition, 0.2f);
    }
}
