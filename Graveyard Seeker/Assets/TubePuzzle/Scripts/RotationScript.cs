using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RotationScript : MonoBehaviour
{
    float[] rotations = { 0f, 90f ,180f ,270f };

    public float[] correctRotation;
    [SerializeField]
    bool isPlaced = false;

    [SerializeField]
    int PossibleRots = 1;

    public GameManager gameManager;

    private void Start()
    {
        PossibleRots = correctRotation.Length;
        int rand = Random.Range(0, rotations.Length);
        transform.eulerAngles = new Vector3(0, 0, rotations[rand]);

        foreach (float rt in correctRotation)
            if (Mathf.Abs(rt - transform.eulerAngles.z) < 1)
            {
                isPlaced = true;
                gameManager.correctMove();
                return;
            }
    }

    private void OnMouseDown()
    {
      transform.Rotate(new Vector3(0, 0, 90));

        foreach(float rt in correctRotation)
            if (Mathf.Abs(rt - transform.eulerAngles.z) < 1)
            {
                isPlaced = true;
                gameManager.correctMove();
                return;
            }
        else if (isPlaced == true)
        {
            isPlaced = false;
            gameManager.wrongMove();
        }
    }
}
