using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger1 : MonoBehaviour
{
    public GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
      if(other.tag == "Player")
      {
        player.transform.localScale = new Vector3(20, 20, 1);
        Debug.Log("KRASAVA");
      }
    }
}
