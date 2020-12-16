using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Name of the scene: ")]
    public string sceneName;
    public void PlayGame (string sceneName)
    {
       SceneManager.LoadScene(sceneName);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
}
