using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXIObjects : MonoBehaviour
{
   PlayerMovement playerCheck;
    ParticleSystem dust;
    Rigidbody2D rb;
    private bool playDust;
    public bool isGrabbed = false;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        dust = GetComponentInChildren<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!isGrabbed)
        {
            if (Mathf.Abs(rb.velocity.x) >= 0.1f && Mathf.Abs(rb.velocity.y) <= 0.1f && !playDust)
            {
                dust.Play();
                playDust = true;
            }
            else if (Mathf.Abs(rb.velocity.x) <= 0.01f && playDust)
            {
                Invoke("StopDust", 0.1f);
            }

        }
        else
        {

            if (Mathf.Abs(rb.velocity.x) <= 0.01f && playDust)
            {
                Invoke("StopDust", 0.1f);
            }
        }

        
        
    }


    void StopDust()
    {
        dust.Stop();
        playDust = false;
    }


}
