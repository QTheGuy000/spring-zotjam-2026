using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class artemis : enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject[] _list_of_ranged_attacks;
    [SerializeField] string[] _names_of_ranged_attacks;
    [SerializeField] float _seconds_between_attack;
    [SerializeField] SpriteRenderer _sprite;

    private float _attack_timer;
    private bool _enraged;
    private GameObject _instantiated_ranged_attack;

    enum ArtemisPhases
    {
        melee,
        ranged
    }
    
    private ArtemisPhases current_phase = ArtemisPhases.ranged;

    void Start()
    {
        spriteColor = GetComponent<SpriteRenderer>().color;
        _attack_timer = _seconds_between_attack;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (isActive == false || isDead == true)
        {
            return;
        }

        _attack_timer -= Time.deltaTime;
        _movement_timer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (isActive == false || isDead == true)
        {
            return;
        }

        if (_rigidbody.linearVelocityX < 0)
        {
            _sprite.flipX = true;
        }
        else
        {
            _sprite.flipY = true;
        }


        if (_attack_timer < 0)
        {
            _attack_timer = _seconds_between_attack;
            if (current_phase == ArtemisPhases.ranged)
            {
                RangedAttack();
            }
        }





    }
    
    void RangedAttack()
    {
        int selected_attack_index = Random.Range(0, _list_of_ranged_attacks.Count());
        //int selected_attack_index = 5;
        GameObject selected_attack = _list_of_ranged_attacks[selected_attack_index];
        _instantiated_ranged_attack = Instantiate(selected_attack, transform.position, Quaternion.identity);

    }




}
