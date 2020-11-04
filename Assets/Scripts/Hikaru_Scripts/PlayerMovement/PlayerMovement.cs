// This script controls the player's movement and physics within the game

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool drawDebugRaycasts = true;   //Should the environment checks be visualized

    [Header("Movement Properties")]
    public float speed = 8f;                //Player speed
    public float coyoteDuration = .05f;     //How long the player can jump after falling
    public float maxFallSpeed = -25f;       //Max speed player can fall

    [Header("Jump Properties")]
    public float jumpForce = 6.3f;          //Initial force of jump
    public float hangingJumpForce = 15f;    //Force of wall hanging jumo
    public float jumpHoldForce = 1.9f;      //Incremental force when jump is held
    public float jumpHoldDuration = .1f;    //How long the jump key can be held
    public Vector2 wallJumpForce;
    public float wallJumpDistance = 0.35f;  //Control the distance of Ray Cast when jump from the wall
    public int extraJumpsValue = 1;         //
    public int extraJumps;

    [Header("Environment Check Properties")]
    public Vector2 footPosition;            //Foot position
    public float footOffset = .4f;          //X Offset of feet raycast
    public Vector2 headPosition;            //Head position
    public float eyeHeight = 1.5f;          //Height of wall checks
    public float reachOffset = .7f;         //X offset for wall grabbing
    public float headClearance = .5f;       //Space needed above the player's head
    public float groundDistance = .2f;      //Distance player is considered to be on the ground
    public float grabDistance = .4f;        //The reach distance for wall grabs
    public LayerMask groundLayer;           //Layer of the ground

    [Header("Input Check")]
    public float horizontal;
    public float vertical;

    [Header("Status Flags")]
    public bool isOnGround;                 //Is the player on the ground?
    public bool isTotallyOnGround;
    public bool isJumping;                  //Is player jumping?
    public bool isHanging;                  //Is player hanging?
    public bool isHeadBlocked;
    public bool canWallJump;
    

    PlayerMovementInput input;              //The current inputs for the player
    CapsuleCollider2D bodyCollider;         //The collider component
    Rigidbody2D rigidBody;                  //The rigidbody component
    PlayerStats stats;

    float jumpTime;                         //Variable to hold jump duration
    float coyoteTime;                       //Variable to hold coyote duration
    float coyoteTimeWall;

    float originalXScale;                   //Original scale on X axis
    public int direction = 1;                      //Direction player is facing

    const float smallAmount = .2f;         //A small amount used for hanging position


    void Start()
    {
        //Get a reference to the required components
        input = GetComponent<PlayerMovementInput>();
        rigidBody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        stats = GetComponent<PlayerStats>();
        

        //Record the original x scale of the player
        originalXScale = transform.localScale.x;

    }


    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        
        //Check the environment to determine status
        PhysicsCheck();

        VerticalMovement();
        //Process ground and air movements
        GroundMovement();
        MidAirMovement();

    }

    void PhysicsCheck()
    {
        //Start by assuming the player isn't on the ground and the head isn't blocked
        isOnGround = false;
        isHeadBlocked = false;
        isTotallyOnGround = false;

        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0) + footPosition, Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0) + footPosition, Vector2.down, groundDistance);

        //If either ray hit the ground, the player is on the ground
        if (leftCheck || rightCheck)
            isOnGround = true;
        if (leftCheck && rightCheck)
            isTotallyOnGround = true;

        //Cast the ray to check above the player's head
        RaycastHit2D headCheck = Raycast(headPosition, Vector2.up, headClearance);

        //If that ray hits, the player's head is blocked
        if (headCheck)
            isHeadBlocked = true;

        //Determine the direction of the wall grab attempt
        Vector2 grabDir = new Vector2(direction, 0f);

        //Cast three rays to look for a wall grab
        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, headPosition.y), grabDir, grabDistance);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, headPosition.y), Vector2.down, grabDistance);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance);






        //Ledge Grab
        //If the player is off the ground AND is not hanging AND is falling AND
        //found a ledge AND found a wall AND the grab is NOT blocked...
        // && rigidBody.velocity.y < 0f change made
        //if (!isOnGround && !isHanging && rigidBody.velocity.y < -1f &&
        //    ledgeCheck && wallCheck && !blockedCheck)
        //{
        //    //...we have a ledge grab. Record the current position...
        //    Vector3 pos = transform.position;
        //    //...move the distance to the wall (minus a small amount)...
        //    pos.x += (wallCheck.distance - smallAmount) * direction;
        //    //...move the player down to grab onto the ledge...
        //    pos.y -= ledgeCheck.distance;
        //    //...apply this position to the platform...
        //    transform.position = pos;
        //    //...set the rigidbody to static...
        //    rigidBody.bodyType = RigidbodyType2D.Static;
        //    //...finally, set isHanging to true
        //    isHanging = true;
        //    //..flip direction
        //    //FlipCharacterDirection();
        //}







        //WallClimb and Jump Ray Cast

        //RaycastHit2D middle_wallJumCheck = Raycast(new Vector2(footOffset * direction, 0f), grabDir, wallJumpDistance);

        //Create raycast to detect wall
        RaycastHit2D footRight_wallJumpCheck = Raycast(new Vector2(footOffset * direction, footPosition.y), grabDir, wallJumpDistance);
        RaycastHit2D footLeft_wallJumpCheck = Raycast(new Vector2(-footOffset * direction, footPosition.y), -grabDir, wallJumpDistance);
        
        //if is in the air AND not hanging and also it is not ready to wall jump while checking if right foot and head or left foot touch the wall
        //and if the velocity is less than one and the player is holding the grab botton the can wall jump 
        if (!isOnGround && !isHanging && !canWallJump && footRight_wallJumpCheck && rigidBody.velocity.y <= 1 && input.wallHold)
        {

            rigidBody.bodyType = RigidbodyType2D.Kinematic;
            
            canWallJump = true;
            
        }//check reasons to stop walljump
        else if(isOnGround || isHanging || !footRight_wallJumpCheck || !input.wallHold)
        {
            canWallJump = false;
            rigidBody.bodyType = RigidbodyType2D.Dynamic;
        } 

    }

    void GroundMovement()
    {
        //horizontal movement input storage
        horizontal = input.horizontal;

        //vertical movement input storage
        

        //If currently hanging, the player can't move to exit
        if (isHanging || canWallJump)
            return;


        //Calculate the desired velocity based on inputs
        if (input.horizontal >= 0.5f || input.horizontal <= -0.5f)
        {
            float xVelocity = speed * input.horizontal;
            //float xVelocity = speed * direction;

            //If the sign of the velocity and direction don't match, flip the character
            if (xVelocity * direction < 0f)
                FlipCharacterDirection();

            //Apply the desired velocity 
            rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
        }
       

        //If the player is on the ground, extend the coyote time window
        if (isOnGround)
        {
            extraJumps = 0;
            coyoteTime = Time.time + coyoteDuration;
        }


    }




    void VerticalMovement()
    {
        vertical = input.vertical;

        if (isOnGround || !canWallJump) return;

        float yVelocity = speed * input.vertical;
        //float xVelocity = speed * direction;


        //Apply the desired velocity 
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, yVelocity);




    }




    void MidAirMovement()
    {
        //If the player is currently hanging...
        if (isHanging)
        {
            

            //If jump is pressed...
            if (input.jumpPressed)
            {
                //...let go...
                isHanging = false;
                //...set the rigidbody to dynamic and apply a jump force...
                rigidBody.bodyType = RigidbodyType2D.Dynamic;
                rigidBody.AddForce(new Vector2(0f, hangingJumpForce), ForceMode2D.Impulse);
                //...and exit
                return;
            }
        }

       
        //If the jump key is pressed AND the player isn't already jumping AND EITHER
        //the player is on the ground or within the coyote time window...
        if (input.jumpPressed && !isJumping && !canWallJump && (isOnGround || coyoteTime > Time.time ))
        {

            //...The player is no longer on the ground and is jumping...
            isTotallyOnGround = false;
            isOnGround = false;
            isJumping = true;

            //...record the time the player will stop being able to boost their jump...
            jumpTime = Time.time + jumpHoldDuration;

            //...add the jump force to the rigidbody...
            rigidBody.velocity = Vector3.zero;
            rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            //if (extraJumps < extraJumpsValue && !isOnGround && !(coyoteTime > Time.time))
            //{
            //    extraJumps++;
            //}
            //...and tell the Audio Manager to play the jump audio
            //AudioManager.PlayJumpAudio();
        }
        else if (isJumping)
        {
            //...and the jump button is held, apply an incremental force to the rigidbody...
            if (input.jumpHeld)
                rigidBody.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);

            //...and if jump time is past, set isJumping to false
            if (jumpTime <= Time.time)
            {
                isJumping = false;
            }

        }
        //If jump is pressed and can wall jump with coyote time
        if (input.jumpPressed && canWallJump)
        {

            //...let go..
            rigidBody.velocity = Vector3.zero;
            rigidBody.drag = 0;
            rigidBody.AddForce(wallJumpForce,ForceMode2D.Impulse);
            //...and exit
            isJumping = true;
            canWallJump = false;
     
        }

        //If player is falling to fast, reduce the Y velocity to the max
        if (rigidBody.velocity.y < maxFallSpeed)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
    }

    void FlipCharacterDirection()
    {
        //Turn the character by flipping the direction
        direction *= -1;

        //Flip the parent
        transform.Rotate(0f, 180f, 0f);
;
    }


    //These two Raycast methods wrap the Physics2D.Raycast() and provide some extra
    //functionality
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        //Call the overloaded Raycast() method using the ground layermask and return 
        //the results
        return Raycast(offset, rayDirection, length, groundLayer);
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
    {
        //Record the player's position
        Vector2 pos = transform.position;

        //Send out the desired raycasr and record the result
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

        //If we want to show debug raycasts in the scene...
        if (drawDebugRaycasts)
        {
            //...determine the color based on if the raycast hit...
            Color color = hit ? Color.red : Color.green;
            //...and draw the ray in the scene view
            Debug.DrawRay(pos + offset, rayDirection * length, color);
        }

        //Return the results of the raycast
        return hit;
    }

}
