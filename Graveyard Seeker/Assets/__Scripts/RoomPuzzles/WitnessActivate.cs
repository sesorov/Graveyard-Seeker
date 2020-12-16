using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WitnessActivate : MonoBehaviour
{
    public Camera mainCam;
    public GameObject puzzles;
    public GameObject playerCam;
    public GameObject roomManager;

    private static RoomData rd;

    private void Start()
    {
        rd = DataSaver.loadData<RoomData>("room_data");
    }
    public void BeginPuzzle()
    {
        mainCam.enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false; // делаем объект неинтерактивным
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Puzzles"));
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
        if (SceneManager.GetActiveScene().name == "Puzzles")
        {
            puzzles.SetActive(true);
            playerCam.SetActive(true);
            if (puzzles.transform.childCount == 0)
            {
                rd.is_witness = true;
                DataSaver.saveData<RoomData>(rd, "room_data");
                EndPuzzle();
            }
        }
        else if (SceneManager.GetActiveScene().name == "room" && mainCam != null)
        {
            mainCam.enabled = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
