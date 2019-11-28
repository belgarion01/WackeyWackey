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

    private Vector2 direction;

    Vector3 mousePosition;

    enum Position { CounterClock, Clock }
    Position position;

    public LayerMask bordMask;
    public LayerMask murMask;

    Vector3 futurPosition;
    Vector2 cornerOffset;

    bool LockerRunning = false;

    Mouse mouse;

    private void Start()
    {
        mouse = FindObjectOfType<Mouse>();
        target = mouse.transform;
    }

    void Update()
	{
		Debug.DrawLine(origin.position, target.position);

        mousePosition = mouse.cursorPosition;

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
		target = mouse.transform;
		Lock = false;
		OnUnlock?.Invoke();
	}

    IEnumerator Locker(RaycastHit2D hit)
    {
        LockerRunning = true;
        Lock = true;

        yield return new WaitForEndOfFrame();
        if (Vector2.SignedAngle(direction, mousePosition - origin.position) > 0) position = Position.CounterClock;
        if (Vector2.SignedAngle(direction, mousePosition - origin.position) <= 0) position = Position.Clock;

        this.nextRope = Instantiate(prefab);
        this.nextRope.SetOriginPosition(futurPosition);
        target = this.nextRope.origin;
        OnLock?.Invoke();

        direction = mousePosition - (origin.position-(Vector3)cornerOffset);

        LockerRunning = false;
    } 

	public void SetOriginPosition(Vector3 position) => origin.position = position;
}
