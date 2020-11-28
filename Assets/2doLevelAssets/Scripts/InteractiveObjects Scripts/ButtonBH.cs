using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBH : MonoBehaviour
{
    public DoorBH door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "IObjects")
        {
            door.DoorOpen();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "IObjects")
        {
            door.DoorClose();
        }
    }

}
