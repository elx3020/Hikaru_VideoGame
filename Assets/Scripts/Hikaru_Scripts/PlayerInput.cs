// This script handles inputs for the player. It serves two main purposes: 1) wrap up
// inputs so swapping between mobile and standalone is simpler and 2) keeping inputs
// from Update() in sync with FixedUpdate()

using UnityEngine;

//We first ensure this script runs before all other player scripts to prevent laggy
//inputs
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public bool readyToClear;                              //Bool used to keep input in sync

    void Update()
    {
        //Clear out existing input values
        ClearInput();

        //If the Game Manager says the game is over, exit
        //if (GameManager.IsGameOver())
        //    return;

        //Process keyboard, mouse, gamepad (etc) inputs
        ProcessInputs();

        //Add some extra functionality
        ExtraFunction();
    }

    void FixedUpdate()
    {
        //In FixedUpdate() we set a flag that lets inputs to be cleared out during the 
        //next Update(). This ensures that all code gets to use the current inputs
        readyToClear = true;
    }

    public virtual void ClearInput()
    {
        //Clear the Inut in each update
    }

    public virtual void ProcessInputs()
    {
        //Get the inputs
    }

    public virtual void ExtraFunction()
    {
        //Add extra behavior
    }

}
