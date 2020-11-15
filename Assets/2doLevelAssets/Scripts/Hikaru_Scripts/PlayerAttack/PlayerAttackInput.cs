using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackInput : PlayerInput
{


    //For main Attack
    [HideInInspector] public bool attackHeld;
    [HideInInspector] public bool attackInstant;
    [HideInInspector] public bool leftRightAttack;    
    [HideInInspector] public bool downAttack;
    [HideInInspector] public bool upAttack;
    [HideInInspector] public float vertical;


    //For main defense
    [HideInInspector] public bool defenseHeld;
    [HideInInspector] public bool defenseInstant;


    public float attackSensitivity = 0.5f;

    public override void ClearInput()
    {
        //If we're not ready to clear input, exit
        if (!readyToClear)
            return;

        //Reset all inputs
        //For attack
        vertical = 0f;
        attackHeld = false;
        attackInstant = false;
        leftRightAttack = false;
        upAttack = false;
        downAttack = false;
        readyToClear = false;

        //For defense
        defenseHeld = false;
        defenseInstant = false;

    }

    public override void ProcessInputs()
    {

        //For Main Attack
        //Accumulate horizontal axis input
        vertical += Input.GetAxis("Vertical");
        //For quick attacks
        attackInstant = attackInstant || Input.GetButtonDown("Attack");
 
        //Accumulate button inputs
        attackHeld = attackHeld || Input.GetButton("Attack");

        leftRightAttack = leftRightAttack || attackInstant && vertical >=-attackSensitivity && vertical < attackSensitivity;
        upAttack = upAttack || (attackInstant && vertical >= attackSensitivity);
        downAttack = downAttack || (attackInstant && vertical <= -attackSensitivity);

        //For Defense

        defenseHeld = defenseHeld || Input.GetButton("Defence");
        defenseInstant = defenseInstant || Input.GetButtonDown("Defence");


    }
}
