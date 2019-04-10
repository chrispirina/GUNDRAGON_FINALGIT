using UnityEngine;
public class B : MonoBehaviour
{                                                                                           // Line 260 for Damage over time addition.
    public enum ElementType
    {
        NONE,
        FIRE,
    }

    public ElementType elementType = ElementType.NONE;

    bool didElementSwap = false;

    public ParticleSystem GunChange;
    public ParticleSystem fireShot;
    public ParticleSystem noneShot;

    GameObject fireGun;
    GameObject noneGun;

    float shootGun = 0;
    float gunOut = 0;
    public float fireRate = 0.2f;
    public float gunDamage = 5.0f;
    bool firstShot = false;

    public float hitMelee = 0;
    public float meleeRate = 0.2f;
    public float meleeDamage = 10.0f;
    public static bool didSmack = false;

    public static bool playerIsDead = false;
    public static bool endReached = false;


    public static bool didPause = false;

    public bool playerWasHit = false;
    public float visHealth;
    public static float health;
    public static float maxPlayerHealth = 200.0f;

    public bool gotSpear = false;

    private Transform gunPos;
    private Transform meleePos;
    private Vector3 fwdGun;
    private Animator playerAnimator;


    void Awake()
    {

        fireGun = GameObject.FindGameObjectWithTag("FireGun");
        noneGun = GameObject.FindGameObjectWithTag("NoneGun");
        meleePos = GameObject.FindGameObjectWithTag("MeleeHitbox").transform;
        gunPos = GameObject.FindGameObjectWithTag("GunEmitter").transform;
        playerAnimator = GetComponent<Animator>();
        health = maxPlayerHealth;
        health = maxPlayerHealth;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        visHealth = health;

        if (playerIsDead == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (playerIsDead == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.didStart == true)
            {
                if (didPause == false)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    didPause = true;
                }

                else if (didPause == true)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    didPause = false;
                }
            }

            if (didPause == false)
            {
                if (endReached == true)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                if (health <= 0)
                {
                    playerIsDead = true;
                }

                if (didElementSwap == true)
                {
                    WhatsEquipped();
                    didElementSwap = false;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (elementType == ElementType.NONE)
                    {
                        elementType = ElementType.FIRE;
                        didElementSwap = true;
                    }
                    else if (elementType == ElementType.FIRE)
                    {
                        elementType = ElementType.NONE;
                        didElementSwap = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (elementType == ElementType.NONE)
                    {
                        elementType = ElementType.FIRE;
                        didElementSwap = true;
                    }
                    else if (elementType == ElementType.FIRE)
                    {
                        elementType = ElementType.NONE;
                        didElementSwap = true;
                    }
                }

                if (didPause == false)
                {
                    fwdGun = gunPos.forward;

                    if (gunOut > 0)
                        gunOut -= Time.deltaTime;

                    playerAnimator.SetBool(Anim.GUN, gunOut > 0);

                    if (shootGun <= 0)
                    {
                        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("GunAim") && firstShot)
                        {
                            gunOut = 1.0f;
                            Shoot();
                            shootGun = fireRate;
                            firstShot = false;
                        }

                        if (Input.GetMouseButtonDown(1))
                        {
                            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("GunAim") != true)
                            {
                                playerAnimator.ResetTrigger("Holster");
                                playerAnimator.SetTrigger("Draw");
                                gunOut = 2.0f;
                                if (firstShot == false)
                                {
                                    firstShot = true;
                                }
                            }

                            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("GunAim") == true)
                            {
                                gunOut = 1.0f;
                                Shoot();
                                shootGun = fireRate;
                            }

                        }
                    }
                    if (shootGun > 0)
                    {
                        shootGun -= Time.deltaTime;
                    }
                    if (hitMelee <= 0)
                    {
                        playerAnimator.ResetTrigger("Attack");
                        didSmack = false;
                        PlayerMeleeDetection.canSmack = true;
                        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit1toHit2") == true)
                        {
                            playerAnimator.SetTrigger("HitToIdle");
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            MeleeAttack();
                            playerAnimator.ResetTrigger("HitToIdle");
                            hitMelee = meleeRate;
                        }
                    }
                    if (hitMelee > 0)
                    {
                        didSmack = true;
                        Debug.Log("Can't hit yet");
                        hitMelee -= Time.deltaTime;

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit1toHit2") == true || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit1") == true)
                            {
                                MeleeAttack();
                                playerAnimator.ResetTrigger("HitToIdle");
                                hitMelee = meleeRate;
                            }

                        }
                    }
                }
            }
        }
    }

    void MeleeAttack()
    {
        if (PlayerMovement.didJump == true)
        {
            playerAnimator.SetTrigger("JumpAttack");
        }
        else if (PlayerMovement.didJump != true)
        {
            playerAnimator.SetTrigger("Attack");
        }

    }

    public void Shoot()
    {
        playerAnimator.SetTrigger("Shoot");
        RaycastHit hit;
        if (Physics.Raycast(gunPos.position, fwdGun, out hit, Mathf.Infinity))
        {
            if (elementType == ElementType.FIRE)
            {
                fireShot.Play(true);
            }
            else if (elementType == ElementType.NONE)
            {
                noneShot.Play(true);
            }

            Debug.Log("Did Shoot");
            Debug.DrawRay(gunPos.position, fwdGun * hit.distance, Color.green);
            if (hit.collider.CompareTag("Enemy"))
            {
                if (hit.transform.gameObject.GetComponent<Enemy>())
                {
                    hit.transform.gameObject.GetComponent<Enemy>().enemyAnim.SetTrigger("Enemy_Hit");                    
                    if (elementType == ElementType.FIRE)
                    {
                        hit.transform.gameObject.GetComponent<Enemy>().startBurn = true;
                    }
                    else if (elementType == ElementType.NONE)
                    {
                        hit.transform.gameObject.GetComponent<Enemy>().enemyHealth -= gunDamage;
                    }
                    hit.transform.gameObject.GetComponent<Enemy>().wasHit = true;
                }
                else if (hit.transform.gameObject.GetComponentInParent<Enemy>())
                {
                    hit.transform.gameObject.GetComponentInParent<Enemy>().enemyAnim.SetTrigger("Enemy_Hit");
                    if (elementType == ElementType.FIRE)
                    {
                        hit.transform.gameObject.GetComponentInParent<Enemy>().startBurn = true;
                    }
                    else if (elementType == ElementType.NONE)
                    {
                        hit.transform.gameObject.GetComponentInParent<Enemy>().enemyHealth -= gunDamage;
                    }
                    hit.transform.gameObject.GetComponentInParent<Enemy>().wasHit = true;
                    
                }
                ScoreManager.Instance.combatScore += ScoreManager.Instance.gunAttackScore * ScoreManager.Instance.comboModifier;
                ScoreManager.Instance.hitCount += 1;
                Debug.Log("Hit an Enemy");

            }
        }
    }

    void WhatsEquipped()
    {
        if (elementType == ElementType.FIRE)
        {
            GunChange.Play(true);
            noneGun.gameObject.SetActive(false);
            fireGun.gameObject.SetActive(true);
        }
        if (elementType == ElementType.NONE)
        {
            GunChange.Play(true);
            fireGun.gameObject.SetActive(false);
            noneGun.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {

    }

}