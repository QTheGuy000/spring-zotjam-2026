using UnityEngine;

public class enemy : MonoBehaviour
{
    protected playerMovement _target;
    protected float _target_x;
    protected float _target_y;
    protected float _distance_to_target_x;
    protected float _distance_to_target_y;
    protected float _time;
    protected float _movement_timer;
    protected float _horizontal_movement_additive;
    protected float _vertical_movement_additive;
    protected float _seconds_between_force_capping;
    protected float _force_capping_timer;
    [SerializeField] protected float _vertical_acceleration_multiplier;
    [SerializeField] protected float _horizontal_acceleration_multiplier;
    [SerializeField] protected float _seconds_between_movement_change = 2;
    [SerializeField] protected float _movement_speed = 3f;
    [SerializeField] protected Rigidbody2D _rigidbody;
    [SerializeField] protected int _health;
    public bool isDead = false;


    void _checkHealth()
    {
        if (_health < 0)
        {
            isDead = true;
            transform.gameObject.SetActive(false);
        }
    }


    public void receiveDamage(int damage_amount)
    {
        _health -= damage_amount;
        _checkHealth();
    }

}
