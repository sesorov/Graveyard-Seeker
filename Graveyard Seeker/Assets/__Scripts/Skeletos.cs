using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeletos : Enemy
{
    [Header("Set in Inspector: Skeletos")]
    public int speed = 2;
    public float timeThinkMin = 1f;
    public float timeThinkMax = 4f;

    [Header("Set Dynamically: Skeletos")]
    public int facing = 0;
    public float timeNextDecision = 0;

    private void Update()
    {
        if (Time.time >= timeNextDecision)
        {
            DecideDirection();
        }
        rigid.velocity = directions[facing] * speed; // унаследвано от Enemy, инициализируется в Enemy.Awake()
    }

    void DecideDirection() // выбор случайного направления + случайный интервал времени до следующей смены направления
    {
        facing = Random.Range(0, 4);
        timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);
    }
}
