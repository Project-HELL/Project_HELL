using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyBase : EntityBase, Attackable, Falldownable
{
    float timer = 0.0f;
    float offsetTime = 1.8f;
    public GameObject BulletObj;
    Transform Player;
    public override Vector2 MoveAI()
    {
        Player = GameObject.Find("Player").GetComponent<Transform>();
        if (Player != null)
        {
            float targetX = transform.position.x - Player.position.x;
            Vector3 targetPosition = Vector3.left * targetX * moveSpeed;
            return targetPosition * Time.deltaTime;
        }
        return Vector2.zero;
    }
    public void Attack()
    {
        timer += Time.deltaTime;

        if (timer > offsetTime)
        {
            GameObject bullet = Instantiate(BulletObj, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            timer = 0;
        }
    }
}
