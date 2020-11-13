using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LinePuzzle : MonoBehaviour
{

    public bool PuzzleFocused = false;
    public bool PuzzleActive = false;
    public bool PuzzleFinished = false;
    public bool PuzzleComplete = false;
    public Transform FocusArea;
    private GameObject Player;
    public GameObject MoveX;
    public GameObject MoveY;
    public GameObject MoveBackX;
    public GameObject MoveBackY;


    public Image Cursor;
    public RectTransform CursorChecker;
    public Image StartingCircle;

    public RectTransform LineParent;
    public RectTransform LinePrefab;

    public float CursorSpeed;

    public Image[] Lines;
    public RectTransform EndSlot;

    Vector2 LastCorner;
    public Corner[] CornerFinders;
   



    public List<RectTransform> Corners;
    public List<Corner> ArrayCorners;
    RectTransform CurrentLine;

    RectTransform LastLineRiding;
    RectTransform LineRiding;

    public PuzzleElement[] puzzleElements;

    public int ArraySize;

    public int CurrentCorner;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CurrentCorner = 99;
    }

    // Update is called once per frame
    void Update()
    {
        if (PuzzleFocused && PuzzleActive)
        {
            MoveCursor();
            UpdateCurrentLine();
            if (CheckPuzzleComplete())
            {
                CompletePuzzle();
            }
        }
    }
    public bool CheckPuzzleComplete()
    {
        if (Vector2.Distance(EndSlot.position,Cursor.rectTransform.position)<0.035f)
        {
            return true;
        }
        return false;
    }
    public void CompletePuzzle()
    {
        PuzzleActive = false;
        bool isvalid = true;

        //Make a 2d array to check against
        int[,] PuzzleArray;

        PuzzleArray = new int[ArraySize, ArraySize];

       // ArrayCorners.Insert(0, StartingCircle.GetComponent<Corner>());
        ArrayCorners.Add(EndSlot.GetComponent<Corner>());
        for (int i = 0; i < ArrayCorners.Count - 1; i++)
        {

            Vector2 Position = ArrayCorners[i].ArrayPosition;
            Vector2 EndPoint = ArrayCorners[i + 1].ArrayPosition;
            Vector2 CursorPosition = Position;
            do
            {
                //print(CursorPosition);
                PuzzleArray[Mathf.RoundToInt(CursorPosition.x),Mathf.RoundToInt(CursorPosition.y)] = 1;
                CursorPosition = Vector2.MoveTowards(CursorPosition, EndPoint, 1);

            } while (CursorPosition != EndPoint);
            PuzzleArray[Mathf.RoundToInt(CursorPosition.x), Mathf.RoundToInt(CursorPosition.y)] = 1; // add the last boy
        }

        if (puzzleElements.Length > 0)
        {
            foreach (PuzzleElement P in puzzleElements)
            {
                //Check the puzzle 
                if (P.IsValid(PuzzleArray) == false)
                {
                    isvalid = false;
                }

                //print(PuzzleArray[1, 2]);
            }
        }
        if (isvalid)
        {
            PuzzleComplete = true;
            PuzzleFocused = false;
        }
        else
        {
            ResetPuzzle();
        }


    }
    public void FocusPuzzle()
    {
        PuzzleFocused = true;
    }
    public void UnfocusPuzzle()
    {
        PuzzleFocused = true;
        PuzzleActive = false;
        if (!PuzzleComplete){
            ResetPuzzle();
        }
    }
    public void ActivatePuzzle()
    {
        ResetPuzzle();
        PuzzleActive = true;      
        CurrentLine = Instantiate(LinePrefab, new Vector3(LastCorner.x,LastCorner.y, StartingCircle.rectTransform.position.z), Quaternion.identity,LineParent);
    }
    public Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] WorldCorners = new Vector3[4];
        rt.GetWorldCorners(WorldCorners);
        Vector3 topLeft = WorldCorners[0];
        Vector2 size;
        Rect LineRect;
        if (rt.rect.size.x > rt.rect.size.y)
        {
            
            size = new Vector2(rt.rect.size.x-(0.035f/2), 0);
            LineRect = new Rect((topLeft + WorldCorners[1]) / 2, size);
            LineRect.xMin += 0.035f/2;

        }
        else
        {
            size = new Vector2(0, rt.rect.size.y - (0.035f / 2));
            LineRect = new Rect((topLeft + WorldCorners[3])/2, size);
            LineRect.yMin += 0.035f/2;
        }


        Vector3 finaltopLeft = new Vector3(LineRect.xMin, LineRect.yMin, rt.position.z);


        Vector3 finalBottomRight = new Vector3(LineRect.xMax, LineRect.yMax, rt.position.z);
        Debug.DrawLine(finaltopLeft, finalBottomRight);


        return LineRect;
    }
    public Rect GetCursorWorldRect(RectTransform rt)
    {
        Vector3[] WorldCorners = new Vector3[4];
        rt.GetWorldCorners(WorldCorners);
        Rect LineRect = new Rect(WorldCorners[0], new Vector2(rt.rect.size.x, rt.rect.size.y)); 
        return LineRect;
    }
    public void NewCorner()
    {
        if (Corners.Count > 1)
        {
            if (Vector3.Distance(Corners[Corners.Count-1].position, Cursor.rectTransform.position) < 0.03f)
            {
                Destroy(Corners[Corners.Count - 1].gameObject);
                Corners.RemoveAt(Corners.Count-1);
                ArrayCorners.RemoveAt(ArrayCorners.Count - 1);

                LastCorner = Corners[Corners.Count - 1].position;
                CurrentLine = Corners[Corners.Count - 1];
            }
            else
            {
                MakeCorner();
            }
        }
        else{
            MakeCorner();
        }

    }
    public void MakeCorner()
    {


        float distance = Mathf.Infinity;
        Corner ClosestCornerFinder = null;
        foreach (Corner c in CornerFinders)
        {
            float newdist = Vector3.Distance(c.GetComponent<RectTransform>().position, Cursor.rectTransform.position);
            if (newdist < distance)
            {
                distance = newdist;
                ClosestCornerFinder = c;
            }
        }

        ArrayCorners.Add(ClosestCornerFinder);

        CurrentLine.sizeDelta = new Vector2(Vector2.Distance(CurrentLine.position, ClosestCornerFinder.GetComponent<RectTransform>().position) / CurrentLine.localScale.x, 0.03f / CurrentLine.localScale.x);
        float targetRotation = Mathf.Atan2((ClosestCornerFinder.GetComponent<RectTransform>().position.y - CurrentLine.position.y), (ClosestCornerFinder.GetComponent<RectTransform>().position.x - CurrentLine.position.x)) * Mathf.Rad2Deg;
        CurrentLine.localRotation = Quaternion.Euler(0, 0, targetRotation);
        LastCorner = ClosestCornerFinder.GetComponent<RectTransform>().position;
        CurrentLine = Instantiate(LinePrefab, new Vector3(LastCorner.x, LastCorner.y, StartingCircle.rectTransform.position.z), Quaternion.identity, LineParent);
        Corners.Add(CurrentLine);

    }
    public void UpdateCurrentLine()
    {
        CurrentLine.sizeDelta =new Vector2(Vector2.Distance(CurrentLine.position,Cursor.rectTransform.position)/CurrentLine.localScale.x,0.03f / CurrentLine.localScale.x);
        float targetRotation = Mathf.Atan2((Cursor.rectTransform.position.y - CurrentLine.position.y) , (Cursor.rectTransform.position.x - CurrentLine.position.x))* Mathf.Rad2Deg;
        CurrentLine.localRotation = Quaternion.Euler(0, 0, targetRotation);
    }
    
    public void MoveCursor()
    {
        bool DoesOverlapx = false;
        CursorChecker.position = Cursor.rectTransform.position;


        float Dx = 0.5f * (MoveX.GetComponent<MoveLineMobile>().is_pressed ? 1 : MoveBackX.GetComponent<MoveLineMobile>().is_pressed ? -1 : 0) * CursorSpeed * Time.deltaTime;
        float Dy = 0.5f * (MoveY.GetComponent<MoveLineMobile>().is_pressed ? 1 : MoveBackY.GetComponent<MoveLineMobile>().is_pressed ? -1 : 0) * CursorSpeed * Time.deltaTime;

        //float Dy = Input.GetAxis("Mouse Y") * CursorSpeed * Time.deltaTime;

        CursorChecker.localPosition += new Vector3(Dx, 0, 0);
        foreach (Image i in Lines)
        {
            Rect CursorRect = GetCursorWorldRect(CursorChecker);
            Rect LineRect = GetWorldRect(i.rectTransform);
            if (CursorRect.Overlaps(LineRect))
            {
                DoesOverlapx = true;
                LineRiding = i.rectTransform;
            }
        }
        for(int i = 0; i < Corners.Count - 2; i++) 
        {
            Vector3 ClosestPoint = Math3d.ProjectPointOnLineSegment(Corners[i].position, Corners[i + 1].position, CursorChecker.position);
            if (Vector3.Distance(ClosestPoint, CursorChecker.position) < 0.035)  // 0.035 для пк
            {
                DoesOverlapx = false;
            }
        }
        if (DoesOverlapx)
        {
            Cursor.rectTransform.localPosition += new Vector3(Dx, 0, 0);
        }



        CursorChecker.position = Cursor.rectTransform.position;
        bool DoesOverlapy = false;
        CursorChecker.localPosition += new Vector3(0, Dy, 0);
        foreach (Image i in Lines)
        {
            Rect CursorRect = GetCursorWorldRect(CursorChecker);
            Rect LineRect = GetWorldRect(i.rectTransform);
            if (CursorRect.Overlaps(LineRect))
            {
                DoesOverlapy = true;
                LineRiding = i.rectTransform;
            }
        }
        for (int i = 0; i < Corners.Count - 2; i++)
        {

            Vector3 ClosestPoint = Math3d.ProjectPointOnLineSegment(Corners[i].position, Corners[i+1].position, CursorChecker.position);
            if (Vector3.Distance(ClosestPoint, CursorChecker.position) < 0.035)
            {
                DoesOverlapy = false;
            }
        }
        if (DoesOverlapy)
        {
            Cursor.rectTransform.localPosition += new Vector3(0, Dy, 0);
        }


        if (LastLineRiding != LineRiding)
        {
            NewCorner();
        }
        LastLineRiding = LineRiding;
    }

    public void ResetPuzzle()
    {
        PuzzleComplete = false;
        PuzzleFinished = false;
        LastLineRiding = null;
        LineRiding = null;
        foreach (Transform c in LineParent)
        {
            Destroy(c.gameObject);
        }
        Corners.Clear();
        ArrayCorners.Clear();
        Cursor.rectTransform.position = StartingCircle.rectTransform.position;
        CursorChecker.position = StartingCircle.rectTransform.position;
        LastCorner = StartingCircle.rectTransform.position;
        CurrentCorner = 99;
    }
}
