using UnityEngine;
using Cinemachine;

public class PlayerStats : CharacterStats
{
    //References
    Rigidbody2D rbPlayer;
    PlayerMovement movement;
    PlayerAttack attack;
    public HealthBar healthBar;
    CinemachineImpulseSource shakeCamera;
    //If you want items that modified this stat create with Stat class
    //public Stat speed;
    //public Stat attackRate;
    //public Stat attackRange;


    [Space]
    [Header("Attack Stats")]
    public float attackRate = 2f;
    public float attackRange = 0.5f;
    [Space]
    [Header("Movement Stats")]
    public float stamina = 3;
    public float speed = 12f;
    public float bounceforce = 10;
    public bool isInvulnerable = false;
    public float invulnerableTime = 0.125f;
    Animator animator;
    int hitParamID;
    public GameObject VFXdamage;
    PlayerAttackInput input;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        hitParamID = Animator.StringToHash("isHit");
        input = GetComponent<PlayerAttackInput>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        healthBar.SetMaxHealth(maxHealth);
        shakeCamera = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public override void TakeDamage(int damage)
    {
        //if (input.defenseHeld)
        //    return;
        if (isInvulnerable) return;

        base.TakeDamage(damage);
        healthBar.SetHealth(currentHealth);
        BeginDamage();
        Invoke("EndDamage", invulnerableTime);

        
    }

    public void BeginDamage()
    {
        Time.timeScale = 0.8f;
        shakeCamera.GenerateImpulse();
        isInvulnerable = true;
    }

    public void EndDamage()
    {
        Time.timeScale = 1f;
        isInvulnerable = false;
    }




    public override void  InstantianteEffect()
    {
        animator.SetTrigger(hitParamID);

        //Instantiate(VFXdamage, transform.position, transform.rotation);
    }
}
