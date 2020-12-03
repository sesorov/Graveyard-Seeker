using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadScenes : MonoBehaviour
{
    [SerializeField] private string[] _sceneNames;
    public string[] _SceneNames => this._sceneNames;

    private AsyncOperation _asyncOperation;

    private IEnumerator LoadSceneAsyncProcess(string sceneName)
    {
        // предзагрузка сцены
        this._asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // не активируем сцену, пока не надо
        this._asyncOperation.allowSceneActivation = false;

        while (!this._asyncOperation.isDone)
        {
            Debug.Log($"[scene]:{sceneName} [load progress]: {this._asyncOperation.progress}");

            yield return null;
        }
    }

    private void Preload()
    {
        Debug.Log("Started Scene Preloading");

        // начинаем предзагрузку
        foreach (string _scene in _sceneNames)
        {
            this.StartCoroutine(this.LoadSceneAsyncProcess(sceneName: _scene));
            //this._asyncOperation.allowSceneActivation = false;
        }
        this._asyncOperation.allowSceneActivation = true;
    }
    private void Start()
    {
        foreach (string _scene in _sceneNames)
            SceneManager.LoadScene(sceneName: _scene, mode: LoadSceneMode.Additive);
    }
    /*private void Update()
    {
        if (this._asyncOperation == null)
        {
            foreach (string _scene in _sceneNames)
            {
                this.StartCoroutine(this.LoadSceneAsyncProcess(sceneName: _scene));
                this._asyncOperation.allowSceneActivation = false;
            }
        }
        if (this._asyncOperation != null)
            this._asyncOperation.allowSceneActivation = true;
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && this._asyncOperation == null)
        {
            Debug.Log("Started Scene Preloading");

            // начинаем предзагрузку
            this.StartCoroutine(this.LoadSceneAsyncProcess(sceneName: this._sceneName));
        }

        // активируем сцену
        if (Input.GetKeyDown(KeyCode.Space) && this._asyncOperation != null)
        {
            Debug.Log("Allowed Scene Activation");

            this._asyncOperation.allowSceneActivation = true;
        }
    }*/
}