using UnityEngine;

public class verticalMeleeEnemy : enemy
{

    private bool _recently_hit_player = false;
    private float _max_height = 3.6f;
    private float _falling_down_force_multiplier = 1.25f;

    enum verticalEnemyState
    {
        movingUp,
        movingHorizontal,
        movingDown
    }

    private verticalEnemyState _currentState = verticalEnemyState.movingUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _target = gameController.instance.player;
        _seconds_between_force_capping = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        _movement_timer -= Time.deltaTime;
        _force_capping_timer -= Time.deltaTime;
    }


    private void FixedUpdate()
    {
        _target = gameController.instance.player;
        _move();
        if (_force_capping_timer < 0) // when _force_capping_timer hits 0, velocity is normalized and multiplied by movement speed 
        {
            _force_capping_timer = _seconds_between_force_capping;
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _movement_speed;

        }
    }

    void _move()
    {

        if (_movement_timer < 0)
        {
            _movement_timer = _seconds_between_movement_change;
            _target_x = _target.transform.position.x; // player x is always targeted

            if (transform.position.y < _max_height || _recently_hit_player == true) // target roof 
            {
                if (_currentState == verticalEnemyState.movingDown && _recently_hit_player == false)
                {
                    if (transform.position.y < _hat_top_bound)
                    {
                        _target_y = _screen_upper_bound;
                        _currentState = verticalEnemyState.movingUp;
                    }
                }
                else
                {
                    _target_y = _screen_upper_bound;
                    _currentState = verticalEnemyState.movingUp;
                }
            }


            if (transform.position.y >= _max_height) // target player x only
            {
                _currentState = verticalEnemyState.movingHorizontal;
                _recently_hit_player = false;
            }

        }

        if (_recently_hit_player == false && Mathf.Abs(transform.position.x - _target.transform.position.x) < 0.3f && transform.position.y > _target.transform.position.y) // target player y when directly above
        {
            _currentState = verticalEnemyState.movingDown;
            _recently_hit_player = false;
            _target_y = _screen_lower_bound;
        }


        _distance_to_target_x = Mathf.Abs(_target_x - transform.position.x);
        _distance_to_target_y = Mathf.Abs(_target_y - transform.position.y);


        if (_currentState == verticalEnemyState.movingUp || _currentState == verticalEnemyState.movingDown)
        {
            if (_target_y > transform.position.y) // movement multiplier is +/- if _target_y is above/below the current position 
            {
                _vertical_movement_additive = _distance_to_target_y / (_distance_to_target_x + _distance_to_target_y) * _vertical_acceleration_multiplier;
            }
            else
            {
                _vertical_movement_additive = -1 * _distance_to_target_y / (_distance_to_target_x + _distance_to_target_y) * _vertical_acceleration_multiplier * _falling_down_force_multiplier;

                if (Mathf.Abs(transform.position.y - _target_y) < 0.2f)
                {
                    _vertical_movement_additive = 0f;
                }
            }
        }
        else
        {
            _vertical_movement_additive = 0;
        }

        if (_currentState == verticalEnemyState.movingHorizontal)
        {
            if (_target_x > transform.position.x) // movement multiplier is +/- if _target_x is to the right/left of the current position 
            {
                _horizontal_movement_additive = _distance_to_target_x / (_distance_to_target_x + _distance_to_target_y) * _horizontal_acceleration_multiplier;
            }
            else
            {
                _horizontal_movement_additive = -1 * _distance_to_target_x / (_distance_to_target_x + _distance_to_target_y) * _horizontal_acceleration_multiplier;
            }
        }
        else
        {
            _horizontal_movement_additive = 0;
        }

        _rigidbody.linearVelocity = (new Vector2(_horizontal_movement_additive, _vertical_movement_additive)); // force added every frame. To prevent exponential speed increases, _force_capping_timer applies a normalization
        Debug.Log(_currentState.ToString() + _horizontal_movement_additive.ToString());
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _recently_hit_player = true;
            _currentState = verticalEnemyState.movingUp;
            _target_y = _screen_upper_bound;
        }
    }
}
