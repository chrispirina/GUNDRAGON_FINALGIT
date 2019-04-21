using UnityEngine.AI;
using UnityEngine;

public class EnemyOrc : EnemyHealth
{
    private Player player;
    public float range = 20f;
    public float atkRange = 0.5f;
    private CharacterController controller;
    public float orcSpeed = 5f;

    private Animator animator;
    private float yVelocity;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 heading = player.transform.position - transform.position;
        heading.y = 0F;
        float distance = heading.magnitude;
        Vector3 direction = heading / distance;

        yVelocity += 9.81F * Time.deltaTime;

        if (distance <= atkRange)
        {
            animator.SetBool("isMoving", false);

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Combat Idle"))
                animator.SetTrigger(Random.Range(0, 100) > 75 ? "MultiAttack" : "Attack");

            direction *= 0F;
        }
        else if (distance > range)
        {
            animator.SetBool("isMoving", false);
            direction *= 0F;
        }
        else
        {
            animator.SetBool("isMoving", true);
        }

        controller.Move((direction * orcSpeed + Vector3.down * yVelocity) * Time.deltaTime);

        if (controller.isGrounded)
            yVelocity = 0F;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }
}
