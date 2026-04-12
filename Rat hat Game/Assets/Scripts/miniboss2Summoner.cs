using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;

public class miniboss2Summoner : enemy
{

    private float _distance_to_target;
    private float _summon_timer;

    private GameObject _instantiated_summon;
    [SerializeField]
    GameObject[] _list_of_summonables;

    [SerializeField]
    GameObject[] _list_of_vanguards;

    [SerializeField] float _seconds_between_summon = 1;
    [SerializeField] float _summon_spawn_multiplier = 1.2f;
    [SerializeField] float _angles_per_second;
    [SerializeField] float _vanguard_radius = 2f;

    private List<float> _list_of_vanguard_angles = new List<float>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        spriteColor = GetComponent<SpriteRenderer>().color;
        base.Start();
        _summon_timer = _seconds_between_summon;
        _force_capping_timer = _seconds_between_force_capping;

        for (int i = 0; i<_list_of_vanguards.Count(); i++)
        {
            _list_of_vanguard_angles.Add(i * 360 / _list_of_vanguards.Count());
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
        _summon_timer -= Time.deltaTime;
        _force_capping_timer -= Time.deltaTime;

        if (_summon_timer <= 0) // when _projectile_timer hits 0, fires projectile
        {
            _summon_timer = _seconds_between_summon;
            _summonEntity();
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
        _rotateVanguards();
        if (_force_capping_timer < 0) // when _force_capping_timer hits 0, velocity is normalized and multiplied by movement speed 
        {
            _force_capping_timer = _seconds_between_force_capping;
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _movement_speed;
        }


    }

    void _rotateVanguards()
    {
        for (int i = 0; i<_list_of_vanguards.Count(); i++)
        {
            _list_of_vanguard_angles[i] += Time.deltaTime * _angles_per_second;
            _list_of_vanguard_angles[i] = _list_of_vanguard_angles[i] % 360;
            if (_list_of_vanguards[i] != null)
            {

                vanguardMeleeEnemy vanguard = _list_of_vanguards[i].GetComponent<vanguardMeleeEnemy>();
                float x_position = transform.position.x + _vanguard_radius * Mathf.Cos(_list_of_vanguard_angles[i] * Mathf.Deg2Rad);
                float y_position = transform.position.y + _vanguard_radius * Mathf.Sin(_list_of_vanguard_angles[i] * Mathf.Deg2Rad);


                vanguard._vanguard_rigidbody.MovePosition(new Vector2(x_position, y_position));
                Debug.Log(_list_of_vanguard_angles[i]);


            }




        }



    }

    void _summonEntity() // instantiates projectile 
    {
        Vector3 line_to_target = _target.transform.position - transform.position;
        line_to_target = Vector3.Normalize(line_to_target);
        int summon_index = Random.Range(0, _list_of_summonables.Count());
        _instantiated_summon = Instantiate(_list_of_summonables[summon_index], transform.position + line_to_target * _summon_spawn_multiplier, Quaternion.identity);
        if (summon_index > 0 && summon_index < 4)
        {
            _instantiated_summon.transform.GetComponent<enemy>().isActive = true;
        }

    }

    void _move()
    {
        _movement_timer -= Time.deltaTime;
        if (_movement_timer <= 0) // every time _movement_timer hits 0, finds new _target_x coordinate to move towards 
        {
            _target_x = Random.Range(-5f, 5f);
            _target_y = Random.Range(0f, 4f);
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
