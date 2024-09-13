using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObj : MonoBehaviour
{
    public float bulletSpeed = 10;
    Transform playerPos;
    Vector2 dir;
    void Awake() // 플레이어에게 날아감
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        dir = playerPos.position - transform.position;
        GetComponent<Rigidbody2D> ().velocity = (dir.normalized * bulletSpeed);
    }
}