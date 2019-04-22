using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeDetection : MonoBehaviour
{
    private Collider hitCollider;

    private void Awake()
    {
        hitCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (!enemy)
                enemy = other.GetComponentInParent<EnemyHealth>();
            if (enemy)
            {

                enemy.Health -= GameManager.Instance.playerMeleeDamage;
                hitCollider.enabled = false;
                other.GetComponentInChildren<Animator>().SetTrigger(Anim.DAMAGED);

            }

            ScoreManager.Instance.hitCount += 1;
            ScoreManager.Instance.combatScore += (ScoreManager.Instance.meleeAttackScore * ScoreManager.Instance.comboModifier);
        }
        else
            Debug.Log("Couldnt smack enemy");

    }
}
