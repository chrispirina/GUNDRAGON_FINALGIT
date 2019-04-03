using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearPickup : MonoBehaviour
{
    public bool spearGathered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spearGathered == true)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Player") == true)
        {
            if (spearGathered == false)
            {
                other.GetComponent<Player>().gotSpear = true;
                spearGathered = true;
            }


        }
    }
}
