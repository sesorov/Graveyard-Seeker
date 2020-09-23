using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditorTile : MonoBehaviour, IPointerClickHandler {
    [Header("Set Dynamically")]
    public int      tileNum;
    public int      x,y;

    private Image   img;

    void Awake() {
        img = GetComponent<Image>();
    }

    public void ShowTile(int tNum) {
        tileNum = tNum;
        if (tNum >= EditorTileSelection.S_spriteArray.Length) {
            print("Error: Trying to load tile "+tNum+" whe max is "+
                (EditorTileSelection.S_spriteArray.Length-1) );
        }
        img.sprite = EditorTileSelection.S_spriteArray[tNum];
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
            EditorMap.ChangeTile(this);
        } else if (eventData.button == PointerEventData.InputButton.Middle) {
            // Do nothing
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            EditorMap.CopyTile(this);
        }
    }
}


