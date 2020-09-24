using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    private IFacingMover mover;

    private void Awake()
    {
        mover = GetComponent<IFacingMover>();
    }

    private void FixedUpdate()
    {
        if (!mover.moving) return;
        int facing = mover.GetFacing();

        // если объект перемещается, применить выравнивание по сетке, получив координаты ближайшего узла
        Vector2 rPos = mover.roomPos;
        Vector2 rPosGrid = mover.GetRoomPosOnGrid();
        // этот код полагается на IFacingMover для определения шага сетки

        // двигаем объект в сторону линии сетки
        float delta = 0;
        if (facing == 0 || facing == 2) delta = rPosGrid.y - rPos.y; // горизонтальное движение, выравнивание по у
        else delta = rPosGrid.x - rPos.x; // вертикальное движение, выравнивание по х
        if (delta == 0) return; // объект уже выровнен

        float move = mover.GetSpeed() * Time.fixedDeltaTime;
        move = Mathf.Min(move, Mathf.Abs(delta));
        if (delta < 0) move = -move;
        if (facing == 0 || facing == 2) rPos.y += move; // горизонтальное движение, выравнивание по у
        else rPos.x += move; // вертикальное движение, выравнивание по х

        mover.roomPos = rPos;
    }
}
