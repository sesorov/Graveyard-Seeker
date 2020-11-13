using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLineMobile : MonoBehaviour
{
    public bool is_pressed;

    public int GetAxisX()
    {
        return is_pressed ? 1 : 0;
    }

    public void Pressed()
    {
        is_pressed = true;
    }

    public void NotPressed()
    {
        is_pressed = false;
    }
}
