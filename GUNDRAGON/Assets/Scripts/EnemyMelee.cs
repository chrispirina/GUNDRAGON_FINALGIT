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
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("hit player");
            if (thisEnemy.amAttacking == true)
            {
                Player.publicPlayerHealth -= GetComponentInParent<Enemy>().Damage;                
                other.gameObject.GetComponent<Player>().playerWasHit = true;
                other.gameObject.GetComponent<Animator>().SetTrigger("WasHit");
                Debug.Log("Smacked player");
                thisEnemy.amAttacking = false;
            }
            else
                Debug.Log("Couldnt smack player");
        }
    }
}
