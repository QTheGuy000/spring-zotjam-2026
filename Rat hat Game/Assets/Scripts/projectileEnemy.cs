using UnityEditor.UI;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{

    private playerMovement _target;
    private float _time;
    private float _distance_to_target;
    private float _projectile_timer;
    private float _movement_timer;
    private float _target_x;
    private float _target_y;
    private float _base_y;
    private float _horizontal_movement_multiplier;
    private float _vertical_movement_multiplier;
    private float _seconds_between_force_capping;
    private float _force_capping_timer;
    private float _velocity_height_fluctuation_bound = 1.2f;

    [SerializeField] float _max_distance_from_target = 4;
    [SerializeField] float _amplitude = 2;
    [SerializeField] float _frequency = 4f;

    [SerializeField] float _movement_speed = 3f;


    private GameObject _instantiated_projectile;
    [SerializeField] GameObject _projectile;
    [SerializeField] float _seconds_between_projectiles = 1;
    [SerializeField] float _seconds_between_movement_change = 2;
    [SerializeField] float _projectile_spawn_multiplier = 1.2f;
    [SerializeField] Rigidbody2D _rigidbody;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _projectile_timer = _seconds_between_projectiles;
        _seconds_between_force_capping = 2 / _frequency;
        _force_capping_timer = _seconds_between_force_capping;
        _base_y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        _projectile_timer -= Time.deltaTime;
        _seconds_between_force_capping -= Time.deltaTime;

        if (_projectile_timer <= 0)
        {
            _projectile_timer = _seconds_between_projectiles;
            _fireAtTarget();
        }


    }

    private void FixedUpdate()
    {
        _target = gameController.instance.player;
        _move();
        if (_force_capping_timer < 0)
        {
            _force_capping_timer = _seconds_between_force_capping;
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _movement_speed;
        }
    }

    void _fireAtTarget()
    {
        Vector3 line_to_target = _target.transform.position - transform.position;
        line_to_target = Vector3.Normalize(line_to_target);
        _instantiated_projectile = Instantiate(_projectile, transform.position + line_to_target * _projectile_spawn_multiplier, Quaternion.identity);
        _instantiated_projectile.GetComponent<projectile>().setTarget(_target);
    }

    void _move()
    {
        _movement_timer -= Time.deltaTime;
        if (_movement_timer <= 0)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.y * Random.Range(-_velocity_height_fluctuation_bound, _velocity_height_fluctuation_bound));
            _movement_timer = _seconds_between_movement_change;
            _distance_to_target = Vector3.Distance(transform.position, _target.transform.position);
            if (_distance_to_target > _max_distance_from_target)
            {
                _target_x = _target.transform.position.x;
            }
            else
            {
                if (transform.position.x > 0){
                    _target_x = -8;
                }
                else
                {
                    _target_x = 8;
                }
            }

            if (_target_x > transform.position.x)
            {
                _horizontal_movement_multiplier = 1.5f;
            }
            else
            {
                _horizontal_movement_multiplier = -1.5f;
            }

        }

        _target_y = _base_y + Mathf.Sin(_time * _frequency) * _amplitude;

        if (_target_y > transform.position.y)
        {
            _vertical_movement_multiplier = 1;
        }
        else
        {
            _vertical_movement_multiplier = -1;
        }

        _rigidbody.AddForce(new Vector2(_horizontal_movement_multiplier, _vertical_movement_multiplier * _amplitude));
    }

}
