using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXIObjects : MonoBehaviour
{
   PlayerMovement playerCheck;
    ParticleSystem dust;
    Rigidbody2D rb;
    Collider2D collider2d;
    public LayerMask ground;
    private bool touchingGround;
    private bool playDust;
    public bool isGrabbed = false;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        dust = GetComponentInChildren<ParticleSystem>();
        collider2d = GetComponentInParent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        touchingGround = collider2d.IsTouchingLayers(ground);
        if (!isGrabbed && touchingGround)
        {
            if (Mathf.Abs(rb.velocity.x) >= 0.1f && Mathf.Abs(rb.velocity.y) <= 0.1f && !playDust)
            {
                dust.Play();
                playDust = true;
                
            }
            else if (Mathf.Abs(rb.velocity.x) <= 0.01f && playDust)
            {
                Invoke("StopDust", 0.05f);
            }

        }
        else
        {

            if (Mathf.Abs(rb.velocity.x) <= 0.01f && playDust)
            {
                Invoke("StopDust", 0.05f);
            }
        }

        
        
    }


    void StopDust()
    {
        dust.Stop();
        playDust = false;
    }


}
