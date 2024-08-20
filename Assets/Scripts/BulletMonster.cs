using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMonster : MonoBehaviour
{
    Transform player;
    float moveSpeed = 5f;
    public GameObject bulletObjA;
    float stopYPosition = 6f;

    public enum BulletEnemyType
    {
        BulletEnemyB,
        BulletEnemyAR,
        BulletEnemyAL
    }
    public BulletEnemyType currentBulletEnemy = BulletEnemyType.BulletEnemyB;

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
        if (currentBulletEnemy == BulletEnemyType.BulletEnemyB)
        {
            StartCoroutine(BFireDelay());
        }
        else
        {
            InvokeRepeating("FireBullet", 0f, 5f); // ÅºÈ¯Çü A ÅºÈ¯ ¹ß»ç µô·¹ÀÌ
        }
    }

    void Update()
    {
        if (currentBulletEnemy == BulletEnemyType.BulletEnemyB)
        {
            if (!isMoving) return;

            if (currentBulletEnemy == BulletEnemyType.BulletEnemyB && transform.position.y > stopYPosition)
            {
                if (!isAiming) MoveTowardsPlayer();
            }
            else
            {
                FollowPlayerX();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // ÅºÈ¯Çü ¸ó½ºÅÍ B°¡ Hook¿¡ ¸ÂÀ» °æ¿ì »ç¶óÁü
    {
        if (currentBulletEnemy == BulletEnemyType.BulletEnemyB && collision.CompareTag("Hook"))
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
        float targetX = player.position.x;
        float currentX = transform.position.x;
        float directionX = Mathf.Sign(targetX - currentX);

        rigid.velocity = new Vector2(directionX * moveSpeed, 0);
    }

    void StopMovement() // ÅºÈ¯Çü B ÀÌµ¿ ¸ØÃã
    {
        rigid.velocity = Vector2.zero;
    }

    IEnumerator BFireDelay() // ÅºÈ¯Çü B °ø°Ý
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
        if (currentBulletEnemy == BulletEnemyType.BulletEnemyB)
        {
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(Vector2.down * 10, ForceMode2D.Impulse);
        }
    }

    void ShowAimLine() // ÅºÈ¯Çü B °ø°Ý ±Ëµµ
    {
        if (currentBulletEnemy == BulletEnemyType.BulletEnemyB)
        {
            Vector3 startPoint = transform.position;
            Vector3 endPoint = startPoint + Vector3.down * 30f;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }
    }

    void FireBullet() // ÅºÈ¯Çü A ÅºÈ¯ °£°Ý
    {
        FireForward();
        Invoke("FireForward", 0.2f);
        Invoke("FireForward", 0.4f);
        Invoke("FireForward", 0.6f);
    }

    void FireForward() // ÅºÈ¯Çü A ÅºÈ¯ ¹ß»ç
    {
        GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        switch (GetComponent<BulletMonster>().currentBulletEnemy)
        {
            case BulletMonster.BulletEnemyType.BulletEnemyAL:
                rigid.AddForce(Vector2.left * 20, ForceMode2D.Impulse);
                break;
            case BulletMonster.BulletEnemyType.BulletEnemyAR:
                rigid.AddForce(Vector2.right * 20, ForceMode2D.Impulse);
                break;
        }
    }
}