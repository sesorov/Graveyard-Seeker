using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBrain : MonoBehaviour
{
    public GameObject player;
    public GameObject instance;

    public bool isFirstAltered = false;
    public bool isSecondAltered = false;
    public bool isThirdAltered = false;

    [SerializeField]
    private Vector3 _playerPos;

    public Vector3 Pos
    {
        get { return _playerPos; }
        set { _playerPos = value; }
    }

    private void Awake()  // singleton implementation
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length > 1)
            Destroy(gameObject);
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void LateUpdate()
    {
        if (player == null && SceneManager.GetActiveScene().name == "graveyard")
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (_playerPos == Vector3.zero)
                _playerPos = player.transform.position;
            player.transform.position = _playerPos;
        }
    }
    void Start()
    {
        
    }
}
