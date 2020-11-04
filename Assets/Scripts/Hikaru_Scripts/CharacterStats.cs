using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;

    //Stats that are not changed by an item
    public int currentHealth { get; private set; }

    //Stats that are changed by an item
    public Stat damage;
    public Stat armor;

    
    void Awake()
    {
        
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
       

        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage,0, int.MaxValue);

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, int.MaxValue);

        Debug.Log(transform.name + "takes " + damage + " damage.");
        Debug.Log(transform.name + " current health: " + currentHealth);

        InstantianteEffect();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Die in some way
        // This method is meant to be overwrittean
        Debug.Log(transform.name + " died.");
    }

    public virtual void InstantianteEffect()
    {
        Debug.Log(transform.name + " instante hurt effects.");
    }
}
