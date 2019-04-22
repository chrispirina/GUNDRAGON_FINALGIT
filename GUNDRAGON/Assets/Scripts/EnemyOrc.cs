using UnityEngine.AI;
using UnityEngine;

public class EnemyOrc : MonoBehaviour
{
    private float health = 30.0f;
    private float damage = 10.0f;
    private float speed = 5.0f;
    private float retreatRange = 0.0f;
    private float chaseRange = 25.0f;
    private float attackRange = 2.0f;
    private float attackCooldown = 0.0f;
    private float attackCooldownMax = 1.0f;
    public CharacterController enemyMover;
    private EnemyMaster enemyMaster;
    private Animator animator;



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
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (attackCooldown >= 0.0f)
        {
            attackCooldown -= Time.deltaTime;
        }
        
        if (enemyMaster.isAttacking)
        {
            animator.SetBool("isMoving", false);

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Combat Idle") && attackCooldown <= 0.0f)
            {
                animator.SetTrigger(Random.Range(0, 100) > 75 ? "MultiAttack" : "Attack");
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

    }

}
