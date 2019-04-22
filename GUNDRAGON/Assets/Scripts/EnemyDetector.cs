using UnityEngine;
using System.Collections;

public class EnemyDetector : MonoBehaviour
{
    public GameObject[] testEnemies = { };
    public GameObject[] destroyObjects = { };
    public GameObject particlesTemplate;

    private bool isTriggered = false;

    private void Update()
    {
        if (isTriggered)
            return;

        foreach (GameObject enemy in testEnemies)
        {
            if (enemy)
                return;
        }

        isTriggered = true;
        StartCoroutine(DestroyEffect());
    }

    private IEnumerator DestroyEffect()
    {
        GameObject[] particles = new GameObject[destroyObjects.Length];

        for (int i = 0; i < destroyObjects.Length; i++)
        {
            if (particlesTemplate)
                particles[i] = Instantiate(particlesTemplate, destroyObjects[i].transform.position, destroyObjects[i].transform.rotation);

            Destroy(destroyObjects[i]);
        }

        float time = 2F;

        while (time >= 0F)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        foreach (GameObject go in particles)
            Destroy(go);
    }
}