using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Set Dynamically")]
    public int x, y, tileNum;

    public void SetTile(int _x, int _y, int _tileNum = -1)
    {
        x = _x;
        y = _y;
        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D3") + "x" + y.ToString("D3"); //вернет число в десятичной форме (3+ символа) в виде строки (для x=23 и y=5 -> "023x005")

        if (_tileNum == -1) _tileNum = TileCamera.GET_MAP(x, y);
        tileNum = _tileNum;
        GetComponent<SpriteRenderer>().sprite = TileCamera.SPRITES[tileNum];
    }
}
