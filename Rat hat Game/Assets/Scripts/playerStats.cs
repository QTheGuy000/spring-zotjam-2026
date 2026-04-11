using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class playerStats : MonoBehaviour
{
    private Rigidbody2D rb;
    public float knockbackForce;
    [SerializeField] private int maxhealth = 3;
    private int currentHealth;
    [SerializeField] public List<Image> uiImageList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxhealth;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            projectile pr = collision.gameObject.GetComponent<projectile>();
            if (!pr.checkIsDeflected())
            {
                Debug.Log(pr.checkIsDeflected());
                decreaseHealth();
                Vector2 dir = collision.contacts[0].normal;
                dir = new Vector2(dir.x * 10, dir.y);
                rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }

        }
    }
    public void decreaseHealth(int health = 1)
    {
        uiImageList[currentHealth - 1].enabled = false;
        currentHealth--;
    }
}
