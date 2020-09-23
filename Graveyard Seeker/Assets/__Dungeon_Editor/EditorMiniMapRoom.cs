using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditorMiniMapRoom : MonoBehaviour {
    const float     coroutineDelay = 0.25f;

    [Header("Set Dynamically")]
    public int x,y;

    private Image   img;

    void Awake() {
        img = GetComponent<Image>();
        StartCoroutine(SlowUpdate());
    }

	public void Click () {
        EditorMiniMap.SET_ROOM(x,y);
	}

    public Sprite sprite {
        get { return img.sprite; }
        set {
            if (img.sprite != value) {
             img.sprite = value;
            }
        }
    }

    IEnumerator SlowUpdate() {
        while (true) {
            yield return new WaitForSeconds(coroutineDelay);

            CheckRoomImage();
        }
    }

    public void CheckRoomImage() {
        if (EditorMap.MAP == null) return;
        // Check to see if this room has content.
        // It might be better if this chunk of code was elsewhere, but it will work here. - JB
        int x0, y0;
        Vector2 mLoc = EditorMap.RoomToMap(x,y);
        x0 = (int) mLoc.x;
        y0 = (int) mLoc.y;
        bool contentInRoom = false;
        for (int i=x0+1; i<x0+EditorMap.ROOM_W-1; i++) {
            for (int j=y0+1; j<y0+EditorMap.ROOM_H-1; j++) {
                if (EditorMap.MAP[i,j] != 0) {
                    contentInRoom = true;
                    break;
                }
            }
        }
        EditorMap.ROOM_HAS_CONTENT[x,y] = contentInRoom;


        // This chunk of code should definitely be here. - JB
        if (EditorMap.ROOM_X == x && EditorMap.ROOM_Y == y) {
            sprite = EditorMiniMap.SPRITE_ON;
        } else if (EditorMap.ROOM_HAS_CONTENT[x,y]) {
            sprite = EditorMiniMap.SPRITE_GRAY;
        } else {
            sprite = EditorMiniMap.SPRITE_OFF;
        }
    }
}
