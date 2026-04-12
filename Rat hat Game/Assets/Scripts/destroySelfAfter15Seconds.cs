using UnityEngine;

public class destroySelfAfter15Seconds : MonoBehaviour
{

    private float _time;
  

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time > 15)
        {
            Destroy(gameObject);
        }
    }
}
