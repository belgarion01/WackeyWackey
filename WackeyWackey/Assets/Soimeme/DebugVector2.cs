using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVector2 : MonoBehaviour
{
    public float angle;
    Vector3 vForce;

    public Transform test;

    void Start()
    {
        vForce = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        vForce = Vector2.right;
        Debug.Log(vForce);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(Vector2.SignedAngle(vForce, test.position - transform.position));
        }

        Debug.DrawLine(transform.position, vForce);
    }
}
