using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected static Vector3[] directions = new Vector3[] // направления (спасибо, кэп)
    {
        Vector3.right, Vector3.up, Vector3.left, Vector3.down
    };

    [Header("Set in Inspector: Enemy")]
    public float maxHealth = 1;

    [Header("Set Dynamically: Enemy")]
    public float health;

    protected Animator anim;
    protected Rigidbody rigid;
    protected SpriteRenderer sRend;

    protected virtual void Awake() // объявление Awake виртуальным и защищённым позволит переопределить его в подклассах
    {
        health = maxHealth; // устанавливаем уровень здоровья по умолчанию
        anim = GetComponent<Animator>(); // кэшируем ссылки на компоненты
        rigid = GetComponent<Rigidbody>();
        sRend = GetComponent<SpriteRenderer>();
    }
}
