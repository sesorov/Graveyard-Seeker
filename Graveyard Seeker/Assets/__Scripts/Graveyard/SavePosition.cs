using UnityEngine;

public class SavePosition: MonoBehaviour
{
    public Transform Player;

    public void SavePos()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneBrain>().Pos = Player.position;
    }
}
