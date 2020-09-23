using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRoom : MonoBehaviour
{
    public static int ROOM_W = 16; // ширина комнаты
    public static int ROOM_H = 11; // высота комнаты
    public static int WALL_T = 2;

    public static int MAX_RM_X = 9;
    public static int MAX_RM_Y = 9;
    public Vector2[] DOORS = new Vector2[]
    {
        new Vector2(14, 5),
        new Vector2(7.5f, 9),
        new Vector2(1, 5),
        new Vector2(7.5f, 1)
    };

    [Header("Set in Inspector")]
    public bool keepInRoom = true;
    public float gridMult = 1;

    private void LateUpdate() // вызывается после Update каждый кадр (выполняет заключительную операцию в кадре - возвращает сбежавших обратно в комнату)
    {
        if (keepInRoom)
        {
            Vector2 rPos = roomPos;
            rPos.x = Mathf.Clamp(rPos.x, WALL_T, ROOM_W - 1 - WALL_T); // Mathf.Clamp() гарантирует, что мин. значение WALL_T < координата rPos.x < макс. значения ROOM_W - 1 - WALL_T
            rPos.y = Mathf.Clamp(rPos.y, WALL_T, ROOM_H - 1 - WALL_T); // соответственно, он удерживает перса в комнате
            roomPos = rPos; // после наложения ограничений новые координаты rPos пристваиваются свойству roomPos, в результате чего выполнится метод записи set свойства roomPos и перс вернется обратно в комнату при попытке покинуть её
        }
    }

    public Vector2 roomPos // местоположение игрока в координатах комнаты относительно левого нижнего угла (X:0, Y:0)
    {
        get
        {
            Vector2 tPos = transform.position;
            tPos.x %= ROOM_W;
            tPos.y %= ROOM_H;
            return tPos;
        }
        set
        {
            Vector2 rm = roomNum;
            rm.x *= ROOM_W;
            rm.y *= ROOM_H;
            rm += value;
            transform.position = rm;
        }
    }

    public Vector2 roomNum // номер комнаты, в которой находится игрок (левая нижняя комната - X:0, Y:0)
    {
        get
        {
            Vector2 tPos = transform.position;
            tPos.x = Mathf.Floor(tPos.x / ROOM_W);
            tPos.y = Mathf.Floor(tPos.y / ROOM_H);
            return tPos;
        }
        set
        {
            Vector2 rPos = roomPos;
            Vector2 rm = value;
            rm.x *= ROOM_W;
            rm.y *= ROOM_H;
            transform.position = rm + rPos;
        }
    }

    public Vector2 GetRoomPosOnGrid(float mult = -1) // вычисляет координаты угла сетки в комнате, ближайшего к персонажу
    {
        if (mult == -1) mult = gridMult;
        Vector2 rPos = roomPos;
        rPos /= mult;
        rPos.x = Mathf.Round(rPos.x);
        rPos.y = Mathf.Round(rPos.y);
        rPos *= mult;
        return rPos;
    }
}
