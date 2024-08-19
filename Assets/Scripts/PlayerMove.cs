using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public float minX = -9.1f;
    public float maxX = 9f;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private GrappleHook grappleHook;
    private Vector2 velocity = Vector2.zero;
    private float horizontalInput;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        grappleHook = GetComponent<GrappleHook>();
    }

    void Update()
    {
        if (grappleHook.isGrappling)
        {
            anim.SetBool("isRunning", false);
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        if (horizontalInput != 0)
        {
            spriteRenderer.flipX = horizontalInput > 0;
        }
        anim.SetBool("isRunning", Mathf.Abs(horizontalInput) > 0);
    }

    void FixedUpdate()
    {
        if (grappleHook.isGrappling) return;

        Vector2 targetVelocity = new Vector2(horizontalInput * maxSpeed, rigid.velocity.y);
        rigid.velocity = Vector2.SmoothDamp(rigid.velocity, targetVelocity, ref velocity, 0.1f);

        Vector2 clampedPosition = new Vector2(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y);
        transform.position = clampedPosition;

        if (rigid.velocity.y < 0 && IsGrounded())
        {
            anim.SetBool("isJumping", false);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 1f, LayerMask.GetMask("Grapplable", "Platform"));
        return rayHit.collider != null;
    }
}