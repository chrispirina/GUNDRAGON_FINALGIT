using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    public float healAmount = 20.0f;


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player.Health < player.maxHealth)
            {
                player.Health += healAmount;
                Destroy(this.gameObject);
            }

            
        }
    }

}
