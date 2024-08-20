using System.Collections;
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

    // 훅이 걸리기 까지의 시간.
    public float grappleShootSpeed = 0.2f;
    // 절벽을 오르기 까지의 시간.
    public float climbSpeed = 0.1f;

    [HideInInspector] public bool isGrappling = false;

    Vector2 target;
    float grappleStartedTime = 0f;

    private void Awake()
    {
        
    }

    private void Update()
    {
        Vector3 playerPos = gameObject.transform.position;

        if (Input.GetMouseButtonDown(0)) {
            ShootGrapple();
        }

        if (Input.GetMouseButtonUp(0)) {
            isGrappling = false;
            line.enabled = false;
        }

        if (isGrappling) {
            grappleStartedTime += Time.deltaTime / grappleShootSpeed;

            line.SetPosition(0, playerPos);
            line.SetPosition(1, target);

            gameObject.transform.position = Vector3.Lerp(playerPos, target, grappleStartedTime);

            if (Vector2.Distance(playerPos, target) < 0.5f) {
                // TODO: 수정예정.
                isGrappling = false;
                line.enabled = false;
                gameObject.transform.position = playerPos + Vector3.up * 1.5f;
            }
        }
    }

    private void ShootGrapple() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = (Vector2)mousePosition - (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, platformMask);

        // Grappleable 태그를 가지고 있는 플랫폼에 닿았는지 판별.
        if (hit.collider.tag == "Grapplable") {
            target = hit.point;

            // init lineRenderer
            isGrappling = true;
            line.enabled = true;

            grappleStartedTime = 0f;
        }
    }
}