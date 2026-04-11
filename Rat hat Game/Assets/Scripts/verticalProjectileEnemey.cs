using UnityEditor.UI;
using UnityEngine;

public class verticalProjectileEnemy : enemy
{

    private float _projectile_timer;
    private float _base_x;

    [SerializeField] float _max_distance_from_target = 4;
    [SerializeField] float _amplitude = 4;
    [SerializeField] float _frequency = 4;

    private GameObject _instantiated_projectile;
    [SerializeField] GameObject _projectile;
    [SerializeField] float _seconds_between_projectiles = 1;
    [SerializeField] float _projectile_spawn_multiplier = 1.2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _projectile_timer = _seconds_between_projectiles;
        _seconds_between_force_capping = 1;
        _force_capping_timer = _seconds_between_force_capping;
        _base_x = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        _projectile_timer -= Time.deltaTime;
        _force_capping_timer -= Time.deltaTime;

        if (_projectile_timer <= 0) // when _projectile_timer hits 0, fires projectile
        {
            _projectile_timer = _seconds_between_projectiles;
            _fireAtTarget();
        }


    }

    private void FixedUpdate()
    {
        _target = gameController.instance.player;
        _move();
        if (_force_capping_timer < 0) // when _force_capping_timer hits 0, velocity is normalized and multiplied by _movement_speed
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
        _instantiated_projectile.GetComponent<projectile>().setTarget(_target);
    }

    void _move()
    {
        _movement_timer -= Time.deltaTime; 
        if (_movement_timer <= 0) // every time _movement_timer hits 0, finds new _target_y coordinate to move towards 
        {
            _movement_timer = _seconds_between_movement_change;

                if (transform.position.y > 0){ // if not too far from player, _target_y is the farthest edge of the map
                    _target_y = -4;
                }
                else
                {
                    _target_y = 4;
                }
        }

        _target_x = _base_x + Mathf.Sin(_time * _frequency) * _amplitude; // _target_x is updated every frame based on a sine wave and the current time 

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
