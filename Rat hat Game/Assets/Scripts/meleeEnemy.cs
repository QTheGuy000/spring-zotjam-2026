using UnityEngine;

public class meleeEnemy : enemy
{

    [SerializeField] float _movement_curve_flatenning_factor;

    private float _minimum_height = 3.25f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _target = gameController.instance.player;

    }

    // Update is called once per frame
    void Update()
    {
        _movement_timer -= Time.deltaTime;
    }


    private void FixedUpdate()
    {
        _target = gameController.instance.player;
        _move();

    }

    void _move()
    {
        if (_movement_timer < 0)
        {
            _movement_timer = _seconds_between_movement_change;
            _target_x = _target.transform.position.x;

            if (_target_x > transform.position.x) // movement multiplier is +/- if _target_x is to the right/left of the current position 
            {
                _horizontal_movement_multiplier = 1.5f;
            }
            else
            {
                _horizontal_movement_multiplier = -1.5f;
            }
        }

        if (_target_y > transform.position.y) // movement multiplier is +/- if _target_y is above/below the current position 
        {
            _vertical_movement_multiplier = 1;
        }
        else
        {
            _vertical_movement_multiplier = -1;
        }

        _target_y = (Mathf.Pow(transform.position.x - _target_x, 2) / _movement_curve_flatenning_factor) - _minimum_height;


        _rigidbody.AddForce(new Vector2(_horizontal_movement_multiplier, _vertical_movement_multiplier)); // force added every frame. To prevent exponential speed increases, _force_capping_timer applies a normalization
    }
}
