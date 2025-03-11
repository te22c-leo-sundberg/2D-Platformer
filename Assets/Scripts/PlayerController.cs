using System.Collections;
using Unity.Mathematics;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 7;
    // float movespeed = 5;
    [SerializeField]
    float jumpForce = 10;
    float originalJumpForce;
    [SerializeField] float jumpBoostTime;
    public bool isJumpBoosted = false;
    bool canJump = true;
    [SerializeField] public float dashBoostTime;
    bool isFacingRight = true;
    int dashDirection = 0;
    private float xMove;
    bool canDash = true;
    bool isDashing;
    public bool isDashBoosted = false;
    public bool freezeRotation;
    [SerializeField] float dashPower = 24f;
    [SerializeField] float dashTime = 0.2f;

    float originalDashTime;
    float originalGravity;
    [SerializeField] float dashCooldown = 1f;

    [SerializeField]
    Transform groundChecker;

    [SerializeField]
    LayerMask groundLayer;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Sprite SlimeSprite;
    [SerializeField] Sprite DashSlimeSprite;
    public void Start()
    {
        originalGravity = rb.gravityScale;
        originalDashTime = dashTime;
        originalJumpForce = jumpForce;
    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        xMove = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(xMove * speed, rb.velocity.y);

        if (rb.velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            isFacingRight = false;
        }
        if (rb.velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            isFacingRight = true;
        }

        if (Input.GetAxisRaw("Jump") > 0 && canJump && IsGrounded())
        {
            Jump();
        }
        if (Input.GetAxisRaw("Jump") == 0)
        {
            StopJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (isDashBoosted)
        {
            dashBoostTime -= Time.deltaTime;
            DashBooster();
        }
        if (isJumpBoosted)
        {
            jumpBoostTime -= Time.deltaTime;
            JumpBooster();
        }
    }

    private bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundChecker.position, .2f, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Jump()
    {
        canJump = false;
        rb.transform.rotation = quaternion.identity;
        rb.AddForce(Vector2.up * jumpForce);
        rb.freezeRotation = true;
    }
    private void StopJump()
    {
        rb.freezeRotation = false;
        canJump = true;
    }
    private IEnumerator Dash() //make rythm game dash? every x seconds your character blinks, dash close to that timeframe and get a stronger dash
    {
        if (isFacingRight)
        {
            dashDirection = 1;
        }
        else if (!isFacingRight)
        {
            dashDirection = -1;
        }
        canDash = false;
        isDashing = true;
        rb.transform.rotation = quaternion.identity;
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.velocity = new Vector2(dashDirection * dashPower, 0f);
        GetComponent<SpriteRenderer>().sprite = DashSlimeSprite;
        yield return new WaitForSeconds(dashTime);
        rb.freezeRotation = false;
        rb.gravityScale = originalGravity;
        GetComponent<SpriteRenderer>().sprite = SlimeSprite;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    public void Timers()
    {
        dashBoostTime -= Time.deltaTime;
    }
    public void DashBooster()
    {
        if (dashBoostTime > 0)
        {
            dashTime = 0.2f;
        }
        else if (dashBoostTime <= 0)
        {
            dashTime = originalDashTime;
            isDashBoosted = false;
            dashBoostTime = 0.2f;
        }
    }
    public void JumpBooster()
    {
        if (jumpBoostTime > 0)
        {
            if (Input.GetAxisRaw("Jump") > 0 && canJump)
            {
                rb.velocity = new Vector2 (0, 0);
                Jump();
            }
        }
        else if (jumpBoostTime <= 0)
        {
            isJumpBoosted = false;
            jumpBoostTime = 0.2f;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "DashBoost")
        {
            isDashBoosted = true;
        }
        if (other.gameObject.tag == "JumpBoost")
        {
            isJumpBoosted = true;
        }
    }
}
