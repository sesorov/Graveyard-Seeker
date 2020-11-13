using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleElement : MonoBehaviour
{

    public Vector2 Position;
    public Animator anim;
   public virtual bool IsValid(int[,] PuzzleArray)
    {
        return true;
    }
}
