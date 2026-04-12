using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public Cloud cloudPrefab;
    public float spawnInterval = 6f;
    public float cloudSpeed = 2f;
    public float cloudLifetime = 10f;
    public Vector3 cloudDirection = Vector3.right;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            Cloud cloud = Instantiate(cloudPrefab, transform.position, Quaternion.identity);
            cloud.SetDirection(cloudDirection, cloudSpeed, cloudLifetime);
            timer = 0f;
            cloud.transform.SetParent(transform);
        }
    }
}
