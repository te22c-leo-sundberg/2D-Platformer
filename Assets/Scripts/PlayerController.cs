using System.Collections;
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
    bool canJump = true;
    [SerializeField] public float dashBoostTime;
    bool isFacingRight = true;
    int dashDirection = 0;
    private float xMove;
    bool canDash = true;
    bool isDashing;
    public bool IsDashBoosted = false;
    [SerializeField] float dashPower = 24f;
    [SerializeField] float dashTime = 0.2f;

    float originalDashTime;
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
        originalDashTime = dashTime;
    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        xMove = Input.GetAxisRaw("Horizontal");

        // rb.velocity = new(
        //     xMove * speed,
        //     rb.velocity.y
        // );
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
            rb.AddForce(Vector2.up * jumpForce);
            canJump = false;
        }
        if (Input.GetAxisRaw("Jump") == 0)
        {
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (IsDashBoosted)
        {
            Timers();
            DashBooster();
        }
        else
        {}
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
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(dashDirection * dashPower, 0f);
        GetComponent<SpriteRenderer>().sprite = DashSlimeSprite;
        yield return new WaitForSeconds(dashTime);
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
            print("I dash further!");
        }
        else if (dashBoostTime <= 0)
        {
            dashTime = originalDashTime;
            print("I stopped dashing further!");
            IsDashBoosted = false;
            dashBoostTime = 0.5f;
        }
    }
}
