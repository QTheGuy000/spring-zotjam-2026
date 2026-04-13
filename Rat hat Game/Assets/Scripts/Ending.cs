using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    [SerializeField] Sprite _sprite1;
    [SerializeField] Sprite _sprite2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Image img = GetComponent<Image>();

        if (Random.Range(0, 1) == 1)
        {
            img.sprite = _sprite1;
        }
        else
        {
            img.sprite = _sprite2;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene("MenuScreen");
        }
    }
}
