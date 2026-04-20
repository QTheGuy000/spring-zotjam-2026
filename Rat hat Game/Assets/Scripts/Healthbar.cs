using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // The fill bar for the health.
    private Transform fill;
    // enemy script from the parent.
    private enemy _enemy;
    private int _maxHealth;
    private Vector3 _originalScale;

    void Start()
    {
        fill = transform.GetChild(1);
        _enemy = transform.parent.GetComponent<enemy>();
        _maxHealth = _enemy._health;
        _originalScale = fill.localScale;
    }

    void LateUpdate()
    {
        // Gets ratio between the health and max health.
        float ratio = Mathf.Clamp01((float)_enemy._health / _maxHealth);
        // Sets the size according to the ratio
        fill.localScale = new Vector3(_originalScale.x * ratio, _originalScale.y, _originalScale.z);
        fill.localPosition = new Vector3((ratio - 1f) * (_originalScale.x * 0.5f), fill.localPosition.y, 0f);
    }
}