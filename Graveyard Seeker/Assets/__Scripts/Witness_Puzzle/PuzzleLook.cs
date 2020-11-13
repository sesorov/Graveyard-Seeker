using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
public class PuzzleLook : MonoBehaviour
{
    public Transform Eyes;
    //FirstPersonController MovementScript;
    LinePuzzle currentPuzzle;
    public Canvas EdgeScreen;
    // Start is called before the first frame update
    void Start()
    {
        //MovementScript = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Eyes.position, Eyes.forward, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag == "LinePuzzle")
                {
                    LinePuzzle PuzzleScript = hit.transform.GetComponent<LinePuzzle>();
                    if (PuzzleScript)
                    {
                        currentPuzzle = PuzzleScript;
                        PuzzleScript.FocusPuzzle();
                        //MovementScript.enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        EdgeScreen.enabled = true;
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            currentPuzzle.UnfocusPuzzle();
            //MovementScript.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EdgeScreen.enabled = false;
        }
    }
}
