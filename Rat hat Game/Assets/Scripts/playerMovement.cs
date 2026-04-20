using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.Mathematics;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5.0f;
    public float bounceAmount;
    public float moveAmount;
    public float dashAmount;
    public float jumpVelocity = 10f;
    public float doubleJumpVelocity = 5f;
    playerStats pStats;

    private bool fell = false;
    public bool isGrounded = true;
    private bool doubleJump = true;
    private bool dashReady = true;
    private float dashDecay = 0.99f;
    Vector2 moveVelocity;
    Vector2 dashVelocity;

    public Vector2 knockback_additive;
    public float seconds_of_knockback;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pStats = GetComponent<playerStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.instance.isTransitioning){
            Vector2 origin = LevelManager.instance.origin.position;
            float distanceToOrigin = Vector2.Distance(transform.position, origin);

            if (distanceToOrigin > 0.1f){
                // Move towards origin
                Vector2 direction = (origin - (Vector2)transform.position).normalized;
                rb.linearVelocity = direction * speed;
            }
            else{
                // Stop at origin
                rb.linearVelocity = Vector2.zero;
                transform.position = origin; // Snap exactly to origin
            }
            return;
        }

        moveVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveAmount, rb.linearVelocityY);
        // Changes facing direction.
        if (moveVelocity.x > 0){
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveVelocity.x < 0){
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && Input.GetAxisRaw("Horizontal") != 0 && dashReady)
        {
            dashVelocity = new Vector2(dashAmount * Input.GetAxisRaw("Horizontal"), 0);
            dashReady = false;
            Debug.Log("Dash");
        }
        if(Mathf.Abs(dashVelocity.x) > 0.1)
        {
            dashVelocity = new Vector2(dashVelocity.x * dashDecay, 0);
        }
        else
        {
            dashVelocity = new Vector2(0, 0);
        }
        // Debug.Log(rb.linearVelocityX);
        if (moveVelocity.x != 0)
        {
            rb.linearVelocity = moveVelocity + dashVelocity;
        }
        else
        {
            rb.linearVelocityX = 0;
        }
        // Jump
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)){
            if (isGrounded)
            {
                rb.linearVelocityY = jumpVelocity;
                isGrounded = false;
            }
            else if(doubleJump && rb.linearVelocityY < 0)
            {
                rb.linearVelocityY = doubleJumpVelocity;
                doubleJump = false;
            }
        }
        if(0 > Camera.main.WorldToViewportPoint(transform.position).y && !fell)
        {
            fell = true;
            pStats.DecreaseHealth();
            StartCoroutine(BounceBackCoroutine());

        }
        if (0 < Camera.main.WorldToViewportPoint(transform.position).y && fell)
        {
            fell = false;
        }
        if (seconds_of_knockback > 0)
        {
            seconds_of_knockback -= Time.deltaTime;
            rb.linearVelocity += knockback_additive;
        }


        //rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, new Vector2(Input.GetAxisRaw("Horizontal") * moveAmount, rb.linearVelocityY), moveAmount);
    }

    IEnumerator BounceBackCoroutine()
    {
        float gravityScale = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(1);
        rb.gravityScale = gravityScale;
        rb.linearVelocityY = jumpVelocity * 2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hat") || collision.gameObject.CompareTag("Platform")){
            isGrounded = true;
            doubleJump = true;
            dashReady = true;
        }
        if(collision.gameObject.CompareTag("Hat"))
        {
            // Bounces only if parry
            //rb.AddForce(Vector2.up * bounceAmount);
        }
    }
}
