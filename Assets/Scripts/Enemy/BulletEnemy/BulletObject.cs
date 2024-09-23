using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletObject : MonoBehaviour
{
    public float bulletSpeed = 10; // 탄환 속도
    Transform playerPos;
    Vector2 targetPos;
    void Awake() // 플레이어에게 날아감
    {
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        targetPos = playerPos.position - transform.position;
        GetComponent<Rigidbody2D>().velocity = (targetPos.normalized * bulletSpeed);
    }
    void OnBecameInvisible() // 필요없는 탄환 삭제
    {
        Destroy(this.gameObject);
    }
}