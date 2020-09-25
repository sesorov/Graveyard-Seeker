using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPanel : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Player player;
    public Sprite healthFull;
    public Sprite healthHalf;
    public Sprite healthEmpty;

    private Text keyCountText;
    private List<Image> healthImages;

    private void Start()
    {
        Transform trans = transform.Find("Key Count");
        keyCountText = trans.GetComponent<Text>();

        Transform healthPanel = transform.Find("Health Panel");
        healthImages = new List<Image>();
        if (healthPanel != null)
            for (int i = 0; i < 20; i++) // отыскиваем объекты от H_0 до H_19 (вложенные в Health Panel)
            {
                trans = healthPanel.Find("H_" + i);
                if (trans == null) break;
                healthImages.Add(trans.GetComponent<Image>());
            }
    }
    private void Update()
    {
        keyCountText.text = player.numKeys.ToString(); // показать кол-во ключей
        int health = player.health; // показать кол-во HP
        for (int i = 0; i < healthImages.Count; i++)
        {
            if (health > 1) healthImages[i].sprite = healthFull;
            else if (health == 1) healthImages[i].sprite = healthHalf;
            else healthImages[i].sprite = healthEmpty;
            health -= 2;
        }
    }
}
