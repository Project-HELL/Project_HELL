using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BulletEnemyBase : EntityBase, Attackable, Falldownable
{
    float timer = 0.0f;
    float offsetTime = 1.8f;
    public GameObject bulletObject;
    protected Transform player;
    public void Attack()
    {
        System.Random rand = new System.Random(); // 탄환 발사주기 랜덤으로 설정
        timer += Time.deltaTime * (float)rand.NextDouble();
        if (timer > offsetTime) // 탄환 발사주기마다 탄환 생성
        {
            GameObject bullet = Instantiate(bulletObject, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            timer = 0;
        }
    }
}