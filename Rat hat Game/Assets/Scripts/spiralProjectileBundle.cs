using UnityEngine;

public class spiralProjectileBundle : MonoBehaviour
{
    [SerializeField] GameObject _projectile;
    [SerializeField] float _distance_from_center;
    [SerializeField] int _projectile_count;
    [SerializeField] bool _clockwise;
    private GameObject _instantiated_projectile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < _projectile_count; i++)
        {
            float angle = (360 / _projectile_count) * i;
            float _spawn_x = transform.position.x + _distance_from_center * Mathf.Cos(angle * Mathf.Deg2Rad);
            float _spawn_y = transform.position.y + _distance_from_center * Mathf.Sin(angle * Mathf.Deg2Rad);
            _projectile.transform.GetComponent<projectile>().starting_angle = angle;
            _projectile.transform.GetComponent<projectile>().clockwise = _clockwise;
            _instantiated_projectile = Instantiate(_projectile, new Vector3(_spawn_x, _spawn_y, 0), Quaternion.identity);

        }




        Destroy(gameObject);
    }


}
