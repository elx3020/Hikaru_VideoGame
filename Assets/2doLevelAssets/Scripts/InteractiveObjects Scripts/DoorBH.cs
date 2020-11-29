using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBH : MonoBehaviour
{


    public void DoorOpen()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //animation to open and sound
    }


    public void DoorClose()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        //animation to close and sound
    }

}
