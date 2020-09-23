using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;


// Uncomment the following line to use PlayerPrefs to store the most recently used fileName
//#define UsePlayerPrefsForFileName

public class EditorMap : MonoBehaviour {
    const int               roomW = 16;
    const int               roomH = 11;
    const int               size = 96;
    public const int        mapSize = 10; // # of rooms wide and high

    static public int[,]    MAP;
    static public bool[,]   ROOM_HAS_CONTENT;
    static private EditorMap S;
    static private bool     mapLoaded = false;

    [Header("Set in Inspector")]
    public GameObject       uiTilePrefab;
    public RectTransform    mapAnchor;
    public InputField       mapNameInput;

    [Header("Set Dynamically")]
    [HideInInspector]
    public string           mapString;
    [SerializeField]
    private int             _roomX, _roomY;

    private int[,]          roomClipboard;

    private EditorTile[,]   roomTiles = new EditorTile[roomW,roomH];

	// Use this for initialization
	void Awake() {
        S = this; // Initialize the private Singleton
#if (UsePlayerPrefsForFileName)
        // Read the most recent fileName from PlayerPrefs
        if (PlayerPrefs.HasKey("fileName")) {
            mapNameInput.text = PlayerPrefs.GetString("fileName");
        }
#endif

        ROOM_HAS_CONTENT = new bool[mapSize,mapSize];

	    // Set up EditorTiles for each position on the map
        float x0 = size/8f;
        float y0 = size/8f;
        for (int j=0; j<roomH; j++) {
            for (int i=0; i<roomW; i++) {
                GameObject go = Instantiate( uiTilePrefab );
                go.name = i.ToString("D3")+"x"+j.ToString("D3");
                EditorTile et = go.GetComponent<EditorTile>();
                et.x = i;
                et.y = j;
                RectTransform rt = go.GetComponent<RectTransform>();
                rt.SetParent(mapAnchor);
                rt.anchoredPosition = new Vector2(x0+(i*size), y0+(j*size));
                roomTiles[i,j] = et;
            }
        }

//        LoadMap();
	}

    public void SaveMap() {
        if (mapNameInput.text == "" || mapNameInput.text.IndexOf(".txt") != -1) {
            print("You must enter a filename to save. Do not include \".txt\" in your filename.");
            return;   
        }

        // Prepare the map string
        int mapW = roomW * mapSize;
        int mapH = roomH * mapSize;
        string[] indexArray = new string[mapW];
        string[] lineArray = new string[mapH];
        for (int j=0; j<mapH; j++) {
            for (int i=0; i<mapW; i++) {
                if (MAP[i,j] == 0) {
                    indexArray[i] = "..";
                } else {
                    indexArray[i] = MAP[i,j].ToString("x2");//("D3");
                }
            }
            lineArray[j] = string.Join(" ",indexArray);
        }
        mapString = string.Join("\n",lineArray);

        // Output the text file 
        string path = Application.dataPath+"/Resources/"+mapNameInput.text+".txt";
        try {
            File.WriteAllText(path, mapString);
            print("Wrote file: "+path);
#if (UsePlayerPrefsForFileName)
            // If the save worked, save this filename to PlayerPrefs
            PlayerPrefs.SetString("fileName",mapNameInput.text);
#endif
        } catch (System.Exception e) {
            print("Error writing "+path+".\n"+e.Message);
        }

    }

    public void LoadMap() {
//        File.WriteAllText(Application.dataPath+"/Resources/"+outputMapDataTXT+".txt", str);
        if (mapNameInput.text == "" || mapNameInput.text.IndexOf(".txt") != -1) {
            print("You must enter a filename to load. Do not include \".txt\" in your filename.");
            return;   
        }

        try {   // Open the text file using a stream reader.
            string path = Application.dataPath+"/Resources/"+mapNameInput.text+".txt";
            using ( StreamReader sr = new StreamReader(path) ) { //File.OpenRead(path)) {
                // Read the stream to a string, and write the string to the console.
                mapString = sr.ReadToEnd();
                sr.Close();
            }
        } catch (System.Exception e) {
            print("The file "+mapNameInput.text+".txt could not be read.\n"+e.Message);
            return;
        }

        // Read in the map data
        string[] lines = mapString.Split('\n');
        int mapH = lines.Length;
        string[] tileNums = lines[0].Split(' ');
        int mapW = tileNums.Length;


        System.Globalization.NumberStyles hexNum;                            // c
        hexNum = System.Globalization.NumberStyles.HexNumber;

        // Place the map data into a 2D Array to make it faster to access
        MAP = new int[mapSize*roomW, mapSize*roomH];
        Vector2 room, inRm;
        for (int j=0; j<mapH; j++) {
            tileNums = lines[j].Split(' ');
            for (int i=0; i<mapW; i++) {
//                MAP[i,j] = int.Parse( tileNums[i] );

                if (tileNums[i] == "..") {
                    MAP[i,j] = 0;
                } else {
                    MAP[i,j] = int.Parse( tileNums[i], hexNum );             // d
                }

                if (MAP[i,j] != 0) {
                    room = MapToRoom(i,j);
                    inRm = MapToInRoom(i,j);
                    if (!GetRoomHasContent(room)) {
                        if (inRm.x>0 && inRm.x<roomW-1 && inRm.y>0 && inRm.y<roomH-1) {
                            SetRoomHasContent(room, true);
                        }
                    }
                }
            }
        }

        mapLoaded = true;
        ShowRoom();
    }
	
