// This script controls the player's movement and physics within the game

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool drawDebugRaycasts = true;   //Should the environment checks be visualized
    [Space]
    [Header("Movement Properties")]
    public float speed = 8f;                //Player speed
    public float coyoteDuration = .05f;     //How long the player can jump after falling
    public float maxFallSpeed = -25f;       //Max speed player can fall
    [Space]
    [Header("Jump Properties")]
    public float jumpForce = 6.3f;          //Initial force of jump
    public Vector2 wallJumpForce;
    [Space]
    [Header("Environment Check Properties")]
    public Vector2 footPosition;            //Foot position
    public float footOffset = .4f;          //X Offset of feet raycast
    public Vector2 headPosition;            //Head position
    public float bodyPosition;
    public float headClearance = .5f;       //Space needed above the player's head
    public float groundDistance = .2f;      //Distance player is considered to be on the ground
    public float grabDistance = .4f;        //The reach distance for wall grabs
    public LayerMask groundLayer;           //Layer of the ground
    [Space]
    [Header("Input Check")]
    public float horizontal;
    public float vertical;
    [Space]
    [Header("Status Flags")]
    public bool isOnGround;                 //Is the player on the ground?
    public bool isTotallyOnGround;
    public bool isJumping;                  //Is player jumping?
    public bool isHanging;                  //Is player hanging?
    public bool isHeadBlocked;
    public bool isFacingWall;
    public bool isWallJumping;
    public bool wallGrab;
    public bool ledgeReach;
    [Space]
    [Header("Event Flags")]
    public bool groundTouch;
    public bool canMove = true;
    PlayerMovementInput input;              //The current inputs for the player
    CapsuleCollider2D bodyCollider;         //The collider component
    Rigidbody2D rigidBody;                  //The rigidbody component
    PlayerStats stats;

    float jumpTime;                         //Variable to hold jump duration
    float coyoteTime;                       //Variable to hold coyote duration

    float originalXScale;                   //Original scale on X axis
    public int direction = 1;                      //Direction player is facing
    //A small amount used for hanging position


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
        if (canMove) {
            VerticalMovement();
            GroundMovement();
            WallGrab();
            MidAirMovement();
        }
       
        //Process ground and air movements
        
        
    }

    void FixedUpdate()
    {
        
        //Check the environment to determine status
        PhysicsCheck();
    }

    void PhysicsCheck()
    {
 
        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0) + footPosition, Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0) + footPosition, Vector2.down, groundDistance);

        //If either ray hit the ground, the player is on the ground

        isOnGround = leftCheck || rightCheck;
        isTotallyOnGround = leftCheck && rightCheck;
        
        //Cast the ray to check above the player's head
        RaycastHit2D headCheck = Raycast(headPosition, Vector2.up, headClearance);

        //If that ray hits, the player's head is blocked
        isHeadBlocked = headCheck;

        //Determine the direction of the wall grab attempt
        Vector2 grabDir = new Vector2(direction, 0f);

        //Cast three rays to look for a wall grab
        
        //Wall and Ledge Check
        // Use a rays to know if first player is facing a wall
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, bodyPosition), grabDir, grabDistance);
        isFacingWall = wallCheck;
        //Use another ray at the head position to know if a ledge is reached
        RaycastHit2D ledgeCheck = Raycast(new Vector2(footOffset * direction, headPosition.y), grabDir, grabDistance);
        if(isFacingWall)
        ledgeReach = !ledgeCheck;
        

    

        

        //RaycastHit2D middle_wallJumCheck = Raycast(new Vector2(footOffset * direction, 0f), grabDir, wallJumpDistance);

        //Create raycast to detect wall
        
        
        //if is in the air AND not hanging and also it is not ready to wall jump while checking if right foot and head or left foot touch the wall
        //and if the velocity is less than one and the player is holding the grab botton the can wall jump 
        



        

    }

    void GroundMovement()
    {
        //horizontal movement input storage
        horizontal = input.horizontal;

        //vertical movement input storage
        

        //If currently hanging, the player can't move to exit
        if (isHanging || wallGrab)
            return;


        //Calculate the desired velocity based on inputs
        
            float xVelocity = speed * input.horizontal;
            //float xVelocity = speed * direction;

            //If the sign of the velocity and direction don't match, flip the character
            if (xVelocity * direction < 0f)
                FlipCharacterDirection();

            //Apply the desired velocity 
            rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
       
        
       

        //If the player is on the ground, extend the coyote time window



        if (isOnGround)
        {
            coyoteTime = Time.time + coyoteDuration;
            
        }

        if (!isFacingWall)
        {
            isWallJumping = false;
        }

        if (isOnGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!isOnGround && groundTouch)
        {
            groundTouch = false;
        }



        void GroundTouch()
        {
            isJumping = false;

        }


    }




    void VerticalMovement()
    {
        vertical = input.vertical;

        if (isOnGround || !wallGrab) return;

        //float xVelocity = speed * direction;
        float yVelocity = speed * input.vertical;

        if (!ledgeReach)
        {
           
            //Apply the desired velocity 
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, yVelocity);
        }
        else
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(yVelocity, -10f, 0f));
        }
        


    }


    void WallGrab()
    {
        if (!wallGrab && isFacingWall && rigidBody.velocity.y <= 1 && input.wallHold)
        {

            rigidBody.gravityScale = 0;
            wallGrab = true;
           

        }//check reasons to stop walljump
        else if (isOnGround || isHanging || !isFacingWall || !input.wallHold)
        {
            wallGrab = false;
            rigidBody.gravityScale = 5;

        }
    }

    



    void MidAirMovement()
    {

        //If the jump key is pressed AND the player isn't already jumping AND EITHER
        //the player is on the ground or within the coyote time window...
        if (input.jumpPressed && !isJumping && (isOnGround || coyoteTime > Time.time ))
        {

            isJumping = true;

            //...add the jump force to the rigidbody...
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,0f);
            rigidBody.velocity += Vector2.up * jumpForce;

            //...and tell the Audio Manager to play the jump audio
            //AudioManager.PlayJumpAudio();
        }
       
        //If jump is pressed and can wall jump with coyote time
        if (input.jumpPressed && wallGrab)
        {

            //...let go..
            rigidBody.velocity = Vector2.zero;
            if(Mathf.Abs(horizontal + direction) < 1)
            {
                rigidBody.velocity += new Vector2(wallJumpForce.x * -direction, wallJumpForce.y);

            }
            else
            {

                rigidBody.velocity += new Vector2(rigidBody.velocity.x, wallJumpForce.y);

                
                Debug.Log("Jumping");
            }

            //...and exit
            isWallJumping = true;
            wallGrab = false;
            canMove = false;
            rigidBody.gravityScale = 5;
            Invoke("RestoreMove", 0.2f);

        }

        //If player is falling to fast, reduce the Y velocity to the max
        if (rigidBody.velocity.y < maxFallSpeed)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
    }


    void RestoreMove()
    {
        //rigidBody.velocity = Vector2.zero;
        canMove = true;
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
