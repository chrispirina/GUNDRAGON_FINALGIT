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
        if (other.GetComponent<Collider>().CompareTag("Player") == true)
        {
            if (Player.publicPlayerHealth < (Player.maxPlayerHealth - healAmount))
            {
                Player.publicPlayerHealth += healAmount;
                Destroy(this.gameObject);
            }
            else if (Player.publicPlayerHealth < Player.maxPlayerHealth && Player.publicPlayerHealth > (Player.maxPlayerHealth - healAmount))
            {
                Player.publicPlayerHealth = Player.maxPlayerHealth;
                Destroy(this.gameObject);
            }

            
        }
    }

}
