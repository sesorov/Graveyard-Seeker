using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            GameObject.FindGameObjectWithTag("Finish").GetComponent<Devil>().LeaveDungeon();
    }
}
