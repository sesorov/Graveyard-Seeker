using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector3 change;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        this.transform.position = new Vector3(-4, -4, 0);
    }

    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxis("Horizontal");
        change.y = Input.GetAxis("Vertical");
        if(change != Vector3.zero)
        {
          MoveCharacter();
        }
    }

    void MoveCharacter()
    {
      rb.MovePosition(transform.position + change * speed * Time.deltaTime);
    }
}
