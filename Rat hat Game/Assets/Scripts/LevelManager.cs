using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public bool gameComplete = false;
    public bool levelComplete = false;
    public int currentLevel = 0;
    public bool isPaused = false;
    public GameObject pauseMenu;

    private enemy[] enemies;

    public float slideSpeed = 5f;         // Speed of the level transition
    public float slideDistance = 10f;     // How far the level slides down (match your level height)
    public float enemyStartDelay = 3f;    // Seconds before enemies activate

    private bool isTransitioning = false;

    private LevelManager instance;

    public GameObject lincolnDialogue;
    private List<string> dialogues = new(){
        "I am the 16th President!",
        "I was the tallest President!",
        "I signed the Emancimation Proclaimation!",
        "My favorite meal is the Chicken Fricassee!"
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        Button continueButton = pauseMenu.transform.GetChild(1).GetComponent<Button>();
        Button menuButton = pauseMenu.transform.GetChild(2).GetComponent<Button>();
        Button quitButton = pauseMenu.transform.GetChild(3).GetComponent<Button>();

        continueButton.onClick.AddListener(() => TogglePause());
        menuButton.onClick.AddListener(() => GoToMenu());
        quitButton.onClick.AddListener(() => QuitGame());

        pauseMenu.SetActive(false);
        lincolnDialogue.SetActive(false);

        // Disables all other levels
        for (int c = 1; c < transform.childCount; c++)
        {
            transform.GetChild(c).gameObject.SetActive(false);
        }

        // First level is enabled
        GameObject firstLevel = transform.GetChild(0).gameObject;
        firstLevel.SetActive(true);
        PopulateEnemies(firstLevel);
        StartCoroutine(ActivateEnemies());
    }

    void Start()
    {
        
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            TogglePause();
        }

        // Lets transition do transition
        if (isTransitioning) return;

        // Checks for level complete
        if (!levelComplete)
        {
            levelComplete = AllEnemiesDead();
        }

        // Moves to next level
        if (levelComplete)
        {
            levelComplete = false;
            StartCoroutine(NextLevel());
        }
    }

    // Populates enemies list from the level's enemies child object
    void PopulateEnemies(GameObject level)
    {
        // 1th child (index 1) of the level is the enemies container
        Transform enemiesContainer = level.transform.GetChild(1);
        enemies = enemiesContainer.GetComponentsInChildren<enemy>(true);
    }

    // Activates all enemies in the current level
    IEnumerator ActivateEnemies()
    {
        yield return new WaitForSeconds(enemyStartDelay);

        foreach (enemy e in enemies)
        {
            if (e != null){
                //e.isActive = true; // NOT IMPLEMENTED YET
            }
        }
    }

    IEnumerator NextLevel()
    {
        // Lincoln speaks truth first.
        yield return StartCoroutine(LincolnSpeaks());

        isTransitioning = true;
        currentLevel++;

        if (currentLevel >= transform.childCount)
        {
            Debug.Log("No more levels!");
            isTransitioning = false;
            gameComplete = true;
            yield break;
        }

        GameObject oldLevel = transform.GetChild(currentLevel - 1).gameObject;
        GameObject newLevel = transform.GetChild(currentLevel).gameObject;

        // Position new level above the screen
        //newLevel.transform.position = newLevel.transform.position + Vector3.up * slideDistance;
        newLevel.SetActive(true);
        PopulateEnemies(newLevel);

        // Slide both levels downward simultaneously
        float elapsed = 0f;
        float duration = slideDistance / slideSpeed;

        Vector3 oldLevelStart = oldLevel.transform.position;
        Vector3 newLevelStart = newLevel.transform.position;
        Vector3 offset = Vector3.down * slideDistance;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // Ease in-out for smoother feel
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            oldLevel.transform.position = Vector3.Lerp(oldLevelStart, oldLevelStart + offset, smoothT);
            newLevel.transform.position = Vector3.Lerp(newLevelStart, newLevelStart + offset, smoothT);

            yield return null;
        }

        oldLevel.SetActive(false);
        StartCoroutine(ActivateEnemies());
        isTransitioning = false;
    }

    IEnumerator LincolnSpeaks(){
        lincolnDialogue.SetActive(true);

        string text = dialogues[currentLevel];
        GameObject textbox = lincolnDialogue.transform.GetChild(0).gameObject;
        TextAnimation textanim = textbox.GetComponent<TextAnimation>();

        textanim.dialogues.Clear();  // Clear first
        textanim.AddDialogue(text);  // Then add

        yield return StartCoroutine(textanim.RevealText()); // Then run

        yield return new WaitForSeconds(1f);
        lincolnDialogue.SetActive(false);
    }

    bool AllEnemiesDead()
    {
        if (enemies == null || enemies.Length == 0) return false;

        foreach (enemy enemy in enemies)
        {
            if (enemy == null) continue;
            if (!enemy.isDead) return false;
        }

        return true;
    }

    // Pauses and Unpauses the game
    public void TogglePause(){
        // Unpause
        if (Time.timeScale == 0){
            isPaused = false;
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
        // Pause
        else {
            isPaused = true;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }

    public void GoToMenu(){
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MenuScreen"));
    }

    // Closes the game
    public void QuitGame(){
        Debug.Log("QUIT!");
        Application.Quit();
    }
}