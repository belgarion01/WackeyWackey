using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeExtension : MonoBehaviour
{
    [SerializeField] private float extensionValue = 10f;

    bool enable = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Mouse mouse) && enable)
        {
            LockRope(mouse);
            GiveExtension(mouse);
            enable = false;
        }
    }

    void LockRope(Mouse mouse)
    {
        //mouse.ropeDistances[mouse.ropeDistances.Count - 1].Lock();
        //StartCoroutine(mouse.ropeDistances[mouse.ropeDistances.Count - 1].Locker(true));
    }

    void GiveExtension(Mouse mouse)
    {
        mouse.distanceAllowed += extensionValue;
    }
}
