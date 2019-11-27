using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner : MonoBehaviour
{
    public Vector3 center => transform.position;

    private Transform RopeHandle;

    public Vector3 handlePosition => RopeHandle.position;

    public Vector3 offset => RopeHandle.position - center;

    private void Start()
    {
        RopeHandle = transform.GetChild(0);
    }
}
