using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlugController : MonoBehaviour
{
    //[SerializeField]
    //private Transform[] points = default;

    //[SerializeField]
    public List<Transform> points = default;

    [SerializeField]
    private LineRenderer lineRenderer = default;

    [Space]

    //[SerializeField]
    public Transform anchorPoint = default;
    private float radius;

    [Space]

    [SerializeField]
    private float smoothTime;
    [SerializeField]
    private Rigidbody2D rigidBody2D = default;
    private Vector3 velocity;

    private void Start()
    {
        lineRenderer.positionCount = points.Count;

        velocity = rigidBody2D.velocity;
        radius = Vector2.Distance(anchorPoint.position, transform.position);
    }

    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var correctedPosition = Vector2.ClampMagnitude(mousePosition, radius);
        rigidBody2D.MovePosition(Vector3.SmoothDamp(rigidBody2D.position, correctedPosition, ref velocity, smoothTime));

        var linePoints = points.ToList().ConvertAll(p => p.position);
        lineRenderer.SetPositions(linePoints.ToArray());
    }
}
