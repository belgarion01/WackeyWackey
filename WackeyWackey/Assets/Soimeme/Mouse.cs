using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    //Component
    Camera cam;
    Rigidbody2D rb2d;
    DistanceJoint2D distanceJoint;

    //Mouse variable
    float mouseX;
    float mouseY;

    [SerializeField] Transform startAnchor;

    [HideInInspector] public Vector2 mousePosition;
    [HideInInspector] public Vector2 cursorPosition => transform.position;

    [SerializeField] private float cursorSpeed = 20f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float smoothTime = 0.01f;

    Vector2 nextPosition;

    Vector2 refVelocity;

    //Ropes & Distance
    [HideInInspector] public List<Rope> ropeDistances;

    public float distanceAllowed;


    private void Start()
    {
        cam = Camera.main;
        rb2d = GetComponent<Rigidbody2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();

        transform.position = startAnchor.position;
    }

    private void Update()
    {
        //Movement
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        nextPosition = Vector2.SmoothDamp(transform.position, rb2d.position + new Vector2(mouseX, mouseY) * cursorSpeed * Time.deltaTime, ref refVelocity, smoothTime, maxSpeed);

        transform.right = ropeDistances[ropeDistances.Count - 1].target.position - ropeDistances[ropeDistances.Count - 1].origin.position;

        //Clamp distance
        distanceJoint.connectedAnchor = ropeDistances[ropeDistances.Count - 1].origin.position;
        distanceJoint.distance = GetRemainingLength();
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(nextPosition);
    }


    float GetRopeLength()
    {
        float distance = 0;

        for (int i = 0; i < ropeDistances.Count-1; i++)
        {
            distance += (ropeDistances[i].origin.transform.position - ropeDistances[i + 1].origin.transform.position).magnitude;
        }

        return distance;
    }

    float GetRemainingLength()
    {
        float distance = distanceAllowed - GetRopeLength();
        return distance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(cursorPosition, 0.2f);
    }
}
