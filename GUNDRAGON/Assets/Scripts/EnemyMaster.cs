using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMaster : MonoBehaviour
{
    public float enemyHealth;
    public float enemyDamage;
    public float enemySpeed;
    public float enemyRetreatRange;
    public float enemyChaseRange;
    public float enemyAttackRange;

    private Transform enemyTransform;
    private Collider[] hitColliders;
    private float checkRate;
    private float nextCheck;
    public float distanceToPlayer;

    public NavMeshAgent agent;
    public GameObject player;
    public LayerMask playerDetectionLayer;
    private float detectionRadius = 100.0f;

    public bool isAttacking = false;
    public bool isRetreating = false;
    public bool isChasing = false;
    public bool isIdle = false;

    public bool isAlive = true;

    void Start()
    {
        SetInitialReferences();
    }    

    void Update()
    {
        if (enemyHealth <= 0)
        {
            isAlive = false;
            StartCoroutine(EnemyDeath());
        }

        else if (isAlive == true)
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, enemyTransform.position);
            CheckPlayerInRange();

            if (distanceToPlayer < enemyRetreatRange)
            {
                //retreat function attached to behaviour script
                isAttacking = false;
                isRetreating = true;
                isChasing = false;
                isIdle = false;
            }

            else if (distanceToPlayer >= enemyRetreatRange && distanceToPlayer < enemyAttackRange)
            {
                //attack function attached to behaviour script
                isAttacking = true;
                isRetreating = false;
                isChasing = false;
                isIdle = false;
            }

            else if (distanceToPlayer >= enemyAttackRange && distanceToPlayer <= enemyChaseRange)
            {
                // Chase function attached to behaviour script
                isAttacking = false;
                isRetreating = false;
                isChasing = true;
                isIdle = false;
            }

            else if (distanceToPlayer > enemyChaseRange)
            {
                // idle behaviour attached to behaviour script.
                isAttacking = false;
                isRetreating = false;
                isChasing = false;
                isIdle = true;
            }
        }
        
    }

    void SetInitialReferences()
    {
        enemyTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        checkRate = Random.Range(0.8f, 1.5f);
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;        
        distanceToPlayer = Vector3.Distance(player.transform.position, enemyTransform.position);
        isAlive = true;
    }

    void CheckPlayerInRange()
    {
        if (Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;

            hitColliders = Physics.OverlapSphere(enemyTransform.position, detectionRadius, playerDetectionLayer);
            if (hitColliders.Length > 0)
            {
                agent.autoBraking = false;
                agent.enabled = true;
            }
        }
        
    }

    public IEnumerator EnemyDeath()
    {
        agent.enabled = false;
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
        yield return new WaitForSeconds(3f);
        ScoreManager.Instance.CombatScore += 100 * ScoreManager.Instance.comboModifier;
        Destroy(gameObject);
        GameManager.Instance.enemiesRemaining -= 1;
    }
}
