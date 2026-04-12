using UnityEngine;

public class Ending : MonoBehaviour
{

    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Sprite _sprite1;
    [SerializeField] Sprite _sprite2;
    [SerializeField] GameObject _artemis;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sprite.enabled = false;

        if (Random.Range(0, 1) ==1)
        {
            _sprite.sprite = _sprite1;
        }
        else
        {
            _sprite.sprite = _sprite2;
        }

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
