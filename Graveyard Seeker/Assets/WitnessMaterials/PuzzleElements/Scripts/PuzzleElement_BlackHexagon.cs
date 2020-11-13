using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleElement_BlackHexagon : PuzzleElement
{
    public override bool IsValid(int[,] PuzzleArray)
    {
        if (PuzzleArray[Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.y)]==1)
        {
            return true;
        }
        print(transform.name + " is Wrong");
        anim.Play("Wrong");
        return false;
    }
}
