using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleElement_AllFail : PuzzleElement
{
    public override bool IsValid(int[,] PuzzleArray)
    {
        return false;
    }
}
