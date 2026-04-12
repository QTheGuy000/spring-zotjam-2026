using UnityEngine;

public class directMeleeEnemy : enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteColor = GetComponent<SpriteRenderer>().color;
        base.Start();
        _target = gameController.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == false || isDead == true)
        {
            return;
        }

        _movement_timer -= Time.deltaTime;
        _force_capping_timer -= Time.deltaTime;
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

    void _move()
    {
        if (_movement_timer < 0)
        {
            _movement_timer = _seconds_between_movement_change;
            if (transform.position.x < 0)
            {
                _target_x = 10;
            }
            else
            {
                _target_x = -10;
            }
        }

        _target_y = _target.transform.position.y + Random.Range(0, 0.3f);


        _distance_to_target_x = Mathf.Abs(_target_x - transform.position.x);
        _distance_to_target_y = Mathf.Abs(_target_y - transform.position.y);

        if (_target_y > transform.position.y) // movement multiplier is +/- if _target_y is above/below the current position 
        {
            _vertical_movement_additive = _distance_to_target_y / (_distance_to_target_x + _distance_to_target_y) * _vertical_acceleration_multiplier;
        }
        else
        {
            _vertical_movement_additive = -1f * _distance_to_target_y / (_distance_to_target_x + _distance_to_target_y) * _vertical_acceleration_multiplier;
        }

        if (_target_x > transform.position.x) // movement multiplier is +/- if _target_x is to the right/left of the current position 
        {
            _horizontal_movement_additive = _distance_to_target_x / (_distance_to_target_x + _distance_to_target_y) * _horizontal_acceleration_multiplier;
        }
        else
        {
            _horizontal_movement_additive = -1 * _distance_to_target_x / (_distance_to_target_x + _distance_to_target_y) * _horizontal_acceleration_multiplier;
        }

        _rigidbody.linearVelocity = (new Vector2(_horizontal_movement_additive, _vertical_movement_additive)); // force added every frame. To prevent exponential speed increases, _force_capping_timer applies a normalization

    }
}
