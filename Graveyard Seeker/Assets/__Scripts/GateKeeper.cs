using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKeeper : MonoBehaviour
{
    // !!! КОНСТАНТЫ ЗАВИСЯТ ОТ ФАЙЛА С ТАЙЛАМИ !!!
    // ТАЙЛЫ ЗАКРЫТЫХ ДВЕРЕЙ:
    const int lockedR = 95;
    const int lockedUR = 81;
    const int lockedDR = 102;
    const int lockedL = 100;
    const int lockedUL = 80;
    const int lockedDL = 101;

    // ТАЙЛЫ ОТКРЫТЫХ ДВЕРЕЙ:
    const int openR = 48;
    const int openUR = 93;
    const int openDR = 27;
    const int openL = 51;
    const int openUL = 92;
    const int openDL = 26;

    private IKeyMaster keys;

    private void Awake()
    {
        keys = GetComponent<IKeyMaster>();
    }
    private void OnCollisionStay(Collision coll)
    {
        if (keys.keyCount < 1) return; // нет ключей - гуляй

        Tile tile = coll.gameObject.GetComponent<Tile>();
        if (tile == null) return;

        int facing = keys.GetFacing(); // открытие двери будет производиться, только если игрок смотрит лицом в дверь
        // проверим, является ли тайл закрытой дверью
        Tile tile2;
        switch (tile.tileNum)
        {
            case lockedR:
                if (facing != 0) return; // если игрок не смотрит на дверь - ничего не делаем
                tile.SetTile(tile.x, tile.y, openR);
                break;
            case lockedUR:
                if (facing != 1) return;
                tile.SetTile(tile.x, tile.y, openUR);
                tile2 = TileCamera.TILES[tile.x - 1, tile.y];
                tile2.SetTile(tile2.x, tile2.y, openUL);
                break;
            case lockedDR:
                if (facing != 3) return;
                tile.SetTile(tile.x, tile.y, openDR);
                tile2 = TileCamera.TILES[tile.x - 1, tile.y];
                tile2.SetTile(tile2.x, tile2.y, openDL);
                break;
            case lockedL:
                if (facing != 2) return;
                tile.SetTile(tile.x, tile.y, openL);
                break;
            case lockedUL:
                if (facing != 1) return;
                tile.SetTile(tile.x, tile.y, openUL);
                tile2 = TileCamera.TILES[tile.x + 1, tile.y];
                tile2.SetTile(tile2.x, tile2.y, openUR);
                break;
            case lockedDL:
                if (facing != 3) return;
                tile.SetTile(tile.x, tile.y, openDL);
                tile2 = TileCamera.TILES[tile.x + 1, tile.y];
                tile2.SetTile(tile2.x, tile2.y, openDR);
                break;
            default:
                return;
        }
        keys.keyCount--;
    }
}
