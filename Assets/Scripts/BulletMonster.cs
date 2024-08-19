using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform player;
    float moveSpeed = 5f;
    public string enemyName;
    public GameObject bulletObjA;
    public float speed;
    float stopYPosition = 6f;

    bool isAiming = false;
    bool isMoving = true;

    Rigidbody2D rigid;
    LineRenderer lineRenderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        lineRenderer.sortingLayerName = "Background";
        lineRenderer.sortingOrder = -1;
    }

    void Start()
    {
        if (enemyName == "B")
        {
            StartCoroutine(BFireDelay());
        }
        else
        {
            StartCoroutine(AFireDelay());
        }
    }

    void Update()
    {
        if (enemyName == "B")
        {
            if (isMoving)
            {
                if (transform.position.y > stopYPosition)
                {
                    if (isMoving && !isAiming)
                    {
                        MoveTowardsPlayer();
                    }
                }
                else
                {
                    FollowPlayerX();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // 탄환형 몬스터 B가 Hook에 맞을 경우 사라짐
    {
        if (enemyName == "B" && collision.CompareTag("Hook"))
        {
            Destroy(gameObject);
        }
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            rigid.velocity = new Vector2(direction.x * moveSpeed, -moveSpeed);
        }
    }

    void FollowPlayerX()
    {
        if (player != null)
        {
            float targetX = player.position.x;
            float currentX = transform.position.x;
            float directionX = Mathf.Sign(targetX - currentX);

            rigid.velocity = new Vector2(directionX * moveSpeed, 0);
        }
    }

    void StopMovement() // 탄환형 몬스터 B 이동 멈춤
    {
        rigid.velocity = Vector2.zero;
    }

    IEnumerator BFireDelay() // 탄환형 몬스터 B 공격
    {
        while (true)
        {
            isMoving = false;
            StopMovement();

            isAiming = true;
            ShowAimLine();
            lineRenderer.enabled = true;

            yield return new WaitForSeconds(1.5f);

            lineRenderer.enabled = false;
            Fire();

            yield return new WaitForSeconds(0.5f);

            isAiming = false;
            isMoving = true;

            yield return new WaitForSeconds(3.0f);
        }
    }

    void Fire()
    {
        if (enemyName == "B")
        {
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(Vector2.down * 10, ForceMode2D.Impulse);
        }
    }

    void ShowAimLine() // 탄환형 몬스터 B 공격 궤도
    {
        if (enemyName == "B")
        {
            Vector3 startPoint = transform.position;
            Vector3 endPoint = startPoint + Vector3.down * 30f;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }
    }

    IEnumerator AFireDelay() // 탄환형 몬스터 A 공격
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);

            Invoke("FireForward", 0.5f);
            Invoke("FireForward", 0.7f);
            Invoke("FireForward", 0.9f);
            Invoke("FireForward", 1.1f);
        }
    }

    void FireForward() // 전방으로 4발 발사
    {
        GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        switch (enemyName)
        {
            case "AL":
                rigid.AddForce(Vector2.left * 20, ForceMode2D.Impulse);
                break;
            case "AR":
                rigid.AddForce(Vector2.right * 20, ForceMode2D.Impulse);
                break;
        }
    }
}