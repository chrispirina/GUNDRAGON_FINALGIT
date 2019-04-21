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
            health = Mathf.Clamp(value, 0F, maxHealth);
            UpdateHealth();
        }
    }
    [SerializeField]
    [Readonly]
    private float health;
    public float maxHealth = 200F;
    [Readonly]
    public bool isDead;
    public bool startBurn;
    public bool wasHit;


    private void UpdateHealth()
    {
        if (Health <= .0F)
            isDead = true;
    }
}
