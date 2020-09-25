using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCamera : MonoBehaviour
{
    private static int W, H;
    private static int[,] MAP; // приватный с методами GET_MAP() и SET_MAP() во избежание IndexOutOfRangeException
    public static Sprite[] SPRITES;
    public static Transform TILE_ANCHOR;
    public static Tile[,] TILES;
    public static string COLLISIONS;

    [Header("Set in Inspector")]
    public TextAsset mapData;
    public Texture2D mapTiles;
    public TextAsset mapCollisions;
    public Tile tilePrefab;
    [Tooltip("mapCollisions.txt")]
    public TextAsset collisions;

    private void Awake()
    {
        COLLISIONS = collisions.text.Replace("\n", "").Replace("\r", "");
        LoadMap();
    }

    public void LoadMap()
    {
        //родительский gameobject всех плиток Tile
        GameObject go = new GameObject("TILE_ANCHOR");
        TILE_ANCHOR = go.transform;

        //загрузка спрайтов из mapTiles
        SPRITES = Resources.LoadAll<Sprite>(mapTiles.name); //изображение тайлов находится в папке Resourses, выгружаем спрайты из него

        //парсинг информации для карты
        string[] lines = mapData.text.Split('\n'); // разбиваем содержимое файла на строки
        H = lines.Length; // число строк - H
        string[] tileNums = lines[0].Split(' '); // первая строчка разбивается на подстроки по пробелу, каждый 2-значн. 16-ичн. код сохраняется в отдельный элемент
        W = tileNums.Length; // количество элементов

        System.Globalization.NumberStyles hexNum = System.Globalization.NumberStyles.HexNumber; // Для преобразования строк с 2-зн. 16-ичн. кодами в целые числа нужно сделать такой финт ушами
        //сохраняем инфу для карты в двумерный массив для ускорения доступа
        MAP = new int[W, H];
        for (int j = 0; j < H; j++)
        {
            tileNums = lines[j].Split(' ');
            for (int i = 0; i < W; i++)
            {
                if (tileNums[i] == "..") MAP[i, j] = 0; // ".." == пустота
                else MAP[i, j] = int.Parse(tileNums[i], hexNum);
            }
        }
        print("Parsed " + SPRITES.Length + " sprites."); // !!! ТОЧКА ОСТАНОВА для проверки значений в полях MAP и SPRITES
        print("Map size: " + W + " wide by " + H + " high");

        ShowMap();
    }

    public static int GET_MAP(int x, int y)
    {
        if (x < 0 || x >= W || y < 0 || y >= H)
        {
            Debug.LogError("IndexOutOfRangeException");
            return -1;
        }
        return MAP[x, y];
    }

    public static int GET_MAP(float x, float y)
    {
        return GET_MAP(Mathf.RoundToInt(x), Mathf.RoundToInt(y - 0.25f)); // -0.25f потому, что перс может находиться за пределами Tile, но юнити считает, что он на ней
    }

    public static void SET_MAP(int x, int y, int tNum) // точка останова, если карта генерируется неверно
    {
        if (x < 0 || x >= W || y < 0 || y >= H)
        {
            Debug.LogError("IndexOutOfRangeException");
            return;
        }
        MAP[x, y] = tNum;
    }

    private void ShowMap()
    {
        TILES = new Tile[W, H];
        for (int j = 0; j < H; j++)
            for (int i = 0; i < W; i++)
                if (MAP[i, j] != 0)
                {
                    Tile tempTile = Instantiate<Tile>(tilePrefab);
                    tempTile.transform.SetParent(TILE_ANCHOR); // усыновляем плиточку
                    tempTile.SetTile(i, j);
                    TILES[i, j] = tempTile;
                }
    }
}
