using UnityEngine;


public class projectile : MonoBehaviour
{
    [SerializeField] float _lifespan = 6;
    [SerializeField] float _movement_speed = 4;
    [SerializeField] float _min_movement_speed = 2;
    [SerializeField] float _seconds_of_homing_time;
    [SerializeField] bool _homing;
    private bool isDeflected = false;

    [SerializeField] Rigidbody2D _rigidbody;
    private playerMovement _target;

    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _target = gameController.instance.player;
        transform.right = _target.transform.position - transform.position;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _lifespan -= Time.deltaTime;
        if (_lifespan <= 0)
        {
            Destroy(gameObject);
        }

        // Destroys if it goes out of screen.
        Vector3 screenPos = mainCamera.WorldToViewportPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1) {
            Destroy(gameObject);
        }


    }

    private void FixedUpdate()
    { 
        if (!isDeflected){
            _rigidbody.AddForce(transform.right * _movement_speed);
            if (_rigidbody.linearVelocity.magnitude < _min_movement_speed)
            {
                _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _min_movement_speed;
            }
        }

        if (_homing == true && _seconds_of_homing_time < 0)
        {
            _seconds_of_homing_time -= Time.deltaTime;
            transform.right = _target.transform.position - transform.position;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isDeflected){
            // Player takes damage.
            Destroy(gameObject);
        }
        
    }

    public void setTarget(playerMovement _entity)
    {
        _target = _entity;
    }

    // Changes the direction of the bullet. 
    public void Deflect(Vector2 newDirection){
        transform.right = newDirection;
        // Stops current velocity.
        _rigidbody.linearVelocity = Vector2.zero;
        // Adds new velocity.
        _rigidbody.AddForce(newDirection * _movement_speed, ForceMode2D.Impulse);
        isDeflected = true;
        _homing = false;   
    }

    public bool checkIsDeflected()
    {
        return isDeflected;
    }    
}
