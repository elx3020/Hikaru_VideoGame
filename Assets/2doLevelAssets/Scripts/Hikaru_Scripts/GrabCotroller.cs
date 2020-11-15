using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCotroller : MonoBehaviour
{
    Rigidbody2D rigidBody;
    PlayerMovement playerCheck;
    public GameObject grabbedobject;
    public Transform grabPoint;
    public Transform objectHolder;
    public float dist = .5f;
    public bool isGrabing = false;

    public float throwForce;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerCheck = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D objectGrab = Physics2D.Raycast(grabPoint.position, Vector2.right * playerCheck.direction, dist);

        if (objectGrab.collider != null && objectGrab.collider.tag == "IObjects" && !playerCheck.isHeadBlocked)
        {
            if (Input.GetButtonDown("Grab") && !isGrabing)
            {
                grabbedobject = objectGrab.collider.gameObject;
                grabbedobject.transform.position = objectHolder.position;
                grabbedobject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                grabbedobject.GetComponent<FixedJoint2D>().enabled = true;
                grabbedobject.GetComponent<FixedJoint2D>().connectedBody = rigidBody;
                Invoke("Grab", 0.1f);
            }


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





    private void OnDrawGizmos()
    {
        Vector2 targetposition = Vector2.zero;
        targetposition += new Vector2(grabPoint.position.x + (dist ),grabPoint.position.y);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(grabPoint.position, targetposition);
    }
}
