using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public abstract class EntityBase : MonoBehaviour {

    Rigidbody2D rigid;
    SpriteRenderer sprite;

    public float moveSpeed = 10f;
    public float attackDamage = 1f;

    protected bool isMoveBlockedBefore = false;

    public abstract Vector2 MoveAI();

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // 기본적으로 닿으면 공격당함.
    public void PerformAttack() {
        if (this is Attackable) {
            ((Attackable)this).Attack();
        } else {
            // Default Attack logic
        }
    }

    // if you want, you can override it.
    public void SetFlipX(bool isFlip) {
        sprite.flipX = isFlip;
    }

    private void Update() {

        PerformAttack();

        Vector2 movePos = transform.position + (Vector3)MoveAI();
        if (!(this is Falldownable)) {
            RaycastHit2D hit = Physics2D.Raycast(movePos, Vector2.down, 1f, LayerMask.GetMask("Platform"));
            Debug.DrawRay(movePos, Vector2.down * 1f);
            
            if (hit.collider == null) {
                isMoveBlockedBefore = true;
                return;
            }
        }

        transform.position = movePos;
        isMoveBlockedBefore = false;
    }
}