using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnClick : MonoBehaviour
{

    public GameObject RespawnObject;
    public Camera mainCamera;

   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            Instantiate(RespawnObject, new Vector3(touchPosition.x,touchPosition.y,0f), Quaternion.identity);
        }
    }
}
