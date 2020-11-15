using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public GameObject tail;
    public Vector2 offset;
    [HideInInspector]public Vector2 targetPosition;
    public float speed;
    public float ymultiplayer;


    // Update is called once per frame
    void LateUpdate()
    {
        targetPosition = new Vector2(transform.position.x,transform.position.y) + offset;

        Move(targetPosition);
    }

    void Move(Vector2 direction)
    {
        Vector2 movePosition = tail.transform.position;
        movePosition.x = Mathf.Lerp(movePosition.x, direction.x, speed * Time.deltaTime);
        movePosition.y = Mathf.Lerp(movePosition.y, direction.y, speed * ymultiplayer * Time.deltaTime);
        tail.transform.position = movePosition;
    }

}
