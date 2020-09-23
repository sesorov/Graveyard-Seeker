using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private GameObject sword;
    private Player player;

    private void Start()
    {
        sword = transform.Find("Sword").gameObject;
        player = transform.parent.GetComponent<Player>();
        sword.SetActive(false); // не отображаем меч
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90 * player.facing); // поворачиваем меч в сторону, куда смотрит игрок
        sword.SetActive(player.mode == Player.eMode.attack); // отображаем, если игрок атакует
    }
}