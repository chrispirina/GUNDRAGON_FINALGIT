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
            if (Player.didSmack == true)
            {
                if(other.gameObject.GetComponent<Enemy>())
                {
                    if (canSmack == true)
                    {
                        other.gameObject.GetComponent<Enemy>().enemyAnim.SetTrigger("Enemy_Hit");
                        other.gameObject.GetComponent<Enemy>().enemyHealth -= GameManager.Instance.playerMeleeDamage;
                        other.gameObject.GetComponent<Enemy>().wasHit = true;
                        canSmack = false;
                    }                    
                }
                else if (other.gameObject.GetComponentInParent<Enemy>())
                {
                    if (canSmack == true)
                    {
                        other.gameObject.GetComponentInParent<Enemy>().enemyAnim.SetTrigger("Enemy_Hit");
                        other.gameObject.GetComponentInParent<Enemy>().enemyHealth -= GameManager.Instance.playerMeleeDamage;
                        other.gameObject.GetComponentInParent<Enemy>().wasHit = true;
                        canSmack = false;
                    }                    
                }
                
                ScoreManager.Instance.hitCount += 1;
                ScoreManager.Instance.CombatScore += (ScoreManager.Instance.meleeAttackScore * ScoreManager.Instance.comboModifier);
                Debug.Log("Smacked an Enemy");
            }
            else
                Debug.Log("Couldnt smack enemy");
        }
    }
}
