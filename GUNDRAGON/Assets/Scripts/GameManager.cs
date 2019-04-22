using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
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

    public Player player;
    public bool requireCursor = false;

    public bool IsPaused
    {
        get => isPaused; set
        {
            isPaused = value;
            UpdatePause();
        }
    }
    [SerializeField]
    private bool isPaused;

    void Awake()
    {
        Instance = this;

        playerMeleeDamage = 5.0f;

        /*
        startMenuPanel = GameObject.FindGameObjectWithTag("StartMenu");
        pauseMenuPanel = GameObject.FindGameObjectWithTag("PauseMenu");
        endMenuPanel = GameObject.FindGameObjectWithTag("EndMenu");
        youLose = GameObject.FindGameObjectWithTag("LoseText");
        youWin = GameObject.FindGameObjectWithTag("WinText");*/

        didStart = false;
        startMenuPanel.SetActive(true);
        requireCursor = true;
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
    void Update()
    {
        Cursor.lockState = requireCursor ? CursorLockMode.None : CursorLockMode.Locked;

        playerHealthSlider.value = player.Health;

        comboMeasure.value = ScoreManager.Instance.comboTimer;

        if (enemiesRemaining <= 0)
            PlayerWon();

        if (player.isDead)
            PlayerDied();

        combatScore.text = (ScoreManager.Instance.combatScore.ToString() + " X " + ScoreManager.Instance.comboModifier.ToString());
        scoreIndicator.text = (ScoreManager.Instance.LevelScore.ToString());
        finalScore.text = ("Final Score: " + ScoreManager.Instance.FinalScore.ToString());
        levelTime.text = ("Final Time: " + ScoreManager.Instance.minutes.ToString("00") + ":" + ScoreManager.Instance.seconds.ToString("00"));


    }

    public void UpdatePause()
    {
        Time.timeScale = isPaused ? 0F : 1F;
        if (pauseMenuPanel)
            pauseMenuPanel.SetActive(isPaused);
        requireCursor = isPaused;
    }

    void PlayerDied()
    {
        ScoreManager.Instance.pointsEarned = ScoreManager.Instance.pointsEarned / 2;
        pointGained.text = ("Points Earned: " + ScoreManager.Instance.pointsEarned.ToString());
        upgradePoints += ScoreManager.Instance.pointsEarned;
        Time.timeScale = 0;
        requireCursor = true;
        youWin.SetActive(false);
        youLose.SetActive(true);
        startMenuPanel.SetActive(false);
        endMenuPanel.SetActive(true);
        youLose.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    void PlayerWon()
    {
        pointGained.text = ("Points Earned: " + ScoreManager.Instance.pointsEarned.ToString());
        upgradePoints += ScoreManager.Instance.pointsEarned;
        Time.timeScale = 0;
        requireCursor = true;
        youLose.SetActive(false);
        youWin.SetActive(true);
        startMenuPanel.SetActive(false);
        endMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    public void Resume()
    {
        startMenuPanel.SetActive(false);
        endMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        IsPaused = false;
    }

    public void Restart()
    {
        youLose.SetActive(false);
        startMenuPanel.SetActive(true);
        endMenuPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
