using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{

    //References
    Rigidbody2D rbPlayer;
    PlayerMovement movement;
    PlayerAttack attack;

    //If you want items that modified this stat create with Stat class
    //public Stat speed;
    //public Stat attackRate;
    //public Stat attackRange;

    [Header("Attack Stats")]
    public float attackRate = 2f; 
    public float attackRange = 0.5f;

    [Header("Movement Stats")]
    public float speed = 12f;
    public float bounceforce = 10;
    Animator animator;
    int hitParamID;
    public GameObject VFXdamage;


    PlayerAttackInput input;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hitParamID = Animator.StringToHash("isHit");
        input = GetComponent<PlayerAttackInput>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public override void TakeDamage(int damage)
    {
        if (input.defenseHeld)
            return;

        base.TakeDamage(damage);
        
    }

    public void BeginDamage()
    {
        Time.timeScale = 0.5f;
        rbPlayer.velocity = Vector3.zero;
        float forceValue = 40f;
        rbPlayer.AddForce(Vector2.right * forceValue * -movement.direction, ForceMode2D.Impulse);
        movement.enabled = false;
        attack.enabled = false;
        
    }
    public void EndDamage()
    {
        Time.timeScale = 1f;
        movement.enabled = true;
        attack.enabled = true;
    }




    public override void  InstantianteEffect()
    {
        animator.SetTrigger(hitParamID);
        Instantiate(VFXdamage, transform.position, transform.rotation);
    }
}
