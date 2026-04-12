using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;

public class enemy : MonoBehaviour
{

    protected float _screen_lower_bound = -4.25f;
    protected float _screen_upper_bound = 4.25f;
    protected float _hat_top_bound = -3.00f;
    protected int _screen_count = 1;
    protected float _screen_height = 10f;
    protected playerMovement _target;
    protected float _target_x;
    protected float _target_y;
    protected float _distance_to_target_x;
    protected float _distance_to_target_y;
    protected float _time;
    protected float _movement_timer;
    protected float _horizontal_movement_additive;
    protected float _vertical_movement_additive;
    protected float _force_capping_timer;
    [SerializeField] protected float _vertical_acceleration_multiplier;
    [SerializeField] protected float _horizontal_acceleration_multiplier;
    [SerializeField] protected float _seconds_between_force_capping;
    [SerializeField] protected float _seconds_between_movement_change = 2;
    [SerializeField] protected float _movement_speed = 3f;
    [SerializeField] protected Rigidbody2D _rigidbody;
    public float chaos_factor = 0;
    public Color spriteColor;

    // Main Stats
    [SerializeField] protected int _health = 2;
    public bool isDead = false;
    public bool isActive = false;
    
    public void Start(){
        spriteColor = GetComponent<SpriteRenderer>().color;
    }

    void CheckHealth()
    {
        if (_health < 0)
        {
            isDead = true;
            transform.gameObject.SetActive(false);
        }
    }


    public void DecreaseHealth(int damage_amount = 1)
    {
        if (_health > 0){
            _health -= damage_amount;
            StartCoroutine(TakeDamage());
        }

        if (_health <= 0){
            Die();
        }
    }

    public virtual IEnumerator TakeDamage(){
        // Flashes damage twice
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Projectile causes damage
        if (collision.gameObject.CompareTag("Projectile")){
            projectile pr = collision.gameObject.GetComponent<projectile>();
            if (pr.checkIsDeflected()){
                DecreaseHealth();
            }

        }
    }

    void Die(){
        isDead = true;
        isActive = false;
        gameObject.SetActive(false);
        // Explosion
    }

}
