using UnityEngine;


public class projectile : MonoBehaviour
{
    [SerializeField] float _lifespan = 3;
    [SerializeField] float _movement_speed = 4;
    [SerializeField] float _min_movement_speed = 2;

    [SerializeField] Rigidbody2D _rigidbody;
    private playerMovement _target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _target = gameController.instance.player;
        transform.right = _target.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _lifespan -= Time.deltaTime;
        if (_lifespan <= 0)
        {
            Destroy(gameObject);
        }


    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(transform.right * _movement_speed);
        if (_rigidbody.linearVelocity.magnitude < _min_movement_speed)
        {
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _min_movement_speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    public void setTarget(playerMovement _entity)
    {
        _target = _entity;
    }
}
