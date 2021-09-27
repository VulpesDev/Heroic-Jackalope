using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer : MonoBehaviour
{
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        baseSpeed = speed;

        additionalJumps = defaultAdditionalJumps;
    }

    void Update()
    {
        Move();
        Jump();
        BetterJump();
        CheckIfGrounded();
        DashCheck();
    }

    [Header("Movement Settings")]
    public float speed;


    void Move() 
    {
        float x = Input.GetAxisRaw("Horizontal");

        float moveBy = x * speed;

        rb.velocity = new Vector2(moveBy, rb.velocity.y);
    }

    [Space(10)]
    [Header("Jump Settings")]
    public float jumpForce;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public float rememberGroundedFor;
    float lastTimeGrounded;

    public int defaultAdditionalJumps = 1;
    int additionalJumps;


    void Jump() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - lastTimeGrounded <=
            rememberGroundedFor || additionalJumps > 0)) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            additionalJumps--;
        }
    }

    void BetterJump() 
    {
        if (rb.velocity.y < 0) 
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        } 
        else if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space)) 
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }   
    }

    void CheckIfGrounded() 
    {
        Collider2D colliders = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius,
            groundLayer);

        if (colliders != null) 
        {
            isGrounded = true;
            additionalJumps = defaultAdditionalJumps;
        } 
        else 
        {
            if (isGrounded) 
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }
    }
    [Space(10)]
    [Header("Dash Settings")]
    public float multiplier;
    public float duration, baseSpeed, delay;
    bool isDashing = false, canDash = true;
    void DashCheck()
    {
        float fire1 = Input.GetAxisRaw("Fire1");
        if(fire1 >= 0.1f && canDash)
        {
            DashStart();
        }
    }
    void DashStart()
    {
        isDashing = true;
        canDash = false;
        speed *= multiplier;
        Invoke("DashStop", duration);
    }
    void DashStop()
    {
        speed = baseSpeed;
        isDashing = false;
        Invoke("DashDelay", delay);
    }
    void DashDelay()
    {
        canDash = true;
    }


}
