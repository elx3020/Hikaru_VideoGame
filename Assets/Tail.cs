using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public GameObject tail;
    public Vector2 offset;
    [HideInInspector]public Vector2 targetPosition;
    public float speed;


    // Update is called once per frame
    void LateUpdate()
    {
        targetPosition = new Vector2(transform.position.x,transform.position.y) + offset;

        Move(targetPosition);
    }

    void Move(Vector2 direction)
    {
        tail.transform.position = Vector2.Lerp(tail.transform.position, direction, speed * Time.deltaTime);
    }

}
