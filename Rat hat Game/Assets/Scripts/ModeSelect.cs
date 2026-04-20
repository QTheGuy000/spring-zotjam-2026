using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeSelect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button easyButton = transform.GetChild(0).GetComponent<Button>();
        Button normalButton = transform.GetChild(1).GetComponent<Button>();
        Button hardButton = transform.GetChild(2).GetComponent<Button>();

        easyButton.onClick.AddListener(() => EasyMode());
        normalButton.onClick.AddListener(() => NormalMode());
    }

    void EasyMode(){
        Statistics.difficulty = "Easy";
        SceneManager.LoadScene("EasyLevel1Scene");
    }

    void NormalMode(){
        Statistics.difficulty = "Normal";
        SceneManager.LoadScene("Level1Scene");
    }

    void HardMode(){
        Statistics.difficulty = "Hard";
        SceneManager.LoadScene("HardLevel1Scene");
    }

}
