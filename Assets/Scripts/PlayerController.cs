using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 7;
    float movespeed = 5;
    [SerializeField]
    float jumpForce = 10;
    bool canJump = true;

    bool isFacingRight = true;
    private float xMove;

    bool canDash = true;
    bool isDashing;
    [SerializeField] float dashPower = 24f;
    [SerializeField] float dashTime = 0.2f;
    [SerializeField] float dashCooldown = 1f;

    [SerializeField]
    Transform groundChecker;

    [SerializeField]
    LayerMask groundLayer;
    [SerializeField] Rigidbody2D rb;

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
        }
        if (rb.velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
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
        Flip();
    }
    private void Flip()
    {
        if (isFacingRight && xMove < 0f || !isFacingRight && xMove > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
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
    private IEnumerator Dash() //make rythm game dash? every x seconds your character blinks, dash close to that timeframe and get a stronger dash
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
