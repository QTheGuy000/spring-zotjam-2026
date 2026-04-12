using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class playerStats : MonoBehaviour
{
    private Rigidbody2D rb;
    public float knockbackForce;
    [SerializeField] private int maxhealth = 3;
    private int currentHealth;

    public GameObject heartContainer;
    private List<Image> heartImages = new();
    private Color spriteColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        currentHealth = maxhealth;
        // Adds heart images from container
        foreach (Transform child in heartContainer.transform){
            heartImages.Add(child.GetComponent<Image>());
        }

        spriteColor = GetComponent<SpriteRenderer>().color;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Projectile causes damage
        if (collision.gameObject.CompareTag("Projectile"))
        {
            projectile pr = collision.gameObject.GetComponent<projectile>();
            if (!pr.checkIsDeflected())
            {
                Debug.Log(pr.checkIsDeflected());
                DecreaseHealth();
                Vector2 dir = collision.contacts[0].normal;
                dir = new Vector2(dir.x * 10, dir.y);
                rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }

        }
    }

    public void DecreaseHealth(int health = 1)
    {
        if (currentHealth > 0){
            heartImages[currentHealth - 1].enabled = false;
            currentHealth--;
            StartCoroutine(TakeDamage());
        }

        if (currentHealth <= 0){
            gameObject.SetActive(false);
            Die();
        }
    }

    public void Die(){
        LevelManager.instance.GameOver();
    }

    public virtual IEnumerator TakeDamage(){
        // Flashes damage twice
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
    }
}
