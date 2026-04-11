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
    [SerializeField] public List<Image> uiImageList;
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Hat"))
        {
            rb.AddForce(Vector2.up * bounceAmount);
        }
        if (collision.gameObject.CompareTag("Projectile"))
        {
            uiImageList[currentHealth - 1].enabled = false;
            currentHealth--;

        }
    }
}
