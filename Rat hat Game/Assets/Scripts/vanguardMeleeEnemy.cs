using UnityEditor.UI;
using UnityEngine;

public class vanguardMeleeEnemy : enemy
{

    [SerializeField] public GameObject leader;
    [SerializeField] public Rigidbody2D _vanguard_rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        spriteColor = GetComponent<SpriteRenderer>().color;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    private void FixedUpdate()
    {
        if (isActive == false || isDead == true)
        {
            return;
        }

        _move();
    }

    void _move()
    {
        if (leader == null)
        { 
            Destroy(gameObject);
        }
        
    }

}
