using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleActivate : MonoBehaviour
{
    public Camera mainCam;
    public GameObject puzzles;
    public GameObject playerCam;
    public GameObject roomManager;
    public string puzzleSceneName;

    private static RoomData rd;

    private void Start()
    {
        rd = DataSaver.loadData<RoomData>("room_data");
    }
    public void BeginPuzzle()
    {
        mainCam.enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false; // делаем объект неинтерактивным
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(puzzleSceneName));
    }
    public void PausePuzzle()
    {
        EndPuzzle();
    }
    public void EndPuzzle()
    {
        puzzles.SetActive(false);
        playerCam.SetActive(false);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("room"));
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == puzzleSceneName)
        {
            puzzles.SetActive(true);
            playerCam.SetActive(true);
            if (puzzles.transform.childCount == 0)
            {
                if (puzzleSceneName == "Tube")
                    rd.is_tube = true;
                else if (puzzleSceneName == "SlidingPuzzle")
                    rd.is_sliding = true;
                DataSaver.saveData<RoomData>(rd, "room_data");
                EndPuzzle();
            }
        }
        else if (SceneManager.GetActiveScene().name == "room" && mainCam != null)
        {
            mainCam.enabled = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            roomManager.GetComponent<RoomManager>().data = DataSaver.loadData<RoomData>("room_data");
        }
    }
}
