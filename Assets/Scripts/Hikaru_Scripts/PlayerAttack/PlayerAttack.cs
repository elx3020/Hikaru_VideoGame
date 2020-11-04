using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //References
    PlayerAttackInput input;
    PlayerMovement movement;
    AttackEffects effects;
    PlayerStats stats;
    Rigidbody2D rbPlayer;
    
    //Position for the attack point
    //Three attack points
    //1. left_right_attack
    //2. up_attack
    //3. down_attack
    public Transform[] attackPoints;

    //detect only object that area in this layer
    [SerializeField] private LayerMask destroyObjectLayer;
    //Detect enemies to applied damage
    Collider2D[] hitEnemies;

    private float nextAttackTime = 0f;
    private float nextAttackPointTime = 0f;
    public int damage;
    void Start()
    {
        input = GetComponent<PlayerAttackInput>();
        movement = GetComponent<PlayerMovement>();
        effects = GetComponent<AttackEffects>();
        stats = GetComponent<PlayerStats>();
        rbPlayer = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        MainAttack();

    }



    void MainAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            //Controlling the three types of attack
            if (input.leftRightAttack)
            {
                EnemyTakeDamage(attackPoints[0].position);
                effects.CreateHitEffect(attackPoints[0].position);
                nextAttackTime = Time.time + 1 / stats.attackRate;

            }
            else if (input.upAttack)
            {
                EnemyTakeDamage(attackPoints[1].position);
                effects.CreateHitEffect(attackPoints[1].position, new Vector3(0f, 0f, 90f));
                nextAttackTime = Time.time + 1 / stats.attackRate;
                
            }
            else if (input.downAttack && !movement.isOnGround)
            {
                EnemyTakeDamage(attackPoints[2].position);
                effects.CreateHitEffect(attackPoints[2].position, new Vector3(0f, 0f, -90f));
                AddForceVerticalAttack();
                nextAttackTime = Time.time + 1 / stats.attackRate;
            }
        }
    }

    void AddForceVerticalAttack()
    {
        //check if hit enemy or certain object to bounce
        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.gameObject.tag == "Enemy1")
            {
                rbPlayer.velocity = Vector3.zero;
                rbPlayer.AddForce(Vector2.up * stats.bounceforce, ForceMode2D.Impulse);
            }

        }

        
    }


    void EnemyTakeDamage(Vector2 attackPosition)
    {
        //Detect enemies in range of attack
        hitEnemies = Physics2D.OverlapCircleAll(attackPosition, stats.attackRange, destroyObjectLayer);

        // Damage the enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            //enemy.GetComponent<EnemyStats>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < attackPoints.Length; i++)
        {
            if (attackPoints[i] == null)
                continue;
            if (stats != null)
                Gizmos.DrawWireSphere(attackPoints[i].position, stats.attackRange);
        }

    }


}
