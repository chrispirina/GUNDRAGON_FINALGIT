using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeDetection : MonoBehaviour
{
    public static bool canSmack = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("hit enemy");
            //if (Player.didSmack == true)
            //{
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                if(enemy)
                {
                    if (canSmack == true)
                    {
                        enemy.wasHit = true;
                        enemy.Health -= GameManager.Instance.playerMeleeDamage;
                        enemy.wasHit = true;
                        canSmack = false;
                    }                    
                }
                else if (other.gameObject.GetComponentInParent<Enemy>())
                {
                enemy = other.GetComponentInParent<EnemyHealth>();
                    if (canSmack == true)
                    {
                       enemy.wasHit = true;
                       enemy.Health -= GameManager.Instance.playerMeleeDamage;
                       enemy.wasHit = true;
                        canSmack = false;
                    }                    
                }
                
                ScoreManager.Instance.hitCount += 1;
                ScoreManager.Instance.combatScore += (ScoreManager.Instance.meleeAttackScore * ScoreManager.Instance.comboModifier);
                Debug.Log("Smacked an Enemy");
            }
            else
                Debug.Log("Couldnt smack enemy");
        //}
    }
}
