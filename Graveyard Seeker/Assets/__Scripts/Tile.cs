using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Set Dynamically")]
    public int x, y, tileNum;

    private BoxCollider bColl; // ссылка на коллайдер плитки

    private void Awake()
    {
        bColl = GetComponent<BoxCollider>();
    }

    public void SetTile(int _x, int _y, int _tileNum = -1)
    {
        x = _x;
        y = _y;
        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D3") + "x" + y.ToString("D3"); //вернет число в десятичной форме (3+ символа) в виде строки (для x=23 и y=5 -> "023x005")

        if (_tileNum == -1) _tileNum = TileCamera.GET_MAP(x, y);
        tileNum = _tileNum;
        GetComponent<SpriteRenderer>().sprite = TileCamera.SPRITES[tileNum];
        SetCollider();
    }

    void SetCollider()
    {
        bColl.enabled = true;
        char c = TileCamera.COLLISIONS[tileNum]; // извлекаем символ плитки, определяющий вид столкновения
        switch(c)
        {
            case 'S': // вся плитка сплошная
                bColl.center = Vector3.zero;
                bColl.size = Vector3.one;
                break;
            case 'W': // верхняя половина плитки
                bColl.center = new Vector3(0, 0.25f, 0);
                bColl.size = new Vector3(1, 0.5f, 1);
                break;
            case 'A': // левая половина плитки
                bColl.center = new Vector3(-0.25f, 0, 0);
                bColl.size = new Vector3(0.5f, 1, 1);
                break;
            case 'D': // правая половина плитки
                bColl.center = new Vector3(0.25f, 0, 0);
                bColl.size = new Vector3(0.5f, 1, 1);
                break;
            default: // все остальное: _, |, и т.д.
                bColl.enabled = false;
                break;
        }
    }
}
