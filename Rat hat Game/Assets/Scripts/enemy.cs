using UnityEngine;

public class enemy : MonoBehaviour
{
    protected playerMovement _target;
    protected float _target_x;
    protected float _target_y;
    protected float _time;
    protected float _movement_timer;
    protected float _horizontal_movement_multiplier;
    protected float _vertical_movement_multiplier;
    [SerializeField] protected float _seconds_between_movement_change = 2;
    [SerializeField] protected float _movement_speed = 3f;
    [SerializeField] protected Rigidbody2D _rigidbody;

}
