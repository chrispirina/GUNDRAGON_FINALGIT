using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangeBehaviour : MonoBehaviour
{
    private float health = 25.0f;
    private float damage = 10.0f;
    private float speed = 4.0f;
    private float retreatRange = 5.0f;
    private float chaseRange = 30.0f;
    private float attackRange = 20.0f;
    private float attackCooldown = 0.0f;
    private float attackCooldownMax = 3.0f;
    public CharacterController enemyMover;
    private EnemyMaster enemyMaster;
    private Animator animator;
    public GameObject enemyBullet;
    public GameObject gunEmitter;
    Transform emitterPos;
    public GameObject player;




    private void Awake()
    {
        enemyMaster = GetComponent<EnemyMaster>();
        animator = GetComponentInChildren<Animator>();
        enemyMaster.enemyHealth = health;
        enemyMaster.enemyDamage = damage;
        enemyMaster.enemySpeed = speed;
        enemyMaster.enemyRetreatRange = retreatRange;
        enemyMaster.enemyChaseRange = chaseRange;
        enemyMaster.enemyAttackRange = attackRange;
        enemyMover = enemyMaster.controller;
        emitterPos = gunEmitter.transform;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        emitterPos.LookAt(player.transform.position);

        if (attackCooldown >= 0.0f)
        {
            attackCooldown -= Time.deltaTime;
        }

        if (enemyMaster.isAttacking)
        {
            animator.SetBool("isMoving", false);

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Standard_Idle") && attackCooldown <= 0.0f)
            {
                animator.SetTrigger("Attack");
                shootBullet();
                attackCooldown = attackCooldownMax;
            }

        }
        else if (enemyMaster.isIdle)
        {
            animator.SetBool("isMoving", false);

        }
        else
        {
            animator.SetBool("isMoving", true);
        }

        if (enemyMaster.isRetreating)
            animator.SetBool("shouldRetreat", true);
        else animator.SetBool("shouldRetreat", false);
        
    }

    void shootBullet()
    {
        GameObject tempBulletHandler;
        if (emitterPos != null)
        {
            tempBulletHandler = Instantiate(enemyBullet, emitterPos.position, emitterPos.rotation) as GameObject;
        }
    }
}
