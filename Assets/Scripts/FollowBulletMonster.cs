using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBulletMonster : BulletEnemyBase
{
    void OnTriggerEnter2D(Collider2D collision) // �߰� źȯ�� ���Ͱ� Hook�� ���� ��� �����
    {
        if (collision.CompareTag("Hook"))
        {
            Destroy(gameObject);
        }
    }
}