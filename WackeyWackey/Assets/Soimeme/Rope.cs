using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Rope : MonoBehaviour
{
	[SerializeField] Rope prefab = default;
    public Transform origin = default;
    public Transform target = default;

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
    [HideInInspector]
    public Vector2 lastCornerOffset = Vector2.zero;

    bool LockerRunning = false;

    bool SuperLock = false;

    LineRenderer lRenderer;

    Mouse mouse;

    private void Start()
    {
        mouse = FindObjectOfType<Mouse>();
        lRenderer = GetComponent<LineRenderer>();
        target = mouse.transform;
        mouse.ropeDistances.Add(this);
    }

    void Update()
	{

		Debug.DrawLine(origin.position, target.position);

        mousePosition = mouse.cursorPosition;

        UpdateLineRenderer();

        if (SuperLock) return;


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
            RaycastHit2D cornerHit = Physics2D.Raycast(hit.point, Vector2.zero, 0f, bordMask);

            if (cornerHit)
            {
                futurPosition = cornerHit.collider.GetComponent<Corner>().handlePosition;
                cornerOffset = cornerHit.collider.GetComponent<Corner>().offset;
                if (!LockerRunning)
                {
                    StartCoroutine(Locker());
                    
                }

                
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
        mouse.ropeDistances.Remove(nextRope);
		Destroy(nextRope.gameObject);
		target = mouse.transform;
        cornerOffset = Vector2.zero;
		Lock = false;
		OnUnlock?.Invoke();
	}

    public IEnumerator Locker()
    {
        LockerRunning = true;
        Lock = true;

        yield return new WaitForEndOfFrame();
        if (Vector2.SignedAngle(direction, mousePosition - origin.position) > 0) position = Position.CounterClock;
        if (Vector2.SignedAngle(direction, mousePosition - origin.position) <= 0) position = Position.Clock;

        this.nextRope = Instantiate(prefab);
        this.nextRope.SetOriginPosition(futurPosition);
        this.nextRope.lastCornerOffset = cornerOffset;
        target = this.nextRope.origin;
        OnLock?.Invoke();

        direction = mousePosition - (origin.position-(Vector3)cornerOffset);

        LockerRunning = false;
    }

    public IEnumerator Locker(Vector2 lockPosition, bool SuperLock)
    {
        LockerRunning = true;
        Lock = true;

        yield return new WaitForEndOfFrame();
        if (Vector2.SignedAngle(direction, mousePosition - origin.position) > 0) position = Position.CounterClock;
        if (Vector2.SignedAngle(direction, mousePosition - origin.position) <= 0) position = Position.Clock;

        this.nextRope = Instantiate(prefab);
        this.nextRope.SetOriginPosition(lockPosition);
        this.nextRope.lastCornerOffset = cornerOffset;
        target = this.nextRope.origin;
        OnLock?.Invoke();

        direction = mousePosition - (origin.position - (Vector3)cornerOffset);

        this.SuperLock = SuperLock;

        LockerRunning = false;
    }

    void UpdateLineRenderer()
    {
        lRenderer.SetPosition(0, (Vector2)origin.position - lastCornerOffset);
        lRenderer.SetPosition(1, (Vector2)target.position-cornerOffset);
    }

    public void SetOriginPosition(Vector3 position) => origin.position = position;

}
