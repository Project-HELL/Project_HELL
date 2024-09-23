using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class FollowBulletEnemy : BulletEnemyBase
{
    public float fleeDistance; // 플레이어와의 최소 거리
    public float smoothing; // 부드러운 이동을 위한 보간값
    public override Vector2 MoveAI()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>(); // 플레이어 찾기

        float distanceX = transform.position.x - player.position.x; // 플레이어와 거리 계산
        float distanceY = transform.position.y - player.position.y;

        Vector2 moveDirection = Vector2.left * distanceX * moveSpeed * Time.deltaTime; // x축 방향으로 이동
        if (player != null)
        {
            if (Mathf.Abs(distanceY) < fleeDistance) // y축으로 플레이어가 가까워지면 위로 멀어지면 아래로 이동
            {
                moveDirection += Vector2.up * moveSpeed * Time.deltaTime;
            }
            else if (distanceY > fleeDistance)
            {
                moveDirection += Vector2.down * moveSpeed * Time.deltaTime;
            }
            Vector2 newPosition = Vector2.Lerp(transform.position, (Vector2)transform.position + moveDirection, smoothing * Time.deltaTime); // 부드러운 이동
            return newPosition - (Vector2)transform.position;
        }
        return Vector2.zero;
    }
}
