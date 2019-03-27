using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShot : MonoBehaviour
{
    public bool canShoot = true;
    

    Transform gunPos;
    Vector3 fwd;


	void Start ()
    {
        gunPos = GameObject.FindGameObjectWithTag("GunEmitter").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        fwd = gunPos.TransformDirection(Vector3.forward);
	}

    public void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(gunPos.position, fwd, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(gunPos.position, fwd * hit.distance, Color.green);
            
            if (hit.collider.CompareTag("Enemy"))
            {
                gameObject.GetComponent<Enemy>().enemyHealth -= 1;
                Debug.Log("Hit an Enemy");
                ScoreManager.Instance.hitCount += 0.5f;
                
            }
        }
    }
}
