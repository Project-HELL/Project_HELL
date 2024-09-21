using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float hp;
    bool isHurt = false; // 피격 상태를 나타냄
    public float invincibleDuration = 2.0f; // 무적 상태 유지 시간 (2초)

    SpriteRenderer sr;
    Color halfA = new Color(1, 1, 1, 0.5f); // 투명하게 깜빡이는 효과를 위한 색상
    Color fullA = new Color(1, 1, 1, 1); // 원래 색상
    public float speed;
    bool isKnockback = false;

    void Start()
    {
        // SpriteRenderer 초기화
        sr = GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            Debug.LogError("SpriteRenderer가 오브젝트에 없습니다!");
        }
    }

    public void Damage(float damage, Vector2 pos)
    {
        // 무적 상태가 아니라면 데미지 적용
        if (!isHurt)
        {
            Debug.Log("플레이어가 피해를 입었습니다.");
            isHurt = true; // 피격 상태로 설정
            hp -= damage;  // 체력 감소

            Debug.Log("현재 체력: " + hp);

            if (hp <= 0)
            {
                Debug.Log("플레이어의 체력이 다 소진되었습니다.");
            }
            else
            {
                // 넉백 방향 설정
                float x = transform.position.x - pos.x;
                x = (x < 0) ? 1 : -1;

                Debug.Log("넉백 방향: " + x);
                StartCoroutine(Knockback(x));  // 넉백 코루틴 시작
                StartCoroutine(HurtRoutine()); // 무적 상태 유지 코루틴 시작
                StartCoroutine(alphablink());  // 피격 시 깜빡이는 효과
            }
        }
    }

    IEnumerator Knockback(float dir)
    {
        isKnockback = true;
        float ctime = 0;
        while (ctime < 0.2f) // 0.2초 동안 넉백 효과
        {
            if (transform.rotation.y == 0)
                transform.Translate(Vector2.left * speed * Time.deltaTime * dir);
            else
                transform.Translate(Vector2.left * speed * Time.deltaTime * -1f * dir);

            ctime += Time.deltaTime;
            yield return null;
        }
        isKnockback = false; // 넉백 상태 해제
    }

    IEnumerator alphablink()
    {
        while (isHurt) // 무적 상태 동안 깜빡임
        {
            yield return new WaitForSeconds(0.1f);
            sr.color = halfA; // 반투명 상태
            yield return new WaitForSeconds(0.1f);
            sr.color = fullA; // 원래 상태로 복귀
        }
    }

    // 무적 상태 유지 코루틴
    IEnumerator HurtRoutine()
    {
        // invincibleDuration(무적 상태 시간) 동안 무적 유지
        yield return new WaitForSeconds(invincibleDuration);
        isHurt = false; // 무적 상태 해제
    }
}