using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
    [Header("Name of the scene: ")]
    public String sceneName;
    public void ChangeScene(string sceneName)
    {
       SceneManager.LoadScene(sceneName);
    }
}
