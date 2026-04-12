using UnityEditor.UI;
using UnityEngine;

public class circularProjectileEnemy : enemy 
{
    private float _projectile_timer;
    private float _angles = 0;
    private Vector2 _circle_center;
    
    [SerializeField] float _radius = 2;
    [SerializeField] float _angles_per_second = 25;
    [SerializeField] bool _clockwise = true;
    [SerializeField] public bool random_circle_center = true;

    private GameObject _instantiated_projectile;
    [SerializeField] GameObject _projectile;
    [SerializeField] float _seconds_between_projectiles = 1;
    [SerializeField] float _projectile_spawn_multiplier = 1.2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        spriteColor = GetComponent<SpriteRenderer>().color;
        base.Start();
        _projectile_timer = _seconds_between_projectiles;
        _force_capping_timer = _seconds_between_force_capping;
        if (random_circle_center == true)
        {
            _circle_center = new Vector2(Random.Range(-5, 5), Random.Range(-2, 2));
            _circle_center = new Vector2(_circle_center.x + (_screen_count - 1) * _screen_height, _circle_center.y + (_screen_count - 1) * _screen_height);
        }
        
        if (chaos_factor != 0)
        {
            _radius += Random.Range(-chaos_factor, chaos_factor);
            if (Random.Range(0, 1) == 1)
            {
                _clockwise = false;
            }

            _angles_per_second = Random.Range(15, 35);
            _seconds_between_projectiles += Random.Range(-chaos_factor, chaos_factor);
            _movement_speed *= Random.Range(1, 3);
        }


        _target_x = _circle_center.x + _radius * Mathf.Cos(_angles * Mathf.Deg2Rad);
        _target_y = _circle_center.y + _radius * Mathf.Sin(_angles * Mathf.Deg2Rad);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == false || isDead == true)
        {
            return;
        }

        _time += Time.deltaTime;
        _projectile_timer -= Time.deltaTime;
        _force_capping_timer -= Time.deltaTime;
        if (Vector2.Distance(transform.position, new Vector2(_target_x, _target_y)) < 0.5f) // only increases angle if close to target
        {
            if (_clockwise)
            {
                _angles -= Time.deltaTime * _angles_per_second;
            }
            else
            {
                _angles += Time.deltaTime * _angles_per_second;
            }

        }


        if (_projectile_timer <= 0) // when _projectile_timer hits 0, fires projectile
        {
            _projectile_timer = _seconds_between_projectiles;
            _fireAtTarget();
        }


    }

    private void FixedUpdate()
    {
        if (isActive == false || isDead == true)
        {
            return;
        }

        _target = gameController.instance.player;
        _move();
        if (_force_capping_timer < 0) // when _force_capping_timer hits 0, velocity is normalized and multiplied by movement speed 
        {
            _force_capping_timer = _seconds_between_force_capping;
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _movement_speed;
        }
    }

    void _fireAtTarget() // instantiates projectile 
    {
        Vector3 line_to_target = _target.transform.position - transform.position;
        line_to_target = Vector3.Normalize(line_to_target);
        _instantiated_projectile = Instantiate(_projectile, transform.position + line_to_target * _projectile_spawn_multiplier, Quaternion.identity);
    }

    void _move()
    {
        _target_x = _circle_center.x + _radius * Mathf.Cos(_angles * Mathf.Deg2Rad);
        _target_y = _circle_center.y + _radius * Mathf.Sin(_angles * Mathf.Deg2Rad);

        _distance_to_target_x = Mathf.Abs(_target_x - transform.position.x);
        _distance_to_target_y = Mathf.Abs(_target_y - transform.position.y);

        if (_target_y > transform.position.y) // movement multiplier is +/- if _target_y is above/below the current position 
        {
            _vertical_movement_additive = _distance_to_target_y / (_distance_to_target_x + _distance_to_target_y) * _vertical_acceleration_multiplier; ;
        }
        else
        {
            _vertical_movement_additive = -1 * _distance_to_target_y / (_distance_to_target_x + _distance_to_target_y) * _vertical_acceleration_multiplier; ;
        }

        if (_target_x > transform.position.x) // movement multiplier is +/- if _target_x is to the right/left of the current position 
        {
            _horizontal_movement_additive = _distance_to_target_x / (_distance_to_target_x + _distance_to_target_y) * _horizontal_acceleration_multiplier; ;
        }
        else
        {
            _horizontal_movement_additive = -1 * _distance_to_target_x / (_distance_to_target_x + _distance_to_target_y) * _horizontal_acceleration_multiplier; ;
        }

        _rigidbody.AddForce(new Vector2(_horizontal_movement_additive, _vertical_movement_additive)); // force added every frame. To prevent exponential speed increases, _force_capping_timer applies a normalization

    }

}
