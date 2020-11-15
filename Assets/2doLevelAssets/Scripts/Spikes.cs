
using UnityEngine;
using Cinemachine;

public class Spikes : MonoBehaviour
{
    //CinemachineCollisionImpulseSource shakeHit;

    private void Start()
    {
        //shakeHit = GetComponent<CinemachineCollisionImpulseSource>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(1);
        }
    }
}
