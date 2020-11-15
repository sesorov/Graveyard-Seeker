using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : Enemy, IFacingMover
{
    [Header("Set in Inspector: Devil")]
    public int speed = 2;
    public float timeThinkMin = 1f;
    public float timeThinkMax = 2.5f;

    [Header("Set Dynamically: Devil")]
    public int facing = 0;
    public float timeNextDecision = 0;
    public byte difficulty = 0;
    private Transform player;
    private Vector3 curPos, lastPos;
    private SceneBrain gameController;

    private InRoom inRm;

    protected override void Awake()
    {
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
            anim.CrossFade("Devil_idle", 0);
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
    protected void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneBrain>();
    }

    private void DecideDirection() // выбор случайного направления + случайный интервал времени до следующей смены направления
    {
        facing = Random.Range(0, 4);
        timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);
        anim.CrossFade("Devil_walk_" + facing.ToString(), 0);
        anim.speed = 1;
    }
    protected override void Die()
    {
        // добавить анимацию
        gameController.isFirstAltered = true; // игрок вмешался и предотвратил первую смерть
        player.GetComponent<ChangeScene>().load(); // возвращаем игрока в лобби (кладбище)
        base.Die();
    }

    public int GetFacing()
    {
        return facing;
    }
    public bool moving // только для скелетов
    {
        get
        {
            return true; 
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