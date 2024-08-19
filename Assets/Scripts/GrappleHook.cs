using System.Collections;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    LineRenderer line;
    Rigidbody2D rigid;
    Collider2D collider;

    [SerializeField] LayerMask grapplableMask;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float grappleSpeed = 10f;
    [SerializeField] float grappleShootSpeed = 20f;
    [SerializeField] float climbSpeed = 2f;

    public bool isGrappling { get; private set; }
    public bool retracting { get; private set; }
    public bool climbing { get; private set; }

    Vector2 target;
    Coroutine grappleCoroutine;
    float originGravity;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        originGravity = rigid.gravityScale;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isGrappling && !climbing)
        {
            StartGrapple();
        }

        if (Input.GetMouseButtonUp(0) && !climbing)
        {
            CancelGrapple();
        }

        if (retracting)
        {
            HandleGrappleRetract();
        }
        else if (climbing)
        {
            HandleClimbing();
        }
        else
        {
            rigid.gravityScale = originGravity;
        }
    }

    private void StartGrapple()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (Vector2)mousePosition - (Vector2)transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, grapplableMask);

        if (hit.collider != null)
        {
            isGrappling = true;
            target = hit.point;
            line.enabled = true;
            line.positionCount = 2;

            if (grappleCoroutine != null)
            {
                StopCoroutine(grappleCoroutine);
            }

            grappleCoroutine = StartCoroutine(Grapple());
        }
    }

    private IEnumerator Grapple()
    {
        float t = 0;
        float duration = 1f / grappleShootSpeed;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        while (t < duration)
        {
            t += Time.deltaTime;
            line.SetPosition(1, Vector2.Lerp(transform.position, target, t / duration));
            yield return null;
        }

        line.SetPosition(1, target);
        retracting = true;
    }

    private void HandleGrappleRetract()
    {
        rigid.gravityScale = 0;
        transform.position = Vector2.Lerp(transform.position, target, grappleSpeed * Time.deltaTime);
        line.SetPosition(0, transform.position);

        if (Vector2.Distance(transform.position, target) < 0.5f)
        {
            retracting = false;
            climbing = true;
            collider.isTrigger = true;
            line.enabled = false;
        }
    }

    private void HandleClimbing()
    {
        Vector2 targetPos = target + Vector2.up * 2f;
        transform.position = Vector2.Lerp(transform.position, targetPos, climbSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            transform.position = targetPos;
            climbing = false;
            rigid.gravityScale = originGravity;
            collider.isTrigger = false;

            CancelGrapple();
        }
    }

    private void CancelGrapple()
    {
        if (isGrappling || climbing)
        {
            isGrappling = false;
            retracting = false;
            climbing = false;

            if (grappleCoroutine != null)
            {
                StopCoroutine(grappleCoroutine);
            }

            line.enabled = false;
            rigid.gravityScale = originGravity;
            collider.isTrigger = false;
        }
    }
}