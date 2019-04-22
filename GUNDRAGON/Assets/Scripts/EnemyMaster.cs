using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMaster : MonoBehaviour
{
    [System.NonSerialized]
    public float enemyHealth;
    [System.NonSerialized]
    public float enemyDamage;
    [System.NonSerialized]
    public float enemySpeed;
    [System.NonSerialized]
    public float enemyRetreatRange;
    [System.NonSerialized]
    public float enemyChaseRange;
    [System.NonSerialized]
    public float enemyAttackRange;

    private Transform enemyTransform;
    private Collider[] hitColliders;

    private float nextCheck;
    public  float distanceToPlayer;

    private float yVelocity;

    public CharacterController controller;
    public GameObject player;
    public LayerMask playerDetectionLayer;

    public bool isAttacking = false;
    public bool isRetreating = false;
    public bool isChasing = false;
    public bool isIdle = false;

    public bool isAlive = true;

    void Awake()
    {
        SetInitialReferences();
    }    

    void Update()
    {
        Vector3 heading = player.transform.position - transform.position;
        heading.y = 0F;
        float distanceToPlayer = heading.magnitude;
        Vector3 direction = heading / distanceToPlayer;

        yVelocity += 9.81F * Time.deltaTime;

        float speedMultiplier = 1f;

        if (isAlive == false)
        {
            direction *= 0.0f;
            StartCoroutine(EnemyDeath());
        }

        else if (isAlive == true)
        {
            
            //distanceToPlayer = Vector3.Distance(player.transform.position, enemyTransform.position);

           // CheckPlayerInRange();

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
                speedMultiplier = 0;
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
                speedMultiplier = 0;
            }
            if (isRetreating)
            {
                direction *= -1;
            }

            transform.forward = direction;
            controller.Move((direction * speedMultiplier * enemySpeed + Vector3.down * yVelocity) * Time.deltaTime);

            if (controller.isGrounded)
                yVelocity = 0F;
        }

       

    }

    void SetInitialReferences()
    {
        enemyTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        controller = GetComponent<CharacterController>();        
        distanceToPlayer = Vector3.Distance(player.transform.position, enemyTransform.position);
        isAlive = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, enemyChaseRange);
        Gizmos.DrawWireSphere(transform.position, enemyAttackRange);
    }

    public IEnumerator EnemyDeath()
    {
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
        yield return new WaitForSeconds(3f);
        ScoreManager.Instance.combatScore += 100 * ScoreManager.Instance.comboModifier;
        Destroy(gameObject);
        GameManager.Instance.enemiesRemaining -= 1;
    }
}
