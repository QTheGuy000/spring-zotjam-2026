using System.Collections;
using UnityEngine;

public class Spoon: MonoBehaviour
{
    // Radius around player.
    private float radius;
    // Used to get mouse pos.
    private Camera mainCamera;

    private Collider2D spoonCollider;
    private SpriteRenderer spriteRenderer;
    private projectile touchingProjectile = null;
    private Transform player;

    private bool isSwinging = false;
    public float swingCooldown = 0.5f;

    private bool touchingHat = false;

    void Start()
    {
        mainCamera = Camera.main;
        spoonCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = transform.parent;

        // We can change the radius by moving where the spoon is relative from the player. 
        radius = Vector2.Distance(transform.localPosition, Vector2.zero);
    }

    void Update()
    {
        RotateAroundPlayer();

        // Mouse clicked.
        if (Input.GetMouseButtonDown(0) && !isSwinging){
            StartCoroutine(SwingSpoon());
        }
    }

    // Rotates Spoon around player.
    void RotateAroundPlayer()
    { 
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - player.position).normalized;
        transform.position = player.position + direction * radius;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Subtracted by 90 so that the spoon looks upwards.
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }

    // Swings the Spoon.
    IEnumerator SwingSpoon()
    {
        isSwinging = true;
        Debug.Log("Swing");
        spriteRenderer.color = Color.yellow;
        // Deflects Projectile (if exists).
        if (touchingProjectile != null){
            // Sets the direction of the bullet away from the spoon's direction. 
            Vector2 spoonDirection = (transform.position - player.position).normalized;
            Vector2 deflectDirection = transform.up;
            touchingProjectile.Deflect(spoonDirection);
            Debug.Log("Deflect!");
        }
        // Bounce off hat.
        if (touchingHat){
            Vector2 spoonDirection = (transform.position - player.position).normalized;
            player.GetComponent<Rigidbody2D>().AddForce(-spoonDirection * 500);
            Debug.Log("Bounce!");
            touchingHat = false;
        }
        // Waits before letting next swing. 
        yield return new WaitForSeconds(swingCooldown);
        isSwinging = false;
        spriteRenderer.color = Color.white;

    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile")){
            touchingProjectile = other.GetComponent<projectile>();
        }
        if (other.CompareTag("Hat")){
            touchingHat = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Projectile")){
            touchingProjectile = null;
        }
        if (other.CompareTag("Hat")){
            touchingHat = false;
        }
    }

    public bool checkIfSwinging()
    {
        return isSwinging;
    }
}