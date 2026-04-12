using UnityEngine;

public class Ending : MonoBehaviour
{

    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] GameObject _artemis;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_artemis == null)
        {
            _sprite.enabled = true;
        }
    }
}
