using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    private GameManager gameManager;
    public bool didStart = false;

    public Slider playerHealthSlider;
    public Slider comboMeasure;
    public GameObject pauseMenuPanel;
    public GameObject startMenuPanel;
    public GameObject endMenuPanel;
    public GameObject youWin; 
    public GameObject youLose;
    public TextMeshProUGUI scoreIndicator;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI combatScore;
    public TextMeshProUGUI levelTime;
    public TextMeshProUGUI pointGained;

    public float playerMeleeDamage;
    public float playerGunDamage;
    public float upgradePoints;

    public int enemiesRemaining = 5;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        gameManager = GetComponent<GameManager>();
        /*
        startMenuPanel = GameObject.FindGameObjectWithTag("StartMenu");
        pauseMenuPanel = GameObject.FindGameObjectWithTag("PauseMenu");
        endMenuPanel = GameObject.FindGameObjectWithTag("EndMenu");
        youLose = GameObject.FindGameObjectWithTag("LoseText");
        youWin = GameObject.FindGameObjectWithTag("WinText");*/

        Player.didPause = false;
        didStart = false;
        startMenuPanel.SetActive(true);
        endMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        youWin.SetActive(false); // Menu UI is all deactivated.
        youLose.SetActive(false);
        Time.timeScale = 0;
    }

    void Start()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        playerHealthSlider.value = Player.publicPlayerHealth;

        comboMeasure.value = ScoreManager.Instance.comboTimer;

        if (enemiesRemaining <= 0)
        {
            Player.endReached = true;
        }
        if (Instance != null)
        {
            if (didStart == true)
            {
                isPause();
            }
            else
            {
                if (didStart == false)
                {
                    startMenuPanel.SetActive(true);

                }
            }

            if (Player.playerIsDead == true)
            {                
                playerDied();
            }

            if (Player.endReached == true)
            {
                playerWon();
            }
        }

        combatScore.text = (ScoreManager.Instance.CombatScore.ToString() + " X " + ScoreManager.Instance.comboModifier.ToString());
        scoreIndicator.text = (ScoreManager.Instance.LevelScore.ToString());
        finalScore.text = ("Final Score: " + ScoreManager.Instance.FinalScore.ToString());
        levelTime.text = ("Final Time: " + ScoreManager.Instance.minutes.ToString("00") + ":" + ScoreManager.Instance.seconds.ToString("00"));
        
        
	}

    public void isPause()
    {
        if (Player.didPause == true)
        {
            Time.timeScale = 0;
            pauseMenuPanel.SetActive(true);
        }
        else if (Player.didPause == false)
        {
            Time.timeScale = 1;
            pauseMenuPanel.SetActive(false);
        }
    }
    void playerDied()
    {
        ScoreManager.Instance.pointsEarned = ScoreManager.Instance.pointsEarned / 2;
        pointGained.text = ("Points Earned: " + ScoreManager.Instance.pointsEarned.ToString());
        upgradePoints += ScoreManager.Instance.pointsEarned;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        youWin.SetActive(false);
        youLose.SetActive(true);
        startMenuPanel.SetActive(false);
        endMenuPanel.SetActive(true);
        youLose.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    void playerWon()
    {
        pointGained.text = ("Points Earned: " + ScoreManager.Instance.pointsEarned.ToString());
        upgradePoints += ScoreManager.Instance.pointsEarned;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        youLose.SetActive(false);
        youWin.SetActive(true);
        startMenuPanel.SetActive(false);
        endMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    public void Resume()
    {
        if (didStart == false)
        {
            didStart = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startMenuPanel.SetActive(false);
        endMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        Player.didPause = false;
    }

    public void Restart()
    {
        ScoreManager.Instance.LevelScore = 0;
        ScoreManager.Instance.rawLevelTimer = 0;
        ScoreManager.Instance.pointsEarned = 0;
        ScoreManager.Instance.FinalScore = 0;
        ScoreManager.Instance.comboModifier = 1;
        ScoreManager.Instance.highestComboMod = 1;        
        enemiesRemaining = 5;
        Player.health = Player.maxPlayerHealth;
        Player.publicPlayerHealth = Player.maxPlayerHealth;
        Player.playerIsDead = false;
        Player.endReached = false;
        Player.didPause = true;
        didStart = false;
        startMenuPanel.SetActive(true);
        endMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        youWin.SetActive(false); // Menu UI is all deactivated.
        youLose.SetActive(false);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
