using UnityEngine;

public class cameraController : MonoBehaviour
{
    private float _seconds_of_camera_shake = 0;


    
    // Update is called once per frame
    void Update()
    {
        if (_seconds_of_camera_shake > 0)
        {
            _seconds_of_camera_shake -= Time.deltaTime;
            ShakeCamera();
        }
        else
        {
            transform.position = new Vector3(0, 0, -15);
        }
    }
    public void AddCameraShake(float seconds)
    {
        _seconds_of_camera_shake += seconds;
    }
    private void ShakeCamera()
    {
        transform.position += _RandomNormalVector3();
    }

    private Vector3 _RandomNormalVector3()
    {
        return (new Vector3(Random.Range(-.05f, .05f), Random.Range(-.05f, .05f), 0));
    }


}
