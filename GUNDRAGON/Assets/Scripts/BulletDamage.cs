using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public float bulletForce = 10.0f;

    void Update()
    {
        Rigidbody tempBulletRigid;
        tempBulletRigid = gameObject.GetComponent<Rigidbody>();
        tempBulletRigid.velocity = gameObject.transform.forward * bulletForce;
        Destroy(gameObject, 6.0f);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (!player)
                return;

            player.Health -= 5F;
            Debug.Log("Smacked player");
        }

        Destroy(gameObject);
    }
}

