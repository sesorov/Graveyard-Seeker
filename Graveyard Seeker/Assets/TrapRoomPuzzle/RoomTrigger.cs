using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
      if(other.tag == "Player")
      {
        player.transform.position = new Vector3(-4, -4, 0);
      }
    }
}
