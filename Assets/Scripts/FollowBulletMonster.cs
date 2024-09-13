using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBulletMonster : BulletEnemyBase
{
    void OnTriggerEnter2D(Collider2D collision) // 추격 탄환형 몬스터가 Hook에 맞을 경우 사라짐
    {
        if (collision.CompareTag("Hook"))
        {
            Destroy(gameObject);
        }
    }
}