using System.Linq;
using UnityEngine;

public class artemis : enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject[] _list_of_ranged_attacks;
    [SerializeField] string[] _names_of_ranged_attacks;
    [SerializeField] float _seconds_between_attack;
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] GameObject _muskrat_add;

    private float _attack_timer;
    private bool _enraged;
    private GameObject _instantiated_ranged_attack;

    enum ArtemisPhases
    {
        melee,
        ranged
    }
    
    private ArtemisPhases current_phase = ArtemisPhases.ranged;

    new void Start()
    {
        spriteColor = GetComponent<SpriteRenderer>().color;
        _attack_timer = _seconds_between_attack;
        _target = gameController.instance.player;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (isActive == false || isDead == true)
        {
            return;
        }

        if (_health < 20 && _enraged == false)
        {
            _enraged = true;
            _seconds_between_attack = _seconds_between_attack * 0.75f;
            _movement_speed = _movement_speed * 2.25f;
            _vertical_acceleration_multiplier *= 2.15f;
            _horizontal_acceleration_multiplier *= 2.15f;
            for (int i = 0; i < 3; i++)
            {
                GameObject instantiated_add = Instantiate(_muskrat_add, transform.position - new Vector3(3 + (i*2), 0), Quaternion.identity);
                instantiated_add.GetComponent<followleaderProjectileEnemy>().leader = transform.gameObject;
                instantiated_add.GetComponent<followleaderProjectileEnemy>().chaos_factor = Random.Range(0.5f, 1.5f);
            }
        }


        _attack_timer -= Time.deltaTime;
        _movement_timer -= Time.deltaTime;
        _force_capping_timer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {

        if (isActive == false || isDead == true)
        {
            return;
        }

        if (_force_capping_timer < 0) // when _force_capping_timer hits 0, velocity is normalized and multiplied by movement speed 
        {
            _force_capping_timer = _seconds_between_force_capping;
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _movement_speed;
        }


        if (_rigidbody.linearVelocityX < 0)
        {
            _sprite.flipX = true;
        }
        else
        {
            _sprite.flipX = false;
        }


        if (_attack_timer < 0)
        {
            _attack_timer = _seconds_between_attack;
            if (current_phase == ArtemisPhases.ranged)
            {
                RangedAttack();
            }
        }


        _move();


    }
    
    void RangedAttack()
    {
        int selected_attack_index = Random.Range(0, _list_of_ranged_attacks.Count());
        //int selected_attack_index = 5;
        GameObject selected_attack = _list_of_ranged_attacks[selected_attack_index];

        _instantiated_ranged_attack = Instantiate(selected_attack, transform.position, Quaternion.identity);

    }


    void _move()
    {
        _movement_timer -= Time.deltaTime;
        if (_movement_timer <= 0) // every time _movement_timer hits 0, finds new _target_y coordinate to move towards 
        {
            if (current_phase == ArtemisPhases.melee)
            {
                current_phase = ArtemisPhases.ranged;
                _horizontal_acceleration_multiplier -= 0.8f;
            }
            else
            {
                current_phase = ArtemisPhases.melee;
                _horizontal_acceleration_multiplier += 0.8f;
            }

            if (current_phase == ArtemisPhases.ranged)
            {
                _target_x = Random.Range(-6f, 6f);
                _target_y = Random.Range(1f, 3f);
            }

        }

        if (current_phase == ArtemisPhases.melee)
        {
            _target_x = _target.transform.position.x;
            _target_y = _target.transform.position.y;
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

        _rigidbody.AddForce(new Vector2(_horizontal_movement_additive, _vertical_movement_additive)); // force added every frame. To prevent exponential speed increases, _force_capping_timer applies a normalization

    }

}
