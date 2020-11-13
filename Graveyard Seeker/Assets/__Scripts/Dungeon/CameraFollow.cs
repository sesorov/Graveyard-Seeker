using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static bool TRANSITIONING = false;

    [Header("Set in Inspector")]
    public InRoom playerInRm;
    public float transTime = 0.5f;

    private Vector3 p0, p1; // p0 - исходная комната, p1 - новая комната
    private InRoom inRm;
    private float transStart;

    private void Awake()
    {
        inRm = GetComponent<InRoom>();
    }
    private void Update()
    {
        if (TRANSITIONING) // если CameraFollow в режиме перехода, то в течение transTime сек камера перемещается из исходной комнаты в новую
        {
            float u = (Time.time - transStart) / transTime;
            if (u >= 1)
            {
                u = 1;
                TRANSITIONING = false;
            }
            transform.position = (1 - u) * p0 + u * p1;
        }
        else
            if (playerInRm.roomNum != inRm.roomNum) TransitionTo(playerInRm.roomNum);
    }

    private void TransitionTo(Vector2 room)
    {
        p0 = transform.position; // сохраняем позицию камеры в исходной комнате
        inRm.roomNum = room;
        p1 = transform.position + (Vector3.back * 10); // сохраняем позицию в новой комнате ( +(Vector3.back * 10) нужно, потому что просто присваивание roomNum в InRoom запишет в Z число 0
        transform.position = p0; // возвращаем камеру в исходную позицию и выполняем линейную интерполяцию из p0 в p1
        transStart = Time.time;
        TRANSITIONING = true;
    }
}
