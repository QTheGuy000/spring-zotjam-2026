using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5.0f;
    public float bounceAmount;
    public float moveAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
         rb.linearVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveAmount, rb.linearVelocityY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Hat"))
        {
            rb.AddForce(Vector2.up * bounceAmount);
        }
    }
}
