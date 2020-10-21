using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeletos : Enemy, IFacingMover
{
    [Header("Set in Inspector: Skeletos")]
    public int speed = 2;
    public float timeThinkMin = 1f;
    public float timeThinkMax = 4f;
    public int skinType = 1;  // 1 - культист с маской чумного доктора, 2 - культист "безликий"
    public RuntimeAnimatorController cultist_1;
    public RuntimeAnimatorController cultist_2;

    [Header("Set Dynamically: Skeletos")]
    public int facing = 0;
    public float timeNextDecision = 0;
    public byte difficulty = 0;
    private Transform player;
    private Vector3 curPos, lastPos;

    private InRoom inRm;

    protected override void Awake()
    {
        skinType = Random.Range(1, 3);  // 1 или 2
        switch(skinType)
        {
            case 1:
                break;
            case 2:
                GetComponent<Animator>().runtimeAnimatorController = cultist_2;
                break;
            default:
                break;
        }
        lastPos = transform.position;
        base.Awake();
        inRm = GetComponent<InRoom>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // настроить difficulty по доанным с объекта с dontdestroyonload
    }

    protected override void Update()
    {
        base.Update();
        if (knockback) return;

        curPos = transform.position;  // если уперся в стену или просто стоит - не проигрываем анимацию
        if (rigid.velocity.magnitude < 0.2)
        {
            anim.CrossFade("Cultist_" + skinType.ToString() + "_idle", 0);
            anim.speed = 0;
        }
        lastPos = curPos;

        if (difficulty == 0)
        {

            if (Time.time >= timeNextDecision)
            {
                DecideDirection();
            }
            rigid.velocity = directions[facing] * speed; // унаследвано от Enemy, инициализируется в Enemy.Awake()
        }

        if (difficulty == 1)
        {
            transform.gameObject.GetComponent<GridMove>().enabled = false;
            if (Vector2.Distance(transform.position, player.position) < 3)
            {
                rigid.velocity = new Vector3(0, 0, 0);
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else
            {
                if (Time.time >= timeNextDecision)
                {
                    DecideDirection();
                }
                rigid.velocity = directions[facing] * speed;
            }
        }
    }

    private void DecideDirection() // выбор случайного направления + случайный интервал времени до следующей смены направления
    {
        facing = Random.Range(0, 4);
        timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);
        anim.CrossFade("Cultist_" + skinType.ToString() + "_walk_" + facing.ToString(), 0);
        anim.speed = 1;
    }
    
    public int GetFacing()
    {
        return facing;
    }
    public bool moving
    {
        get
        {
            return true; // скелет находится в постоянном движении
        }
    }
    public float GetSpeed()
    {
        return speed;
    }
    public float gridMult
    {
        get
        {
            return inRm.gridMult;
        }
    }
    public Vector2 roomPos
    {
        get { return inRm.roomPos; }
        set { inRm.roomPos = value; }
    }
    public Vector2 roomNum
    {
        get { return inRm.roomNum; }
        set { inRm.roomNum = value; }
    }
    public Vector2 GetRoomPosOnGrid(float mult = -1)
    {
        return inRm.GetRoomPosOnGrid(mult);
    }
}
