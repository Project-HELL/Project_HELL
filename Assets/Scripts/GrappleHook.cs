using System;
using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    
    [Header("General Refernces:")]
    [SerializeField] private LineRenderer line;
    [SerializeField] LayerMask platformMask;
    [SerializeField] 
    
    [Header("General Settings:")]
    // 훅의 최대거리.
    public float maxDistance = 10f;
    // 훅을 타고 이동하는 속도.
    public float grappleMoveSpeed = 1f;

    // 최대 속도
    public float grappleMaxSpeed = 10f;

    // 훅이 걸리기 까지의 시간.
    public float grappleShootSpeed = 0.2f;
    public float animSpeed = 0.25f;

    public AnimationCurve grappleFailAnim = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [HideInInspector] public bool isGrappling = false;
    [HideInInspector] public bool canGrapple = true;


    Rigidbody2D rigid;
    float originGravity;

    Vector2 target;
    float grappleStartedTime = 0f;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        originGravity = rigid.gravityScale;
    }

    private void Update()
    {
        Vector3 playerPos = gameObject.transform.position;

        if (canGrapple && Input.GetMouseButtonDown(0)) {
            ShootGrapple();
        }

        if (Input.GetMouseButtonUp(0)) {
            isGrappling = false;
        }

        if (!isGrappling && canGrapple) {
            line.enabled = false;
        }

        if (isGrappling) {
            grappleStartedTime += Time.deltaTime * grappleMoveSpeed;
            rigid.gravityScale = 0;

            line.SetPosition(0, playerPos);
            line.SetPosition(1, target);

            // gameObject.transform.position = Vector3.Lerp(playerPos, target, grappleStartedTime);
            // rigid.velocity = (target - (Vector2)playerPos).normalized * (/*Vector2.Distance(playerPos, target) * */grappleMoveSpeed + 1);
            rigid.AddForce((target - (Vector2)playerPos).normalized * grappleMoveSpeed * (grappleStartedTime + 1));
            rigid.velocity = Vector2.Min(rigid.velocity, Vector2.one * grappleMaxSpeed);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.5f, platformMask);
            Debug.DrawRay(transform.position, Vector2.up * 0.5f);
            //Debug.Log(hit.collider);
            if (hit.collider != null) {
                isGrappling = false;
                line.enabled = false;
                gameObject.transform.position = playerPos + Vector3.up * 1.5f;
            }

            // if (Vector2.Distance(playerPos, target) < 0.5f) {
            //     isGrappling = false;
            //     line.enabled = false;
            //     gameObject.transform.position = playerPos + Vector3.up * 1.5f;
            // }
        } else {
            rigid.gravityScale = originGravity;
        }
    }

    private void ShootGrapple() {


        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = (Vector2)mousePosition - (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, platformMask);

        Debug.Log(hit.collider);

        // Grappleable 태그를 가지고 있는 플랫폼에 닿았는지 판별.
        if (hit.collider != null && hit.collider.tag == "Grapplable") {
            target = hit.point;

            // init lineRenderer
            isGrappling = true;
            line.enabled = true;

            grappleStartedTime = 0f;
        } else {
            if (hit.collider != null) {
                target = hit.point;
            } else {
                target = (Vector2)transform.position + (direction.normalized * Math.Min(maxDistance, direction.magnitude));
            }

            Debug.Log(target);
            StartCoroutine(GrappleShotAnimation(target));
        }
    }

    IEnumerator GrappleShotAnimation(Vector2 point) {
        canGrapple = false;

        float time = 0;
        line.enabled = true;
        
        while (time < animSpeed) {
            yield return null;
            Vector2 playerPos = gameObject.transform.position;

            line.SetPosition(0, playerPos);
            line.SetPosition(1, Vector2.Lerp(playerPos, target, grappleFailAnim.Evaluate(time / animSpeed)));

            time += Time.deltaTime;
        }

        canGrapple = true;
        line.enabled = false;
    }
}