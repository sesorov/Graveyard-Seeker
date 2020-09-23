using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditorMiniMap : MonoBehaviour {
    private static EditorMiniMap S;

    [Header("Set in Inspector")]
    public GameObject   miniMapRoomPrefab;
    public Sprite       spriteOff, spriteOn, spriteGray;

    private EditorMap   eMap;
    private int         roomX, roomY;
    private EditorMiniMapRoom       selectedRoom;
    private EditorMiniMapRoom[,]    miniMapRooms;

	// Use this for initialization
	void Start () {
        S = this;

        eMap = transform.parent.GetComponent<EditorMap>();
        RectTransform recT = GetComponent<RectTransform>();

        miniMapRooms = new EditorMiniMapRoom[EditorMap.mapSize,EditorMap.mapSize];
        for (int j=0; j<EditorMap.mapSize; j++) {
            for (int i=0; i<EditorMap.mapSize; i++) {
                GameObject go = Instantiate(miniMapRoomPrefab);
                RectTransform rt = go.GetComponent<RectTransform>();
                EditorMiniMapRoom emmr = go.GetComponent<EditorMiniMapRoom>();
                miniMapRooms[i,j] = emmr;
                emmr.sprite = spriteOff;
                emmr.x = i;
                emmr.y = j;
                rt.SetParent(recT);
                rt.anchoredPosition = new Vector2(i*32,j*22);
            }
        }

        HighlightRoom();
	}
	
	// Update is called once per frame
	void Update () {
        // Watch for changes in the room numbers from EditorMap
        if (roomX != eMap.roomX) {
            roomX = eMap.roomX;
            HighlightRoom();
        }
        if (roomY != eMap.roomY) {
            roomY = eMap.roomY;
            HighlightRoom();
        }
	}

    void HighlightRoom() {
        EditorMiniMapRoom oldRoom = selectedRoom;
        selectedRoom = miniMapRooms[roomX, roomY];
        selectedRoom.CheckRoomImage();
        if (oldRoom != null) {
            oldRoom.CheckRoomImage();
        }
//        if (selectedRoom != null) {
//            selectedRoom.sprite = spriteOff;
//        }
//        selectedRoom.sprite = spriteOn;
    }

    static public void SET_ROOM(int x, int y) {
        S.SetRoom(x,y);
    }

    public void SetRoom(int x, int y) {
        roomX = x;
        roomY = y;
        if (roomX != eMap.roomX) {
            eMap.roomX = roomX;
        }
        if (roomY != eMap.roomY) {
            eMap.roomY = roomY;
        }
        HighlightRoom();
    }

    static public Sprite SPRITE_ON {
        get { return S.spriteOn; }
    }
    static public Sprite SPRITE_OFF {
        get { return S.spriteOff; }
    }
    static public Sprite SPRITE_GRAY {
        get { return S.spriteGray; }
    }

}
