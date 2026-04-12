using UnityEngine;

public class artemis : enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == false || isDead == true)
        {
            return;
        }

    }

    private void FixedUpdate()
    {
        if (isActive == false || isDead == true)
        {
            return;
        }

    }

}
