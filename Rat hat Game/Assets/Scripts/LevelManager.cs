using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public bool levelComplete = false;
    public bool stageComplete = false;
    public int currentStage;
    public bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject countdownMenu;
    public Transform origin;

    private enemy[] enemies;

    public float slideSpeed = 5f;         // Speed of the stage transition
    public float slideDistance = 10f;     // How far the stage slides down (match your stage height)
    public int enemyStartDelay = 3;     // Seconds before stage starts

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
        "New quote: I play to win the game B)",
        "I had a pet turkey named Jack!",
        "I invented a device to lift boats over shallow waters!",
        "I was the first President to have a beard!",
        "I was shot at before my assassination and survived!"
    };

    public GameObject levelTransitionCloud;

    public GameObject moon;
    private float moonScaleIncrement = 0.2f;  // How much the moon grows per stage
    private float moonScaleDuration = 1f;     // How long the grow animation takes

    public bool isMiniBoss = false;

    private void Awake()
    {
        currentStage = Statistics.CurrentStage;

        // Acts liek Static class
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Initializes UI
        Button continueButton = pauseMenu.transform.GetChild(0).GetComponent<Button>();
        Button menuButton = pauseMenu.transform.GetChild(1).GetComponent<Button>();
        Button restartButton = pauseMenu.transform.GetChild(2).GetComponent<Button>();
        Button quitButton = pauseMenu.transform.GetChild(3).GetComponent<Button>();

        continueButton.onClick.AddListener(() => TogglePause());
        menuButton.onClick.AddListener(() => GoToMenu());
        restartButton.onClick.AddListener(() => RestartLevel());
        quitButton.onClick.AddListener(() => QuitGame());
        pauseMenu.SetActive(false);

        Button restartButton2 = gameOverMenu.transform.GetChild(0).GetComponent<Button>();
        Button menuButton2 = gameOverMenu.transform.GetChild(1).GetComponent<Button>();
        Button quitButton2 = gameOverMenu.transform.GetChild(2).GetComponent<Button>();

        restartButton2.onClick.AddListener(() => RestartLevel());
        menuButton2.onClick.AddListener(() => GoToMenu());
        quitButton2.onClick.AddListener(() => QuitGame());
        gameOverMenu.SetActive(false);

        lincolnDialogue.SetActive(false);
        countdownMenu.SetActive(false);
        levelTransitionCloud.SetActive(false);

        // Disables all other stages
        for (int c = 1; c < transform.childCount; c++)
        {
            transform.GetChild(c).gameObject.SetActive(false);
        }

        // Explicitly enable only the starting stage
        transform.GetChild(currentStage).gameObject.SetActive(true);
        Time.timeScale = 1; // Add this!
        isPaused = false;   // Add this!
        isTransitioning = true;

        // At the end of Awake, after other setup:
        moon.transform.localScale = new Vector3(Statistics.MoonScale, Statistics.MoonScale, Statistics.MoonScale);
    }

    void Start()
    {
        StartStage(currentStage);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            TogglePause();
        }

        if (levelComplete){
            return;
        }

        // Lets transition do transition
        if (isTransitioning) return;

        // Checks for stage complete
        if (!stageComplete)
        {
            stageComplete = AllEnemiesDead();
        }

        // Moves to next stage
        if (stageComplete){
            stageComplete = false;
            // Destroys all projectiles
            foreach (GameObject projectile in GameObject.FindGameObjectsWithTag("Projectile")){
                Destroy(projectile);
            }

            StartCoroutine(NextStage());
        }
    }

    void StartStage(int stageNum){
        GameObject stage = transform.GetChild(stageNum).gameObject;

        PopulateEnemies(stage);
        StartCoroutine(ActivateEnemies());
    }

    // Populates enemies list from the stage's enemies child object
    void PopulateEnemies(GameObject stage)
    {
        // 1th child (index 1) of the stage is the enemies container
        Transform enemiesContainer = stage.transform.GetChild(1);
        enemies = enemiesContainer.GetComponentsInChildren<enemy>(true);
    }

    // Activates all enemies in the current stage
    IEnumerator ActivateEnemies()
    {
        yield return new WaitForSeconds(1);
        // Starts countdown.
        countdownMenu.SetActive(true);
        for (int i = enemyStartDelay; i > 0; i--){
            countdownMenu.GetComponent<TMP_Text>().text = i.ToString();
            yield return new WaitForSeconds(0.4f);
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

    IEnumerator NextStage()
    {
        isTransitioning = true;

        yield return StartCoroutine(LincolnSpeaks());

        currentStage++;
        Statistics.CurrentStage = currentStage;

        StartCoroutine(GrowMoon()); // Grows during transition

        if (currentStage >= transform.childCount - 1){
            GameObject oldStage = transform.GetChild(currentStage - 1).gameObject;
            GameObject backgroundStage = transform.GetChild(currentStage).gameObject;

            RectTransform cloudRect = levelTransitionCloud.GetComponent<RectTransform>();

            // Start cloud above the screen
            float screenHeight = Screen.height;
            cloudRect.anchoredPosition = new Vector2(0, screenHeight);
            levelTransitionCloud.SetActive(true);

            float elapsed = 0f;
            float duration = slideDistance / slideSpeed;

            // Slide cloud DOWN to cover screen
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float smoothT = Mathf.SmoothStep(0f, 1f, t);
                cloudRect.anchoredPosition = new Vector2(0, Mathf.Lerp(screenHeight, 0, smoothT));
                yield return null;
            }

            // Cloud fully covers screen — reposition and swap stages underneath
            cloudRect.anchoredPosition = Vector2.zero;
            oldStage.SetActive(false);
            backgroundStage.transform.position += Vector3.down * slideDistance; // Move it down!
            backgroundStage.SetActive(true);

            elapsed = 0f;

            // Slide cloud DOWN off screen to reveal new stage
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float smoothT = Mathf.SmoothStep(0f, 1f, t);
                cloudRect.anchoredPosition = new Vector2(0, Mathf.Lerp(0, -screenHeight, smoothT));
                yield return null;
            }

            levelTransitionCloud.SetActive(false);

            levelComplete = true;
            Statistics.CurrentLevel += 1;
            Statistics.CurrentStage = 0;

            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Level" + Statistics.CurrentLevel + "Scene");
            yield break;
        }

        // Otherwise, goes to next stage.
        // Shift all remaining stages down instantly
        for (int i = currentStage + 1; i < transform.childCount; i++)
        {
            Transform stage = transform.GetChild(i);
            stage.position += Vector3.down * slideDistance;
        }

        GameObject prevStage = transform.GetChild(currentStage - 1).gameObject;
        GameObject newStage = transform.GetChild(currentStage).gameObject;
        newStage.SetActive(true);

        float elapsedNormal = 0f;
        float durationNormal = slideDistance / slideSpeed;

        Vector3 prevStart = prevStage.transform.position;
        Vector3 nextStart = newStage.transform.position;
        Vector3 normalOffset = Vector3.down * slideDistance;

        while (elapsedNormal < durationNormal)
        {
            elapsedNormal += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedNormal / durationNormal);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            prevStage.transform.position = Vector3.Lerp(prevStart, prevStart + normalOffset, smoothT);
            newStage.transform.position = Vector3.Lerp(nextStart, nextStart + normalOffset, smoothT);

            yield return null;
        }

        prevStage.SetActive(false);
        StartStage(currentStage);
    }

    IEnumerator LincolnSpeaks(){
        lincolnDialogue.SetActive(true);

        string text = dialogues[UnityEngine.Random.Range(0, dialogues.Count)];
        GameObject textbox = lincolnDialogue.transform.GetChild(0).gameObject;
        TextAnimation textanim = textbox.GetComponent<TextAnimation>();

        textanim.dialogues.Clear();  // Clear first
        textanim.AddDialogue(text);  // Then add

        yield return StartCoroutine(textanim.RevealText()); // Then run

        yield return new WaitForSeconds(1f);
        lincolnDialogue.SetActive(false);
    }

    IEnumerator GrowMoon()
    {
        float elapsed = 0f;
        Vector3 startScale = moon.transform.localScale;
        float targetScaleValue = Statistics.MoonScale + moonScaleIncrement;
        Vector3 targetScale = new Vector3(targetScaleValue, targetScaleValue, targetScaleValue);

        while (elapsed < moonScaleDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moonScaleDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            moon.transform.localScale = Vector3.Lerp(startScale, targetScale, smoothT);
            yield return null;
        }

        moon.transform.localScale = targetScale;
        Statistics.MoonScale = targetScaleValue; // Save to Statistics
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
        SceneManager.LoadScene("MenuScreen");
    }

    // Closes the game
    public void QuitGame(){
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void RestartLevel(){
        Statistics.CurrentStage = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver(){
        gameOverMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }
}