using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pipesHolder;
    public GameObject[] pipes;
    public GameObject puzzle;

    [SerializeField]
    public int totalPipes = 0;
    [SerializeField]
    int correctedPipes;

    void Start()
    {
      totalPipes = pipesHolder.transform.childCount;

      pipes = new GameObject[totalPipes];

      for(int i = 0; i < pipes.Length; i++)
      {
        pipes[i] = pipesHolder.transform.GetChild(i).gameObject;
      }
    }

    public void correctMove()
    {
      correctedPipes += 1;

      if(correctedPipes == totalPipes)
      {
            foreach (Transform child in puzzle.transform)
                Destroy(child.gameObject);
      }
    }

    public void wrongMove()
    {
      correctedPipes -= 1;
    }
}