    public void ShowRoom() {
        ShowRoom(_roomX, _roomY);
    }
    public void ShowRoom(int rX, int rY) {
        if (!mapLoaded) return;
        for (int i=0; i<roomW; i++) {
            for (int j=0; j<roomH; j++) {
                int tNum = MAP[rX*roomW+i, rY*roomH+j];
                roomTiles[i,j].ShowTile(tNum);
            }
        }
    }

    public int roomX {
        get { return _roomX; }
        set {
            _roomX = Mathf.Clamp(value, 0, mapSize-1);
            ShowRoom();
        }
    }
    public int roomY {
        get { return _roomY; }
        set {
            _roomY = Mathf.Clamp(value, 0, mapSize-1);
            ShowRoom();
        }
    }

	void Update () {
        if (!mapLoaded) return;
        // Use Arrow keys to switch rooms
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            roomX++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            roomX--;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            roomY++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            roomY--;
        }
	}

    public void CopyRoom() {
        if (!mapLoaded) return;
        roomClipboard = new int[roomW,roomH];
        for (int i=0; i<roomW; i++) {
            for (int j=0; j<roomH; j++) {
                roomClipboard[i,j] = roomTiles[i,j].tileNum;
            }
        }
    }
    public void PasteRoom() {
        if (!mapLoaded) return;
        Vector2 roomLoc = RoomToMap(roomX, roomY);
        int x0 = (int) roomLoc.x;
        int y0 = (int) roomLoc.y;
        int ndx;
        for (int i=0; i<roomW; i++) {
            for (int j=0; j<roomH; j++) {
                ndx = roomClipboard[i,j];
                MAP[x0+i,y0+j] = ndx;
            }
        }
        ShowRoom();
    }





    static public Vector2 MapToRoom(int x, int y) {
        return MapToRoom(new Vector2(x,y));
    }
    static public Vector2 MapToRoom(Vector2 mLoc) {
        Vector2 rLoc = mLoc;
        rLoc.x = Mathf.Floor(mLoc.x/roomW);
        rLoc.y = Mathf.Floor(mLoc.y/roomH);
        return rLoc;
    }

    static public Vector2 MapToInRoom(int x, int y) {
        return MapToInRoom(new Vector2(x,y));
    }
    static public Vector2 MapToInRoom(Vector2 mLoc) {
        Vector2 rLoc = MapToRoom(mLoc);
        Vector2 inRmLoc = mLoc;
        inRmLoc.x -= rLoc.x*roomW;
        inRmLoc.y -= rLoc.y*roomH;
        return inRmLoc;
    }

    static public Vector2 RoomToMap(int x, int y) {
        return RoomToMap(new Vector2(x,y));
    }
    static public Vector2 RoomToMap(Vector2 rLoc) {
        return InRoomToMap(rLoc, Vector2.zero);
    }
    static public Vector2 InRoomToMap(Vector2 rLoc, Vector2 inRmLoc) {
        Vector2 mLoc = inRmLoc;
        mLoc.x += rLoc.x*roomW;
        mLoc.y += rLoc.y*roomH;
        return mLoc;
    }

    static public bool GetRoomHasContent(Vector2 loc) {
        int x = Mathf.FloorToInt(loc.x);
        int y = Mathf.FloorToInt(loc.y);
        return ROOM_HAS_CONTENT[x,y];
    }
    static public void SetRoomHasContent(Vector2 loc, bool b) {
        int x = Mathf.FloorToInt(loc.x);
        int y = Mathf.FloorToInt(loc.y);
        ROOM_HAS_CONTENT[x,y] = b;
    }

    static public int ROOM_X {
        get { return S.roomX; }
    }
    static public int ROOM_Y {
        get { return S.roomY; }
    }
    static public Vector2 ROOM {
        get { return new Vector2( S.roomX, S.roomY ); }
    }

    static public void ChangeTile(EditorTile et) {
        if (!mapLoaded) return;
        Vector2 mLoc = InRoomToMap( ROOM, new Vector2(et.x, et.y) );
        int i = (int) mLoc.x;
        int j = (int) mLoc.y;
        MAP[i,j] = EditorTileSelection.SELECTED_TILE;
        S.ShowRoom();
    }

    static public void CopyTile(EditorTile et) {
        if (!mapLoaded) return;
        int ndx = et.tileNum;
        EditorTileSelection.SELECTED_TILE = ndx;
    }

    static public int ROOM_H {
        get { return roomH; }
    }
    static public int ROOM_W {
        get { return roomW; }
    }

//    static public void COPY_ROOM() {
//        S.CopyRoom();
//    }
//    static public void PASTE_ROOM() {
//        S.PasteRoom();
//    }
}
