using UnityEngine;

public class miniboss1Circlet : MonoBehaviour
{

    [SerializeField] GameObject[] _worm_segment_list;

    private GameObject _instantiated_enemy;
    private float _circle_count = 7;
    private Vector3[] _spawn_position_list = { new Vector3(-2.56f, -2.07f, 0), new Vector3(-1.22f, -0.13f, 0), new Vector3(0.78f, 1.29f, 0), new Vector3(2.66f, 3.02f, 0), new Vector3(0.52f, 4.96f, 0), new Vector3(-1.19f, 6.73f, 0), new Vector3(-3.04f, 8.24f, 0) };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < _circle_count; i++)
        {
            float angle = (Random.Range(0, 360));


            if (i > 0)
            {
                _worm_segment_list[i].transform.GetComponent<followleaderProjectileEnemy>().leader = _instantiated_enemy;
                _worm_segment_list[i].transform.GetComponent<followleaderProjectileEnemy>().chaos_factor = Random.Range(0.5f, 1.5f);
            }

            _instantiated_enemy = Instantiate(_worm_segment_list[i], _spawn_position_list[i], Quaternion.identity);

            
        }

        Destroy(gameObject);

    }


}
