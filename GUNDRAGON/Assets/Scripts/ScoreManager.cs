using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance = null;
    private ScoreManager scoreManager;

    public float LevelScore;
    public float combatScore;
    public float FinalScore;
    public float comboModifier = 1.0f;
    public int meleeAttackScore = 20;
    public int gunAttackScore = 10;
    public float currentHitCount = 0.0f;
    public float hitCount = 0.0f;
    public float comboTimer = 0;
    public float maxComboTimer = 5.0f;
    public float highestComboMod;
    public float pointsEarned = 0.0f;
    public float rawLevelTimer = 0.0f;
    public float seconds, minutes;

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

    }

	// Use this for initialization
	void Start ()
    {
       
        highestComboMod = 1.0f;
        LevelScore = 0.0f;
        combatScore = 0.0f;
        FinalScore = 0.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        rawLevelTimer += Time.deltaTime;
        seconds = (int)(rawLevelTimer) % 60;
        minutes = (int)(rawLevelTimer / 60f) % 60;

        if (highestComboMod < comboModifier)
        {
            highestComboMod = comboModifier;
        }

        if (GameManager.Instance.player.isDead /*|| Player.endReached*/)
        {
            LevelScore += combatScore;
            hitCount = 0;
            currentHitCount = 0;
            combatScore = 0;
            comboModifier = 1.0f;
        }

        FinalScore = (LevelScore * highestComboMod);

        pointsEarned = (FinalScore / 5);
        pointsEarned = (Mathf.Round(pointsEarned));

		if (hitCount > currentHitCount)
        {
            comboTimer = maxComboTimer;
            currentHitCount = hitCount;
        }
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        }
        if (comboTimer <= 0)
        {
            LevelScore += combatScore;
            hitCount = 0;
            currentHitCount = 0;
            combatScore = 0;
            comboModifier = 1.0f;

        }

        ComboModify();

        
    }

    void ComboModify()
    {
        if (hitCount >= 0 && hitCount < 2)
        {
            comboModifier = 1.0f;
        }
        else if (hitCount >= 2 && hitCount < 4)
            
            {
                comboModifier = 2f;
            }

        else if (hitCount >= 4 && hitCount < 8)

        {
            comboModifier = 3f;
        }

        else if (hitCount >= 8 && hitCount < 12)

        {
            comboModifier = 4f;
        }

        else if (hitCount >= 12 && hitCount < 16)

        {
            comboModifier = 5f;
        }

        else if (hitCount >= 16 && hitCount < 20)

        {
            comboModifier = 6f;
        }

        else if (hitCount >= 20 && hitCount < 25)

        {
            comboModifier = 7f;
        }

        else if (hitCount >= 25 && hitCount < 30)

        {
            comboModifier = 8f;
        }

        else if (hitCount >= 30 && hitCount < 35)

        {
            comboModifier = 9f;
        }

        else if (hitCount >= 35 && hitCount < 50)

        {
            comboModifier = 10f;
        }

        else if (hitCount >= 50)

        {
            comboModifier = 15f;
        }

    }
}
