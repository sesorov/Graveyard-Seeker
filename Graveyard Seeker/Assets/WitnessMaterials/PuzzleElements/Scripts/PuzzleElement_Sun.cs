using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleElement_Sun : PuzzleElement
{
    int[,] PathFindingArray;
    public bool DebugEnabled = false;
    public LinePuzzle ParentPuzzle;
    public int colorId;
    public PuzzleElement_Sun[] OtherSameSuns;
    int ConnectedSuns=0;
    public override bool IsValid(int[,] PuzzleArray)
    {
        PathFindingArray = new int[PuzzleArray.GetLength(0), PuzzleArray.GetLength(1)];
        for (int x = 0; x < PuzzleArray.GetLength(0); x++)
        {
            for (int y = 0; y < PuzzleArray.GetLength(1); y++)
            {
                PathFindingArray[x, y] = PuzzleArray[x, y];
            }

        }



        ConnectedSuns = 0;
        FloodFill((int)Position.x, (int)Position.y);

        if(DebugEnabled)print("done");
        if (ConnectedSuns == 1)
        {
            return true;
        }
        anim.Play("Wrong");
        return false;

    }




    void FloodFill(int nodex, int nodey)
    {

        if (DebugEnabled) print(nodex + "," + nodey);
        if (PathFindingArray[nodex, nodey] == 1) return;
        if (DebugEnabled) print(nodex+","+nodey);
        PathFindingArray[nodex, nodey] = 1;
        foreach(PuzzleElement_Sun p in OtherSameSuns)
        {
        
            if(p.Position == new Vector2(nodex, nodey)) //We have made it to the other sun
            {
                ConnectedSuns++;
            }
        }

        if (CanMoveSouth(nodex,nodey))FloodFill(nodex, nodey + 1);
        if (CanMoveNorth(nodex, nodey)) FloodFill(nodex, nodey - 1);
        if (CanMoveEast(nodex, nodey)) FloodFill(nodex + 1, nodey);
        if (CanMoveWest(nodex, nodey)) FloodFill(nodex - 1, nodey);
        return;
    }





    bool CanMoveNorth(int x, int y)
    {
        if (y == 0) return false; // already on top edge
        return true;
    }
    bool CanMoveSouth(int x, int y)
    {
        if (y == PathFindingArray.GetLength(1) - 2) return false; // already on bottom edge
        return true;
    }
    bool CanMoveEast(int x, int y)
    {
        if (x == PathFindingArray.GetLength(0) - 2) return false; // on right edge
        return true;
    }
    bool CanMoveWest(int x, int y)
    {
        if (x == 0) return false; // on left edge
        return true;
    }
}
