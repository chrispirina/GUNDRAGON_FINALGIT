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
            Player.publicPlayerHealth -= 5;
            other.gameObject.GetComponent<Player>().playerWasHit = true;
            other.gameObject.GetComponent<Animator>().SetTrigger("WasHit");
            Debug.Log("Smacked player");
            Destroy(gameObject);
        }
        else
            Destroy(gameObject);
    }
}

