using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private int maxhealth = 3;
    private int currentHealth;
    public float speed = 5.0f;
    public float bounceAmount;
    public float moveAmount;
    private float jumpAmount = 500f;
    public float knockbackForce;
    [SerializeField] public List<Image> uiImageList;

    public bool isGrounded = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxhealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveAmount, rb.linearVelocityY);
        // Jump
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) && isGrounded){
            rb.AddForce(Vector2.up * jumpAmount);
            isGrounded = false;
        }
        //rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, new Vector2(Input.GetAxisRaw("Horizontal") * moveAmount, rb.linearVelocityY), moveAmount);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hat") || collision.gameObject.CompareTag("Platform")){
            isGrounded = true;
        }
        if(collision.gameObject.CompareTag("Hat"))
        {
            // Bounces only if parry
            //rb.AddForce(Vector2.up * bounceAmount);
        }
        if (collision.gameObject.CompareTag("Projectile"))
        {
            projectile pr = collision.gameObject.GetComponent<projectile>();
            if (!pr.checkIsDeflected())
            {
                Debug.Log(pr.checkIsDeflected());
                uiImageList[currentHealth - 1].enabled = false;
                currentHealth--;
                Vector2 dir = collision.contacts[0].normal;
                dir = new Vector2(dir.x * 10, dir.y);
                rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }

        }
    }
}
