using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHelper : MonoBehaviour
{
    public GameObject puzzle;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered");
        puzzle.GetComponent<LinePuzzle>().CursorSpeed = 0.1f;
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("left");
        puzzle.GetComponent<LinePuzzle>().CursorSpeed = 1f;
    }
}
