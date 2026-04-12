using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class Spoon: MonoBehaviour
{
    private float radius;
    private Camera mainCamera;

    private Collider2D spoonCollider;
    private SpriteRenderer spriteRenderer;
    private projectile touchingProjectile = null;
    private Transform player;

    private bool isSwinging = false;
    public float swingCooldown = 0.5f;
    public float swingFrameDuration = 0.1f; // How long each swing frame lasts

    private GameObject touchingObject = null;

    public Sprite idle;
    public Sprite leftSwing1;
    public Sprite leftSwing2;
    public Sprite rightSwing1;
    public Sprite rightSwing2;

    [SerializeField] AudioClip[] _list_of_hits;
    [SerializeField] AudioClip[] _list_of_misses;

    [SerializeField] AudioSource _hit_source;
    [SerializeField] AudioSource _miss_source;


    void Start()
    {
        mainCamera = Camera.main;
        spoonCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = transform.parent;

        radius = Vector2.Distance(transform.localPosition, Vector2.zero);
    }

    void Update()
    {
        RotateAroundPlayer();

        if (Input.GetMouseButtonDown(0) && !isSwinging){

            if (touchingObject == null && touchingProjectile == null)
            {
                playMiss();
            }

            // Determine swing direction at moment of click
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            bool isLeftSwing = mouseWorldPos.x < player.position.x;
            StartCoroutine(SwingSpoon(isLeftSwing));
        }
        //Debug.Log(isSwinging);
    }

    void RotateAroundPlayer()
    { 
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - player.position).normalized;
        transform.position = player.position + direction * radius;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }

    IEnumerator SwingSpoon(bool isLeftSwing)
    {
        isSwinging = true;

        // Play swing animation based on direction
        if (isLeftSwing){
            Debug.Log("LSwing");
            spriteRenderer.enabled = false;
            sprite.GetComponent<spoonSprite>().LeftSwing();
            Debug.Log("AFTER");
            /*spriteRenderer.sprite = leftSwing1;
            yield return new WaitForSeconds(swingFrameDuration);
            spriteRenderer.sprite = leftSwing2;
        }
        else{

            Debug.Log("LSwing");
            spriteRenderer.enabled = false;
            sprite.GetComponent<spoonSprite>().RightSwing();
            Debug.Log("AFTER");
            /*spriteRenderer.sprite = rightSwing1;
            yield return new WaitForSeconds(swingFrameDuration);
            spriteRenderer.sprite = rightSwing2;
        }

        // Hit detection at peak of swing
        if (touchingProjectile != null){
            Vector2 spoonDirection = (transform.position - player.position).normalized;
            touchingProjectile.Deflect(spoonDirection);

            if (touchingProjectile.gameObject.layer == LayerMask.NameToLayer("Artemis Projectile")) 
            {
                touchingProjectile.gameObject.layer = LayerMask.NameToLayer("Projectile");
            }

            playHit();

            Debug.Log("Deflect!");
        }
        if (touchingObject != null){
            if (touchingObject.CompareTag("Enemy")){
                touchingObject.GetComponent<enemy>().DecreaseHealth();
            }

            Vector2 spoonDirection = (transform.position - player.position).normalized;
            player.GetComponent<Rigidbody2D>().AddForce(-spoonDirection * 500);

            playHit();

            Debug.Log("Bounce!");
            touchingObject = null;
        }


        // Hold swing2 for remaining cooldown, then return to idle
        yield return new WaitForSeconds(swingCooldown - swingFrameDuration);
        //spriteRenderer.sprite = idle;

        isSwinging = false;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile")){
            touchingProjectile = other.GetComponent<projectile>();
        }
        if (other.CompareTag("Hat") || other.CompareTag("Platform") || other.CompareTag("Enemy")){
            touchingObject = other.gameObject;
        }


    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Projectile")){
            touchingProjectile = null;
        }
        if (other.CompareTag("Hat") && other.CompareTag("Platform") && other.CompareTag("Enemy")){
            touchingObject = null;
        }
    }

    public bool checkIfSwinging()
    {
        return isSwinging;
    }

    public void returnSprite()
    {
        spriteRenderer.sprite = idle;
        Debug.Log("ReturnSprite");
        spriteRenderer.enabled = true;
    }
    void playHit()
    {
        _hit_source.clip = _list_of_hits[Random.Range(0, _list_of_hits.Count())];
        _hit_source.Play();
    }

    void playMiss()
    {
        _miss_source.clip = _list_of_misses[Random.Range(0, _list_of_misses.Count())];
        _miss_source.Play();
    }

}