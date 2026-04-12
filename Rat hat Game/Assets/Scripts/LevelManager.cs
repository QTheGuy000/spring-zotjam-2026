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
    public GameObject gameOverMenu;
    public GameObject countdownMenu;
    public Transform origin;

    private enemy[] enemies;

    public float slideSpeed = 5f;         // Speed of the level transition
    public float slideDistance = 10.125f;     // How far the level slides down (match your level height)
    public int enemyStartDelay = 3;     // Seconds before level starts

    public bool isTransitioning = true;

    public static LevelManager instance;

    public GameObject lincolnDialogue;
    private List<string> dialogues = new(){
        "I am the 16th President!",
        "I was the tallest President!",
        "I signed the Emancimation Proclamation!",
        "My favorite meal is the Chicken Fricassee!",
        "I did not murder any individuals!",
        "I have never lied!",
        "I never died!",
        "I'm immortal!",
        "New quote: I play to win the game B)"
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

        Button restartButton = gameOverMenu.transform.GetChild(1).GetComponent<Button>();
        Button menuButton2 = gameOverMenu.transform.GetChild(2).GetComponent<Button>();
        Button quitButton2 = gameOverMenu.transform.GetChild(3).GetComponent<Button>();

        restartButton.onClick.AddListener(() => RestartLevel());
        menuButton2.onClick.AddListener(() => GoToMenu());
        quitButton2.onClick.AddListener(() => QuitGame());
        gameOverMenu.SetActive(false);

        lincolnDialogue.SetActive(false);

        countdownMenu.SetActive(false);

        // Disables all other levels
        for (int c = 1; c < transform.childCount; c++)
        {
            transform.GetChild(c).gameObject.SetActive(false);
        }
        isTransitioning = true;
        StartLevel(currentLevel);
    }

    void Start()
    {
        
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            TogglePause();
        }

        if (gameComplete){
            return;
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

    void StartLevel(int levelNum){
        GameObject level = transform.GetChild(levelNum).gameObject;

        PopulateEnemies(level);
        StartCoroutine(ActivateEnemies());
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
        yield return new WaitForSeconds(1);
        // Starts countdown.
        countdownMenu.SetActive(true);
        for (int i = enemyStartDelay; i > 0; i--){
            countdownMenu.GetComponent<TMP_Text>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownMenu.GetComponent<TMP_Text>().text = "GO";
        yield return new WaitForSeconds(1);
        countdownMenu.SetActive(false);

        // Activates
        foreach (enemy e in enemies){
            if (e != null){
                e.isActive = true;
            }
        }

        isTransitioning = false;
    }

    IEnumerator NextLevel()
    {
        isTransitioning = true;

        yield return StartCoroutine(LincolnSpeaks());

        currentLevel++;

        // Level 3 (index 3) is the background transition, not a real level
        if (currentLevel >= transform.childCount - 1)
        {
            // Slide to the background (child index 3)
            GameObject oldLevel = transform.GetChild(currentLevel - 1).gameObject;
            GameObject backgroundLevel = transform.GetChild(currentLevel).gameObject;
            backgroundLevel.SetActive(true);

            float elapsed = 0f;
            float duration = slideDistance / slideSpeed;

            Vector3 oldLevelStart = oldLevel.transform.position;
            Vector3 newLevelStart = backgroundLevel.transform.position;
            Vector3 offset = Vector3.down * slideDistance;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float smoothT = Mathf.SmoothStep(0f, 1f, t);

                oldLevel.transform.position = Vector3.Lerp(oldLevelStart, oldLevelStart + offset, smoothT);
                backgroundLevel.transform.position = Vector3.Lerp(newLevelStart, newLevelStart + offset, smoothT);

                yield return null;
            }

            oldLevel.SetActive(false);
            gameComplete = true;

            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Stage2Scene");
            yield break;
        }

        // Shift all remaining levels down instantly
        for (int i = currentLevel + 1; i < transform.childCount; i++)
        {
            Transform level = transform.GetChild(i);
            level.position += Vector3.down * slideDistance;
        }

        GameObject prevLevel = transform.GetChild(currentLevel - 1).gameObject;
        GameObject newLevel = transform.GetChild(currentLevel).gameObject;
        newLevel.SetActive(true);

        float elapsedNormal = 0f;
        float durationNormal = slideDistance / slideSpeed;

        Vector3 prevStart = prevLevel.transform.position;
        Vector3 nextStart = newLevel.transform.position;
        Vector3 normalOffset = Vector3.down * slideDistance;

        while (elapsedNormal < durationNormal)
        {
            elapsedNormal += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedNormal / durationNormal);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            prevLevel.transform.position = Vector3.Lerp(prevStart, prevStart + normalOffset, smoothT);
            newLevel.transform.position = Vector3.Lerp(nextStart, nextStart + normalOffset, smoothT);

            yield return null;
        }

        prevLevel.SetActive(false);
        StartLevel(currentLevel);
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

    public void RestartLevel(){
        //um
    }

    public void GameOver(){
        gameOverMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }
}