using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffects : MonoBehaviour
{
    public GameObject attackEffects;
    
    public void CreateHitEffect(Vector2 attackPosition, Vector3 rotation)
    {
        GameObject hitEfftect = Instantiate(attackEffects, attackPosition, Quaternion.Euler(rotation));
        hitEfftect.transform.parent = transform;
    }

    public void CreateHitEffect(Vector2 attackPosition)
    {
        GameObject hitEfftect = Instantiate(attackEffects, attackPosition, transform.rotation);
        hitEfftect.transform.parent = transform;

    }

}
