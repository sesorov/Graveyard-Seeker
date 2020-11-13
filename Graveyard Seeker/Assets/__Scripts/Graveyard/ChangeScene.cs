using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [Header("Name of the scene: ")]
    public string sceneName;
    public void load(string sceneName)
    {
       SceneManager.LoadScene(sceneName);
    }
    public void load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
