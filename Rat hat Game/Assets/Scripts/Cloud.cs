using System.Collections;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float speed;
    private float lifetime;
    private Vector3 direction;
    private bool isActive = false;

    public float playerTimeLimit = 5f;
    private float playerTimer = 0f;
    private bool playerOnCloud = false;

    private SpriteRenderer spriteRenderer;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isActive){
            transform.position += direction * speed * Time.deltaTime;
            // Dies after lifetime ends
            lifetime -= Time.deltaTime;
            if (lifetime <= 0){
                Destroy(gameObject);
            }
        }
        // Dies if player is on for too long.
        if (playerOnCloud){
            playerTimer += Time.deltaTime;
            if (playerTimer >= playerTimeLimit)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetDirection(Vector3 moveDirection, float moveSpeed, float moveDuration)
    {
        direction = moveDirection;
        speed = moveSpeed;
        lifetime = moveDuration;
        isActive = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            // Starts fading the cloud the first time the player gets on. 
            if (!playerOnCloud){
                playerOnCloud = true;
                playerTimer = 0f;
                StartCoroutine(FadeCloud());
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            playerOnCloud = false;
        }
    }

    IEnumerator FadeCloud(){
        Color color = spriteRenderer.color;
        float speed = 0.08f;
        float fade = 0.05f;

        color.a = 1f;
        spriteRenderer.color = color;
        // Hides over time
        for (float a = 1f; a > 0f; a -= fade){
            color.a = a;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(speed);
        }
        // Ensures it is finally hidden
        color.a = 0f;
        spriteRenderer.color = color;
        Destroy(gameObject);
    }

}