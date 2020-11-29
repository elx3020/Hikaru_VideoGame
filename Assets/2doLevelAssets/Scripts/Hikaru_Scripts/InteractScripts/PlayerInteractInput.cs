using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractInput : PlayerInput
{

    //references
    

    [HideInInspector] public bool grab;
    [HideInInspector] public bool lookleftRight;
    [HideInInspector] public bool lookUp;
    [HideInInspector] public bool lookDown;
    [HideInInspector] public bool drop;
    [HideInInspector] public float horizontal;
    [HideInInspector] public float vertical;
    [HideInInspector] public bool aim;

    public float threshold;


    public override void ClearInput()
    {

        if (!readyToClear)
            return;

        vertical = 0f;
        horizontal = 0f;
        grab = false;
        lookleftRight = false;
        lookUp = false;
        lookDown = false;
        drop = false;
        aim = false;
        readyToClear = false;


    }



    public override void ProcessInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        drop = drop || Input.GetButtonDown("Drop");

        grab = grab || Input.GetButtonDown("Grab");

        lookleftRight = lookleftRight || Mathf.Abs(vertical) <= threshold;
        lookUp = lookUp || vertical >= threshold;
        lookDown = lookDown || vertical <= -threshold;

        aim = aim || Input.GetButton("Aim");
        



    }


}
