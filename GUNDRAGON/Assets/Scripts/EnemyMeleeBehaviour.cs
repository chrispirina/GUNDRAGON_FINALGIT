using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeBehaviour : MonoBehaviour
{
    private float health = 25.0f;
    private float damage = 5.0f;
    private float speed = 5.0f;
    private float retreatRange = 10.0f;
    private float chaseRange = 25.0f;
    private float attackRange = 20.0f;
    public NavMeshAgent enemyAgent;
    private EnemyMaster enemyMaster;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetInitialReferences()
    {
        enemyMaster = gameObject.GetComponent<EnemyMaster>();
        enemyMaster.enemyHealth = health;
        enemyMaster.enemyDamage = damage;
        enemyMaster.enemySpeed = speed;
        enemyMaster.enemyRetreatRange = retreatRange;
        enemyMaster.enemyChaseRange = chaseRange;
        enemyMaster.enemyAttackRange = attackRange;
        enemyAgent = enemyMaster.agent;
    }
}
