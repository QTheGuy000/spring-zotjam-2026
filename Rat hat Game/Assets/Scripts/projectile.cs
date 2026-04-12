using UnityEngine;


public class projectile : MonoBehaviour
{
    [SerializeField] float _lifespan = 6;
    [SerializeField] float _movement_speed = 4;
    [SerializeField] float _min_movement_speed = 2;
    [SerializeField] float _seconds_of_homing_time;
    [SerializeField] bool _homing;
    [SerializeField] bool _rotating;
    [SerializeField] public bool clockwise = true;
    [SerializeField] float _angles_per_second = 5;
    [SerializeField] bool _initial_lock_on = true; // if true, locks on to player at start
    private bool isDeflected = false;
    private float _angle;

    public float starting_angle;

    [SerializeField] Rigidbody2D _rigidbody;
    private playerMovement _target;

    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        _angle = starting_angle;
        _target = gameController.instance.player;

        if (_rotating == false && _initial_lock_on == true)
        {
            transform.right = _target.transform.position - transform.position;
        }
        else
        {
            transform.right = new Vector3(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad), 0);
        }


    }

    // Update is called once per frame
    void Update()
    {
        _lifespan -= Time.deltaTime;
        if (_rotating)
        {
            if (clockwise)
            {
                _angle += _angles_per_second * Time.deltaTime;

            }
            else
            {
                _angle -= _angles_per_second * Time.deltaTime;
            }

            _angle %= 360;
        }
        if (_lifespan <= 0)
        {
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

        else if (_rotating == true)
        {
            transform.eulerAngles = new Vector3(0, 0, _angle);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isDeflected){
            Destroy(gameObject);
        }
        
    }

    // Changes the direction of the bullet. 
    public void Deflect(Vector2 newDirection){
        transform.right = newDirection;
        // Stops current velocity.
        _rigidbody.linearVelocity = Vector2.zero;
        // Adds new velocity.
        _rigidbody.AddForce(newDirection * _movement_speed, ForceMode2D.Impulse);

        _homing = false;
        _rotating = false;
        isDeflected = true;

        GetComponent<SpriteRenderer>().color = Color.limeGreen;
    }

    public bool checkIsDeflected()
    {
        return isDeflected;
    }    
}
