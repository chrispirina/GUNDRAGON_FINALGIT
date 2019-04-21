using UnityEngine;

public class Player : MonoBehaviour
{
    public ElementType currentElement = ElementType.NONE;
    public GameObject[] gunTypes = { };

    public float Health
    {
        get => health;
        set {
            health = Mathf.Clamp(value, 0F, maxHealth);
            UpdateHealth();
        }
    }
    [SerializeField]
    [Readonly]
    private float health;
    public float maxHealth = 200F;

    [Header("State")]
    public GameObject spear;
    public GameObject sword;
    public bool gotSpear;
    private bool weaponID = false;
    [Readonly]
    public bool isDead;

    [Header("Anchors")]
    public Transform gunAnchor;
    public Transform meleeAnchor;

    [Header("Properties")]
    public float gunDamage = 5F;

    [Header("Particles")]
    public ParticleSystem[] elementalParticles = { };
    public ParticleSystem gunChangeParticles;

    private float gunOut;
    
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Health = maxHealth;
        weaponID = false;
        spear = GameObject.FindGameObjectWithTag("spear");
        sword = GameObject.FindGameObjectWithTag("sword");
        spear.SetActive(false);
        sword.SetActive(false);
    }

    private void Update()
    {
        if (weaponID == false)
        {
            sword.SetActive(true);
            spear.SetActive(false);
        }

        if (isDead)
        {
            GameManager.Instance.requireCursor = true;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager gm = GameManager.Instance;

            if (gm)
                gm.IsPaused = !gm.IsPaused;
        }

        if (GameManager.Instance.IsPaused)
            return;

        if(Input.GetKeyDown(KeyCode.E))
            CycleElement(1);
        if(Input.GetKeyDown(KeyCode.Q))
            CycleElement(-1);

        if (gunOut > .0F)
            gunOut -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1)) {
            animator.SetTrigger(Anim.SHOOT);
            gunOut = 2F;
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger(Anim.ATTACK);
        }

        animator.SetBool(Anim.GUN, gunOut > .0F);
    }

    private void OnValidate()
    {
        int elementCount = System.Enum.GetValues(typeof(ElementType)).Length;

        if (gunTypes.Length != elementCount)
            System.Array.Resize(ref gunTypes, elementCount);
        if (elementalParticles.Length != elementCount)
            System.Array.Resize(ref elementalParticles, elementCount);
    }

    private void UpdateHealth()
    {
        if (Health <= .0F)
            isDead = true;
    }

    private void CycleElement(int direction)
    {
        int element = (int)currentElement + direction;
        int elementCount = System.Enum.GetValues(typeof(ElementType)).Length;
        if (element >= elementCount)
            element = 0;
        else if (element < 0)
            element = elementCount - 1;
        currentElement = (ElementType)element;
    }

    public void Shoot()
    {
        gunOut = 1F;

        if(Physics.Raycast(gunAnchor.position, gunAnchor.forward, out RaycastHit hit))
        {
            if (elementalParticles[(int)currentElement])
                elementalParticles[(int)currentElement].Play();

            Debug.DrawRay(gunAnchor.position, gunAnchor.forward * hit.distance, Color.green);
            if (!hit.collider.CompareTag("Enemy"))
                return;

            EnemyHealth enemy = hit.collider.attachedRigidbody.GetComponent<EnemyHealth>();

            if (!enemy)
                return;

            switch(currentElement)
            {
                case ElementType.NONE:
                    enemy.Health -= gunDamage;
                    break;
                case ElementType.FIRE:
                    enemy.startBurn = true;
                    break;
            }

            enemy.wasHit = true;

            ScoreManager.Instance.combatScore += ScoreManager.Instance.gunAttackScore * ScoreManager.Instance.comboModifier;
            ScoreManager.Instance.hitCount++;
        }
    }

    public enum ElementType : int
    {
        NONE = 0,
        FIRE = 1
        //WATER = 2,
        //AIR = 3,
        //EARTH = 4
    }
}
