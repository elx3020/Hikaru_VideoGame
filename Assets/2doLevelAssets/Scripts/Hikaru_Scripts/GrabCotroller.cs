
using UnityEngine;

public class GrabCotroller : MonoBehaviour
{
    Rigidbody2D rigidBody;
    PlayerMovement playerCheck;
    Vector2 playerSize;
    
    
    public SpriteRenderer playerSprite;
    public GameObject grabbedobject;
    public Transform grabPoint;
    public Transform objectHolder;
    public float dist = .5f;
    public bool isGrabing = false;
    public bool posibleGrab = false;
    public float throwForce;
    public float smallAmount;



    private Vector2 sizeObject;
    private float initialheadCleareance;
    private Vector2 possibleObjectSize;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerCheck = GetComponent<PlayerMovement>();
        initialheadCleareance = playerCheck.headClearance;
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D objectGrab = Physics2D.Raycast(grabPoint.position, Vector2.right * playerCheck.direction, dist);

        if(objectGrab.collider != null && objectGrab.collider.tag == "IObjects")
        {
            possibleObjectSize = objectGrab.collider.gameObject.GetComponent<SpriteRenderer>().bounds.size;
            if(initialheadCleareance == playerCheck.headClearance)
            {
                float headspace = playerCheck.headClearance + possibleObjectSize.y;
                playerCheck.headClearance = headspace;
            }
        }
        else
        {
            possibleObjectSize = Vector2.zero;
            playerCheck.headClearance = initialheadCleareance;
        }




        if (objectGrab.collider != null && objectGrab.collider.tag == "IObjects" && !playerCheck.isHeadBlocked && grabbedobject == null)
        {
            posibleGrab = true;
        }
        else
        {
            posibleGrab = false;
        }




        if (Input.GetButtonDown("Grab") && !isGrabing && posibleGrab)
        {
            grabbedobject = objectGrab.collider.gameObject;
            sizeObject = grabbedobject.GetComponent<SpriteRenderer>().bounds.size;
            playerSize = playerSprite.bounds.size;
            Vector2 targetPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (playerSize.y / 2) + (sizeObject.y / 2) - smallAmount);
            //grabbedobject.transform.position = objectHolder.position;
            grabbedobject.transform.position = targetPosition;
            grabbedobject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            grabbedobject.GetComponent<FixedJoint2D>().enabled = true;
            grabbedobject.GetComponent<FixedJoint2D>().connectedBody = rigidBody;
            Invoke("Grab", 0.1f);
        }

        if (Input.GetButtonDown("Grab") && isGrabing)
        {
            //grabbedobject.transform.position = grabPoint.position;
            grabbedobject.GetComponent<FixedJoint2D>().enabled = false;
            grabbedobject.GetComponent<FixedJoint2D>().connectedBody = null;
            grabbedobject.GetComponent<Rigidbody2D>().AddForce(new Vector2(playerCheck.horizontal, playerCheck.vertical) * throwForce, ForceMode2D.Impulse);
            grabbedobject = null;
            isGrabing = false;
        }else if(Input.GetButtonDown("Drop") && isGrabing && !playerCheck.isFacingWall)
        {
            grabbedobject.transform.position = grabPoint.position;
            grabbedobject.GetComponent<FixedJoint2D>().enabled = false;
            grabbedobject.GetComponent<FixedJoint2D>().connectedBody = null;
            grabbedobject = null;
            isGrabing = false;
        }


    }
    void Grab()
    {
        isGrabing = true;
    }

    void PosibleGrab()
    {
        posibleGrab = false;
    }



    private void OnDrawGizmos()
    {
        playerCheck = GetComponent<PlayerMovement>();
        Vector2 targetposition = Vector2.zero;
        targetposition += new Vector2(grabPoint.position.x + (dist * playerCheck.direction),grabPoint.position.y);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(grabPoint.position, targetposition);
    }
}
