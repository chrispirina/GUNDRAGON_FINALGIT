using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{                                                                               //Line 86 ranged enemy movement, have enemies notice the player, and when in range kite around between shots
                                                                                //Line 70 melee enemy movement, have enemies notice player, move away from player briefly between attacks.
                                                                                //don't let enemies occ. same space

    public float enemyHealth;

    public bool wasHit = false;
    public float attackTimer;
    public float attackCooldown;
    public float attackRate = 2.0f;
    public float attackCooldownMax = 2.0f;
    public int bulletsShot = 0;

    public bool amAttacking = false;
    public bool didAttack = false;
    public bool enemyDead = false;

    public bool isBurning = false;
    public bool startBurn = false;
    public float burnTime = 0.0f;
    public float maxBurnTime = 5.0f;

    public Animator enemyAnim;
    public Transform PlayerTransform;

    Vector3 PlayerDestination;
    public NavMeshAgent agent;
    public GameObject PlayerTarget;

    public bool isRanged = false;

    public GameObject enemyBullet;
    GameObject gunEmitter;
    Transform emitterPos;
    public float bulletForce = 10.0f;


    public float Damage = 10.0f;

    void Awake()
    {
        enemyAnim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.enabled = true;
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = PlayerTarget.GetComponent<Transform>();
        gunEmitter = gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject; //GameObject.FindGameObjectWithTag("EnemyGunEmitter");
        emitterPos = gunEmitter.transform;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerDestination = PlayerTransform.position;

        if (enemyHealth <= 0)
        {
            StartCoroutine(EnemyDeath());
        }

        if (startBurn == true)
        {
            burnTime = maxBurnTime;
            isBurning = true;
            startBurn = false;
        }

        if (burnTime > 0)
        {
            enemyHealth -= 1 * Time.deltaTime;
            burnTime -= Time.deltaTime;
        }
        else if (burnTime <= 0)
        {
            isBurning = false;
        }

        if (agent.enabled == true && isRanged == false)
        {
            agent.speed = 4.0f;
            if (amAttacking == false)
            {
                transform.LookAt(agent.destination);
            }
            agent.destination = PlayerDestination;
            if (agent.remainingDistance <= 3.0f)
            {
                if (amAttacking == false && didAttack == false)
                {
                    EnemyMeleeAttack();
                }
            }
        }

        if (agent.enabled == true && isRanged == true)
        {
            agent.speed = 2.5f;
            agent.destination = PlayerDestination;
            transform.LookAt(agent.destination);
            if (agent.remainingDistance >= 5.0f && agent.remainingDistance <= 10.0f)
            {
                if (amAttacking == false && didAttack == false)
                {
                    EnemyShootAttack();
                }
            }

            if (agent.remainingDistance < 5.0f)
            {
                agent.isStopped = true;
            }

        }

        if (attackCooldown > 0)
        {
            amAttacking = true;
            if (agent.enabled == true)
            {
                agent.isStopped = true;
            }
            attackCooldown -= Time.deltaTime;
        }
        else if (attackCooldown <= 0)
        {
            if (agent.enabled == true)
            {
                agent.isStopped = false;
            }
            amAttacking = false;
        }
        if (wasHit == true)
        {
            //play hit animation
            wasHit = false;
        }
        if (attackTimer > 0)
        {
            didAttack = true;
            attackTimer -= Time.deltaTime;
        }
        else if (attackTimer <= 0)
        {
            didAttack = false;
        }

        if (bulletsShot > 0)
        {
            shootBullet();
        }

        if (agent.enabled == true && enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Hit") == true)
        {
            agent.isStopped = true;
            attackCooldown = attackCooldownMax;
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
        ScoreManager.Instance.combatScore += 100 * ScoreManager.Instance.comboModifier;
        Destroy(gameObject);
        GameManager.Instance.enemiesRemaining -= 1;
    }

    void EnemyMeleeAttack()
    {
        attackCooldown = attackCooldownMax;
        attackTimer = attackRate;
        enemyAnim.SetTrigger("Enemy_Struck");
    }

    void EnemyShootAttack()
    {
        attackCooldown = attackCooldownMax;
        attackTimer = attackRate;
        enemyAnim.SetTrigger("Enemy_Shot");
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
