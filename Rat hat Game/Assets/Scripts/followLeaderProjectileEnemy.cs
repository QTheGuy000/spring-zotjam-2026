using UnityEditor.UI;
using UnityEngine;

public class followleaderProjectileEnemy : enemy
{

    private float _projectile_timer;
    private float _velocity_height_fluctuation_bound = 1.2f;

    [SerializeField] public GameObject leader;


    private GameObject _instantiated_projectile;
    [SerializeField] GameObject _projectile;
    [SerializeField] float _seconds_between_projectiles = 1;
    [SerializeField] float _projectile_spawn_multiplier = 1.2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        _projectile_timer = _seconds_between_projectiles;
        _force_capping_timer = _seconds_between_force_capping;

        if (chaos_factor != 0)
        {
            _seconds_between_projectiles += chaos_factor;
            _projectile_timer += Random.Range(0, Mathf.Abs(chaos_factor));
        }


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
        _instantiated_projectile.GetComponent<projectile>().setTarget(_target);
    }

    void _move()
    {
        _movement_timer -= Time.deltaTime;
        if (_movement_timer <= 0) // every time _movement_timer hits 0, finds new _target_x coordinate to move towards 
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.y * Random.Range(-_velocity_height_fluctuation_bound, _velocity_height_fluctuation_bound)); // adds degree of randomization to vertical movement
            _movement_timer = _seconds_between_movement_change;

            if (leader != null)
            {

                _target_x = leader.transform.position.x;
                _target_y = leader.transform.position.y;

            }

            else
            {
                _target_x = Random.Range(-8, 8);
                _target_y = Random.Range(-4, 4);
            }

        }

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

        _rigidbody.linearVelocity = (new Vector2(_horizontal_movement_additive, _vertical_movement_additive)); // force added every frame. To prevent exponential speed increases, _force_capping_timer applies a normalization
    }

}
