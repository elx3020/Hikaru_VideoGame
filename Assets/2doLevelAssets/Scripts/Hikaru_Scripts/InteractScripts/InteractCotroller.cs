
using UnityEngine;

public class InteractCotroller : MonoBehaviour
{
    Rigidbody2D rigidBody;
    PlayerMovement playerCheck;
    PlayerInteractInput interactInput;
    Vector2 playerSize;


    public Collider2D[] objectGrab;
    public LayerMask objects;
    public SpriteRenderer playerSprite;
    public GameObject grabbedobject;
    public Transform grabPoint;
    public float radius = .5f;
    public bool isGrabing = false;
    public bool posibleGrab = false;
    public bool isAiming;
    public float throwForce;
    public float smallAmount;
    public float offset;



    private Vector2 sizeObject;
    private float initialheadCleareance;
    private Vector2 possibleObjectSize;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerCheck = GetComponent<PlayerMovement>();
        initialheadCleareance = playerCheck.headClearance;
        interactInput = GetComponent<PlayerInteractInput>();
    }

    // Update is called once per frame
    void Update()
    {
        playerSize = playerSprite.bounds.size;


        LookingDirection();
        Aim();
        Invoke("Grabbing",0.05f);
        Throw();

        //if(objectGrab.collider != null && objectGrab.collider.tag == "IObjects")
        //{
        //    possibleObjectSize = objectGrab.collider.gameObject.GetComponent<SpriteRenderer>().bounds.size;
        //    if(initialheadCleareance == playerCheck.headClearance)
        //    {
        //        float headspace = playerCheck.headClearance + possibleObjectSize.y;
        //        playerCheck.headClearance = headspace;
        //    }
        //}
        //else
        //{
        //    possibleObjectSize = Vector2.zero;
        //    playerCheck.headClearance = initialheadCleareance;
        //}

        //if (objectGrab.collider != null && objectGrab.collider.tag == "IObjects" && !playerCheck.isHeadBlocked && grabbedobject == null)
        //{
        //    posibleGrab = true;
        //}
        //else
        //{
        //    posibleGrab = false;
        //}


        //if(grabbedobject != null)
        //{
        //    grabbedobject.GetComponent<Transform>().transform.localScale = new Vector3(playerCheck.direction, 1f, 1f);
        //}


    }


    void LookingDirection()
    {
        if (!isGrabing)
        {
            objectGrab = Physics2D.OverlapCircleAll(grabPoint.position, radius, objects);
            if ((Mathf.Abs(interactInput.horizontal) >= 0) && !interactInput.lookUp && !interactInput.lookDown)
            {
                if (playerCheck.direction > 0)
                {
                    grabPoint.position = new Vector2((transform.position.x + playerSize.x / 2 - offset), transform.position.y);
                }
                else
                {
                    grabPoint.position = new Vector2((transform.position.x - playerSize.x / 2 + offset ), transform.position.y);
                }

            }
            else if (interactInput.lookUp)
            {
                grabPoint.position = new Vector2(transform.position.x, transform.position.y + playerSize.y / 2 - offset);

            }
            else if (interactInput.lookDown)
            {
                grabPoint.position = new Vector2(transform.position.x, transform.position.y - playerSize.y / 2 - offset);

            }

        }


    }

        
    void Grabbing()
    {
        if (interactInput.grab && !isGrabing && objectGrab.Length > 0)
        {
            grabbedobject = objectGrab[0].gameObject;
            sizeObject = grabbedobject.GetComponent<SpriteRenderer>().bounds.size;
            Vector2 targetPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (playerSize.y / 2) + (sizeObject.y / 2) - smallAmount);
            grabbedobject.GetComponent<Collider2D>().isTrigger = true;
            grabbedobject.transform.position = targetPosition;
            grabbedobject.GetComponentInChildren<VFXIObjects>().isGrabbed = true;
            grabbedobject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            grabbedobject.GetComponent<FixedJoint2D>().enabled = true;
            grabbedobject.GetComponent<FixedJoint2D>().connectedBody = rigidBody;
            Invoke("Grab", 0.1f);
        }

    }

    void Throw()
    {
        if (interactInput.grab && isGrabing && !playerCheck.isFacingWall && grabbedobject != null)
        {
            //grabbedobject.transform.position = grabPoint.position;
            grabbedobject.GetComponent<FixedJoint2D>().enabled = false;
            grabbedobject.GetComponent<FixedJoint2D>().connectedBody = null;
            grabbedobject.GetComponent<Rigidbody2D>().AddForce(new Vector2(playerCheck.horizontal, playerCheck.vertical) * throwForce, ForceMode2D.Impulse);
            grabbedobject.GetComponent<Collider2D>().isTrigger = false;
            grabbedobject.GetComponentInChildren<VFXIObjects>().isGrabbed = false;
            grabbedobject = null;
            Invoke("StopGrab", 0.1f);
        }
        else if (interactInput.drop && isGrabing && !playerCheck.isFacingWall && grabbedobject != null)
        {
            Vector2 leavePosition = new Vector2(gameObject.transform.position.x + ((playerSize.x + sizeObject.x / 4f) * playerCheck.direction), gameObject.transform.position.y + ((sizeObject.x - playerSize.x) / 2));
            grabbedobject.transform.position = leavePosition;
            grabbedobject.GetComponent<FixedJoint2D>().enabled = false;
            grabbedobject.GetComponent<FixedJoint2D>().connectedBody = null;
            grabbedobject.GetComponent<Collider2D>().isTrigger = false;
            grabbedobject.GetComponentInChildren<VFXIObjects>().isGrabbed = false;
            grabbedobject = null;
            Invoke("StopGrab", 0.1f);
        }
    }

    void Aim()
    {
        if (interactInput.aim && isGrabing && playerCheck.isOnGround)
        {
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
            isAiming = true;
        }
        else if(!interactInput.aim || !isGrabing && playerCheck.isOnGround)
        {
            isAiming = false;
        }
            
        
    }


    void Grab()
    {
        isGrabing = true;
    }
    void StopGrab()
    {
        isGrabing = false;
    }

    void PosibleGrab()
    {
        posibleGrab = false;
    }


    private void OnDrawGizmos()
    {
        playerCheck = GetComponent<PlayerMovement>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(grabPoint.position, radius);

        
      
    }
}
