using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    public GameObject prefab;
    GameObject sprite;
    LineRenderer line;

    Quaternion direction;
    Vector2 offscreen = new Vector2(999f, 999f);

    enum Mode { Controller, Mouse };
    Mode aimMode = Mode.Mouse;

    void Start()
    {
        direction = Quaternion.Euler(0, 0, 0);
        sprite = Instantiate(prefab, offscreen, Quaternion.identity);
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        RenderReticle();

        float x = 0;//Input.GetAxis("RightHorizontal") * 10;
        float y = 0;//Input.GetAxis("RightVertical") * 10;
        float z;

        aimMode = Mode.Mouse;

        if (aimMode == Mode.Mouse)
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            z = Vector2.Angle(mouse, Vector2.up);
            x = mouse.x;
            if (x > 0)
            {
                z = 360 - z;
            }
            direction = Quaternion.Euler(0f, 0f, z);
        }
        else
        {

            // If there is no input, don't do anything.
            if (x == 0 && y == 0) { return; }

            // Translate the x and y coords to an angle from up.
            Vector3 vec = new Vector3(x, y, 0);
            z = Vector2.Angle(Vector2.up, vec);

            // I have no idea why this works, but it does.
            if (x > 0)
            {
                z += 180;
            }
            else
            {
                z = 180 - z;
            }

            direction = Quaternion.Slerp(direction, Quaternion.Euler(0f, 0f, z), 0.2f);
        }
    }

    // If there is a target, render the reticle there. Otherwise, render it offscreen.
    void RenderReticle()
    {
        Vector3 target = CurrentTarget();
        line.positionCount = 2;
        line.SetPosition(0, transform.position);

        if (target.x == 0 && target.y == 0)
        {
            sprite.transform.position = offscreen;
            line.SetPosition(1, transform.position + direction * (Vector3.up * 50));
        }
        else
        {
            sprite.transform.position = target;
            line.SetPosition(1, target);
        }
    }

    // Shoot a raycast toward direction and return the hit point
    public Vector2 CurrentTarget()
    {
        LayerMask mask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.CircleCast(transform.position + (Vector3.up * 1f), 0.1f, direction * Vector3.up, Mathf.Infinity, mask);
        if (hit && hit.collider)
        {
            if (hit.collider.gameObject.GetComponent<Snappable>())
            {
                return hit.collider.gameObject.transform.position;
            }
        }
        return hit.point;
    }
}
