using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRope : MonoBehaviour
{
    public GameObject ChainPrefab;
    public GameObject ChainHead;
    public GameObject ChainTail;

    private List<Transform> points = new List<Transform>();

    Vector2 currentOrigin;
    float offsetValue;

    private Transform root;

    public int numberOfChains = 10;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnChain(transform.position, numberOfChains);
        }
    }

    public void SpawnChain(Vector2 origin, int numberOfChains)
    {
        //Reset values
        offsetValue = ChainPrefab.GetComponent<CircleCollider2D>().radius * 2;
        currentOrigin = origin;

        //Root
        root = new GameObject("RopeRoot").transform;

        //Tail
        Transform tail = AddToRope(ChainTail);
        SetupAnchor(GetHingeJoint(tail));

        //First chain
        Transform firstChain = AddChain(tail);

        //Loop chain
        Transform previousChain = firstChain;
        for (int i = 0; i < numberOfChains; i++)
        {
            Transform chain = AddChain(previousChain);
            previousChain = chain;
        }

        //Head
        Transform head = AddToRope(ChainHead);
        ConnectRigidbody(GetHingeJoint(head), previousChain);
        DistanceJoint2D distJoint = head.GetComponent<DistanceJoint2D>();
        ConnectRigidbody(distJoint, tail);
        distJoint.distance = offsetValue * (numberOfChains + 2);
        head.GetComponent<PlugController>().anchorPoint = tail.transform;
        SetupAnchor(GetHingeJoint(head.transform));
        head.GetComponent<PlugController>().points = points;

        //Debug.Break();
    }

    Transform AddToRope(GameObject prefab)
    {
        Transform obj = Instantiate(prefab, currentOrigin, Quaternion.identity, root).transform;
        AddToList(obj);
        AddOffset();
        return obj;
    }

    void AddOffset()
    {
        currentOrigin += offsetValue * Vector2.right;
    }

    void AddToList(Transform point)
    {
        points.Add(point);
    }


    Transform AddChain(Transform previousChain)
    {
        Transform chain = AddToRope(ChainPrefab);
        HingeJoint2D joint = GetHingeJoint(chain);

        SetupChainHingeJoint(ref joint, previousChain);

        return chain;
    }

    void SetupChainHingeJoint(ref HingeJoint2D joint, Transform previousChain)
    {
        ConnectRigidbody(joint, previousChain);

        SetupAnchor(joint);
        SetJointLimits(ref joint);
    }

    void SetJointLimits(ref HingeJoint2D joint)
    {
        JointAngleLimits2D limit = joint.limits;
        limit.min = -45f;
        limit.max = 45f;
        joint.limits = limit;
    }


    void SetupAnchor(HingeJoint2D joint)
    {
        //joint.connectedAnchor = new Vector2(0.2f, 0f);
    }

    HingeJoint2D GetHingeJoint(Transform obj)
    {
        return obj.GetComponent<HingeJoint2D>();
    }

    void ConnectRigidbody(Joint2D joint, Transform target)
    {
        joint.connectedBody = target.GetComponent<Rigidbody2D>();
    }
}
