// This script controls the animations of the Robbie player character. Normally, most
// of this functionality would be added to the PlayerMovement script instead of having
// its own script since that would be more efficient. For the purposes of learning,
// however, this functionality was separated out.

using UnityEngine;


public class PlayerMovementAnimations : MonoBehaviour
{
    PlayerMovement movement;    //Reference to the PlayerMovement script component
    Rigidbody2D rigidBody;      //Reference to the Rigidbody2D component
    PlayerMovementInput input;          //Reference to the PlayerInput script component
    Animator anim;              //Reference to the Animator component

    int hangingParamID;         //ID of the isHanging parameter
    int groundParamID;          //ID of the isOnGround parameter
    int crouchParamID;          //ID of the isCrouching parameter
    int speedParamID;           //ID of the speed parameter
    int fallParamID;            //ID of the verticalVelocity parameter
    int wallParamID;

    void Start()
    {
        //Get the integer hashes of the parameters. This is much more efficient
        //than passing strings into the animator
        hangingParamID = Animator.StringToHash("isHanging");
        groundParamID = Animator.StringToHash("isOnGround");
        speedParamID = Animator.StringToHash("speed");
        fallParamID = Animator.StringToHash("verticalVelocity");
        wallParamID = Animator.StringToHash("IsinWall"); 
        //Get references to the needed components
        movement = GetComponent<PlayerMovement>();
        rigidBody = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerMovementInput>();
        anim = GetComponent<Animator>();

        //If any of the needed components don't exist...
        if (movement == null || rigidBody == null || input == null || anim == null)
        {
            //...log an error and then remove this component
            Debug.LogError("A needed component is missing from the player");
            Destroy(this);
        }
    }

    void Update()
    {
        //Update the Animator with the appropriate values
        anim.SetBool(hangingParamID, movement.isHanging);
        anim.SetBool(wallParamID, movement.wallGrab);
        anim.SetBool(groundParamID, movement.isOnGround);
        anim.SetFloat(fallParamID, rigidBody.velocity.y);

        //Use the absolute value of speed so that we only pass in positive numbers
        anim.SetFloat(speedParamID, Mathf.Abs(rigidBody.velocity.x));
    }


    //EVENTS OF THE ANIMATIONS 
    //This method is called from events in the animation itself. This keeps the footstep
    //sounds in sync with the visuals
    public void StepAudio()
    {
        //Tell the Audio Manager to play a footstep sound
        //AudioManager.PlayFootstepAudio();
    }

    //This method is called from events in the animation itself. This keeps the footstep
    //sounds in sync with the visuals
    public void CrouchStepAudio()
    {
        //Tell the Audio Manager to play a crouching footstep sound
        //AudioManager.PlayCrouchFootstepAudio();
    }
}
