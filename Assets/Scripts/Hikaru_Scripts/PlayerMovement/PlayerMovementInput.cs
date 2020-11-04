using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]

public class PlayerMovementInput : PlayerInput
{
    [HideInInspector] public float horizontal;      //Float that stores horizontal input
    [HideInInspector] public float vertical;        //Float that stores vertical input
    [HideInInspector] public bool jumpHeld;         //Bool that stores jump pressed
    [HideInInspector] public bool jumpPressed;      //Bool that stores jump held
    [HideInInspector] public bool wallHold;
    

   
    public override void ClearInput()
    {
        //If we're not ready to clear input, exit
        if (!readyToClear)
            return;

        // Reset all inputs
        horizontal = 0f;
        vertical = 0f;
        jumpPressed = false;
        jumpHeld = false;
        wallHold = false;
        

        readyToClear = false;
    }

    public override void ProcessInputs()
    {
       

        //Accumulate horizontal axis input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //Accumulate button inputs
        jumpPressed = jumpPressed || Input.GetButtonDown("Jump");
        jumpHeld = jumpHeld || Input.GetButton("Jump");
        wallHold = wallHold || Input.GetButton("WallHold");

    }

    public override void ExtraFunction()
    {
        //Clamp the horizontal input to be between -1 and 1
        //horizontal = Mathf.Clamp(horizontal, -1f, 1f);
    }

    


}
