using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float Health
    {
        get => health;
        set
        {
            Debug.Log(health - value);
            health = Mathf.Clamp(value, 0F, maxHealth);
            UpdateHealth();
        }
    }

    public bool isBurning = false;
    public bool startBurn = false;
    public float burnTime = 0.0f;
    public float maxBurnTime = 5.0f;

    [SerializeField]
    [Readonly]
    private float health;
    public float maxHealth;
    [Readonly]
    public bool isDead;
    public bool wasHit;

    private void Start()
    {
        maxHealth = GetComponent<EnemyMaster>().enemyHealth;
        Health = maxHealth;
    }

    private void UpdateHealth()
    {
        if (Health <= .0F)
            GetComponent<EnemyMaster>().isAlive = false;
        GetComponent<EnemyMaster>().enemyHealth = Health;
    }

    void Update()
    {
        if (startBurn == true)
        {
            burnTime = maxBurnTime;
            isBurning = true;
            startBurn = false;
        }

        if (burnTime > 0)
        {
            Health -= 1.5f * Time.deltaTime;
            burnTime -= Time.deltaTime;
        }
        else if (burnTime <= 0)
        {
            isBurning = false;
        }
    }
}
