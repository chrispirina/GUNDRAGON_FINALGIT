using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;
    public UIManager uiManager;
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
    public Player player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Instantiate GameManager if none exist.
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        
        playerHealthSlider.value = player.Health;

        comboMeasure.value = ScoreManager.Instance.comboTimer;
        combatScore.text = (ScoreManager.Instance.combatScore.ToString() + " X " + ScoreManager.Instance.comboModifier.ToString());
        scoreIndicator.text = (ScoreManager.Instance.LevelScore.ToString());
        finalScore.text = ("Final Score: " + ScoreManager.Instance.FinalScore.ToString());
        levelTime.text = ("Final Time: " + ScoreManager.Instance.minutes.ToString("00") + ":" + ScoreManager.Instance.seconds.ToString("00"));
    }

    public void Resume()
    {
        startMenuPanel.SetActive(false);
        endMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        GameManager.Instance.IsPaused = false;
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
