using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Rope : MonoBehaviour
{
	[SerializeField] Rope prefab = default;
	[SerializeField] Transform origin = default;
	[SerializeField] Transform target = default;

	public UnityEvent OnLock = default;
	public UnityEvent OnUnlock = default;

	RaycastHit2D hit;
	Rope nextRope = default;

	public bool Lock { get; private set; } = false;
	Vector3 Direction => origin.position - target.position;
    Vector3 RaycastDirection => (origin.position - (Vector3)cornerOffset) - target.position;
    //public float UnlockAngle => Vector2.SignedAngle(Vector2.up, Direction.normalized)

    private Vector2 direction;


    public Transform canvas;
    public Text guiText1;
    public Text guiText2;

    Vector3 mousePosition;

    enum Position { CounterClock, Clock }
    Position position;

    public LayerMask bordMask;
    public LayerMask murMask;

    Vector3 futurPosition;
    Vector2 cornerOffset;

    bool LockerRunning = false;

	void Update()
	{
		//Debug.Log($"{gameObject.name} {UnlockAngle}");
		Debug.DrawLine(origin.position, target.position);
		Debug.DrawLine(Vector2.zero, -Direction.normalized, Color.cyan);

        mousePosition = GetComponentInChildren<FollowMousePhysics>().transform.position;

        if(nextRope != null)
        {
            if (!nextRope.Lock)
            {

            }
        }

        //guiText1.text = UnlockAngle.ToString();
        //if(nextRope != null) guiText2.text = nextRope.UnlockAngle.ToString();

        canvas.transform.position = origin.position+(Vector3)Vector2.up*1;

		if (Lock)
		{
            if (nextRope != null)
            {
                if (!nextRope.Lock)
                {
                    HandleUnlock();
                }
            }
			return;
		}

		hit = Physics2D.Raycast(target.position, Direction, Direction.magnitude, murMask);

		if (hit.collider != null)
		{
            RaycastHit2D hit2 = Physics2D.Raycast(hit.point, Vector2.zero, 0f, bordMask);

            if (hit2)
            {
                futurPosition = hit2.collider.GetComponent<Corner>().handlePosition;
                cornerOffset = hit2.collider.GetComponent<Corner>().offset;
                if (!LockerRunning) StartCoroutine(Locker(hit));
            }

            //if (!LockerRunning) StartCoroutine(Locker(hit));

			////nextRope = Instantiate(prefab);
			////nextRope.SetOriginPosition(hit.point + hit.normal * .1f);
			////target = nextRope.origin;
			////Lock = true;
			////OnLock?.Invoke();

   ////         direction = mousePosition - origin.position;

        }
	}

	void HandleUnlock()
	{
        if (position == Position.Clock)
        {
            if (Vector2.SignedAngle(direction, mousePosition - origin.position) < 0)
                return;
        }

        if(position == Position.CounterClock)
        {
            if (Vector2.SignedAngle(direction, mousePosition - origin.position) > 0)
                return;
        }

		Destroy(nextRope.gameObject);
		target = GetComponentInChildren<FollowMousePhysics>().transform;
		Lock = false;
		OnUnlock?.Invoke();
	}

    IEnumerator Locker(RaycastHit2D hit)
    {
        LockerRunning = true;
        Lock = true;

        //Vector2 hitPosition = hit.point;
        //Vector2 hitNormal = hit.normal;


        ////Vector2 mouseHitPosition = mousePosition;

        ////while(Vector2.Distance(mouseHitPosition, mousePosition) < 0.1f)
        ////{
        ////    yield return new WaitForEndOfFrame();
        ////}

        yield return new WaitForEndOfFrame();
        if (Vector2.SignedAngle(direction, mousePosition - origin.position) > 0) position = Position.CounterClock;
        if (Vector2.SignedAngle(direction, mousePosition - origin.position) <= 0) position = Position.Clock;

        ////Debug.Log(Vector2.SignedAngle(direction, mousePosition - origin.position) + " ===> " + position);

        this.nextRope = Instantiate(prefab);
        this.nextRope.SetOriginPosition(/*hitPosition + hitNormal * .1f*/futurPosition);
        target = this.nextRope.origin;
        OnLock?.Invoke();

        direction = mousePosition - (origin.position-(Vector3)cornerOffset);

        LockerRunning = false;
    } 

	public void SetOriginPosition(Vector3 position) => origin.position = position;
}
