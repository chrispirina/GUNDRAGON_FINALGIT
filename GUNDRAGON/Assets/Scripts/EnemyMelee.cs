using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    EnemyMaster thisEnemy;

    void Start()
    {
        thisEnemy = GetComponentInParent<EnemyMaster>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("hit player");
            if (thisEnemy.isAttacking)
            {
                Player player = other.GetComponent<Player>();

                player.Health -= thisEnemy.enemyDamage;                
                Debug.Log("Smacked player");
            }
            else
                Debug.Log("Couldnt smack player");
        }
    }
}
