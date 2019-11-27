using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ropa : MonoBehaviour
{
    Vector2 mousePosition;

    LineRenderer lRenderer;

    public List<Collider2D> lastHitCollider;

    public LayerMask BordMask;

    public LayerMask MurMask;

    private void Start()
    {
        lRenderer = GetComponent<LineRenderer>();
        lRenderer.SetPosition(0, transform.position);
        lRenderer.SetPosition(GetLastIndex(), mousePosition);
    }

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);



        lRenderer.SetPosition(GetLastIndex(), mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(lRenderer.GetPosition(GetLastIndex()-1), GetLastPosition() - (Vector2)lRenderer.GetPosition(GetLastIndex() - 1), Vector2.Distance(lRenderer.GetPosition(GetLastIndex() - 1), GetLastPosition()), BordMask);

        if (hit)
        {
            lRenderer.SetPosition(GetLastIndex(), hit.point);
            AddSegment(hit.point);

            //if(lastHitCollider.Count > 0) lastHitCollider[lastHitCollider.Count].enabled = true;
            //lastHitCollider = hit.collider;
            
            //hit.collider.enabled = false;

            DisableCollider(hit.collider);
        }

        hit = Physics2D.Raycast(GetLastPosition(), (Vector2)lRenderer.GetPosition(GetLastIndex() - 2) - GetLastPosition(), Mathf.Infinity, MurMask);

        Debug.DrawRay(GetLastPosition(), (Vector2)lRenderer.GetPosition(GetLastIndex() - 2) - GetLastPosition(), Color.white);

        if (lRenderer.positionCount > 2)
        {
            if (!hit)
            {
                RemoveLastPoint();
            }
        }

        //if (hit)
        //{
        //    lRenderer.SetPosition(GetLastIndex(), hit.point);
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    AddSegment(mousePosition);
        //}
    }

    void DisableCollider(Collider2D collider)
    {
        collider.enabled = false;

        lastHitCollider.Add(collider);


        if (lastHitCollider.Count > 1)
        {
            lastHitCollider[lastHitCollider.Count-2].enabled = true;
        }       
    }

    void RemoveLastPoint()
    {
        lRenderer.positionCount--;

        lastHitCollider[lastHitCollider.Count - 1].enabled = true;
        if(lastHitCollider.Count>1)
        lastHitCollider[lastHitCollider.Count - 2].enabled = false;
        lastHitCollider.RemoveAt(lastHitCollider.Count - 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(mousePosition, 0.2f);
    }

    void AddSegment(Vector2 position)
    {
        lRenderer.positionCount++;
        lRenderer.SetPosition(GetLastIndex()-1, position);

        lRenderer.SetPosition(GetLastIndex(), mousePosition);
    }

    Vector2 GetLastPosition()
    {
        return lRenderer.GetPosition(GetLastIndex());
    }

    int GetLastIndex()
    {
        return lRenderer.positionCount-1;
    }
}
