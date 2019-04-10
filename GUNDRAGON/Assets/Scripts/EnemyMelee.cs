using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    Enemy thisEnemy;

    void Start()
    {
        thisEnemy = GetComponentInParent<Enemy>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("hit player");
            if (thisEnemy.amAttacking)
            {
                Player player = other.GetComponent<Player>();

                player.Health -= thisEnemy.Damage;                
                Debug.Log("Smacked player");
                thisEnemy.amAttacking = false;
            }
            else
                Debug.Log("Couldnt smack player");
        }
    }
}
