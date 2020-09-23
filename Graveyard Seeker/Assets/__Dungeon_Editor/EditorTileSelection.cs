using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Uncomment the following line for verbose debugging
//#define VERBOSE_ETS_DEBUG

public class EditorTileSelection : MonoBehaviour {
    static EditorTileSelection S;
    static int              _SELECTED_TILE;
    static public Sprite    SELECTED_SPRITE;
    static public Sprite[]  S_spriteArray;

    [Header("Set in Inspector")]
    public int              selectionTileSize = 64;
    public Image            selectionPreview;
    public RectTransform    allTiles;
    public RectTransform    frame;
    public Texture2D        mapTexture;
    [SerializeField]
    private int             _selectedTile = 0;

    [Header("Set Dynamically")]
    public bool             selecting = false;

    int selectedTile {
        get { return SELECTED_TILE; }
        set {
            _SELECTED_TILE = value;
            _selectedTile = value;
            SELECTED_SPRITE = S_spriteArray[SELECTED_TILE];
            selectionPreview.sprite = SELECTED_SPRITE;
        }
    }

    void Awake() {
        S = this;
        S_spriteArray = Resources.LoadAll<Sprite>(mapTexture.name);

        StopSelecting();
        selectedTile = _selectedTile;

        allTiles.GetComponent<RawImage>().texture = mapTexture;
    }

    public void StartSelecting() {
        allTiles.gameObject.SetActive(true);
        frame.gameObject.SetActive(true);
        selecting = true;
    }

    public void StopSelecting() {
        allTiles.gameObject.SetActive(false);
        frame.gameObject.SetActive(false);
        selecting = false;
    }

    void Update() {
        if (!selecting) return;

        // Get mouse position in relative coordinates
        Vector2 mousePos = Input.mousePosition;
        mousePos.y = Screen.height - mousePos.y;
//        mousePos.x = mousePos.x/Screen.width;
//        mousePos.y = mousePos.y/Screen.height;
        Vector2 tileSelection;
        tileSelection.x = Mathf.Floor(mousePos.x/selectionTileSize);
        tileSelection.y = Mathf.Floor(mousePos.y/selectionTileSize);
        tileSelection.x = Mathf.Clamp(tileSelection.x, 0, 15);
        tileSelection.y = Mathf.Clamp(tileSelection.y, 0, 15);

        mousePos.x = tileSelection.x * selectionTileSize;
        mousePos.y = -tileSelection.y * selectionTileSize;
        frame.anchoredPosition = mousePos;

#if VERBOSE_ETS_DEBUG
        if (mousePos.x > 0) {
            print("In.mPos:"+Input.mousePosition+"\tmPos:"+mousePos+"\ttileSel:"+tileSelection);
        }
#endif

        if (Input.GetMouseButtonDown(0)) { // If they click the mouse button
            selectedTile = (int) (tileSelection.x + tileSelection.y*16);
            print("tileSelection:"+tileSelection+"\tsel:"+selectedTile);
            StopSelecting();
        }
    }

    static public int SELECTED_TILE {
        get { return _SELECTED_TILE; }
        set {
            S.selectedTile = value;
        }
    }
}
