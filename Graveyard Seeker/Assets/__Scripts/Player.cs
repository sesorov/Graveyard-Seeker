﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IFacingMover
{
	public enum eMode { idle, moving, attack, transition }

	[Header("Set in Inspector")]
	[Range(0, 5)]
	public float speed = 2f;
	public float attackDuration = 0.25f; // в секундах
	public float attackDelay = 0.5f; // задержка между атаками
	public float transitionDelay = 0.5f; // задержка перехода между комнатами

	[Header("Set Dynamically")]
	public eMode mode = eMode.idle;

	public int facing = 0;
	private float timeAttackDone = 0; // время, когда завершается анимация атаки
	private float timeAttackNext = 0; // время, когда игрок снова может атаковать

	private float transitionDone = 0;
	private Vector2 transitionPos;

    private Rigidbody rb;

	private bool isMovingX = false;
	private bool isMovingY = false;
	private bool isRight = false;
	private bool isUp = false;
	private Animator anim;
	private InRoom inRm; // открывает доступ к подключенному классу InRoom
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		inRm = GetComponent<InRoom>();
	}
    private void Update()
    {
		Movement();
	}
    private void LateUpdate()
    {
		// получаем координаты узла сетки с размером ячейки в 0.5, ближайшего к данному персонажу
		Vector2 rPos = GetRoomPosOnGrid(0.5f);

		int doorNum;
		for (doorNum = 0; doorNum < 4; doorNum++) // проверка, находится ли персонаж на плитке с дверью
			if (rPos == InRoom.DOORS[doorNum]) break; // обнаружили дверь

		if (doorNum > 3 || doorNum != facing) return; // если игрок не находится ни в одной из дверей или не повернут в сторону выхода, то пусть стоит там и дальше

		Vector2 rm = roomNum; // переходим в следующую комнату
		switch (doorNum)
        {
			case 0:
				rm.x += 1;
				break;
			case 1:
				rm.y += 1;
				break;
			case 2:
				rm.x -= 1;
				break;
			case 3:
				rm.y -= 1;
				break;
		}

		// проверить возможность перехода в комнату rm
		if (rm.x >= 0 && rm.x <= InRoom.MAX_RM_X) // проверка наличия комнаты за дверью
			if (rm.y >= 0 && rm.y <= InRoom.MAX_RM_Y)
            {
				roomNum = rm;
				transitionPos = InRoom.DOORS[(doorNum + 2) % 4]; // выбирает противоположную дверь в комнате (вошел в DOORS[3], вышел в DOORS[1])
				roomPos = transitionPos;
				mode = eMode.transition; // переводим игрока в режим transition, что вызывает короткую задержку перехода, чтоб игрок увидел комнату
				transitionDone = Time.time + transitionDelay;
            }
    }
    private void Movement()
    {
		if (mode == eMode.transition)
		{
			rb.velocity = Vector3.zero;
			anim.speed = 0;
			roomPos = transitionPos; // сохраняем местоположение игрока относительно комнаты
			if (Time.time < transitionDone) return;
			mode = eMode.idle;
		}
		if (!isMovingX && !isMovingY && mode != eMode.attack) Stop();
		else if (isMovingX)
		{
			mode = eMode.moving;
			if (isRight) MoveRight();
			else MoveLeft();
		}
		else if (isMovingY)
        {
			mode = eMode.moving;
			if (isUp) MoveUp();
			else MoveDown();
		}
		if (mode == eMode.attack)
        {
			anim.CrossFade("Attack_" + facing, 0);
			anim.speed = 0;
        }
		if (Time.time >= timeAttackDone && mode == eMode.attack)
		{
			mode = eMode.idle;
			Stop();
		}
    }
	public void SetHorizontalMovement(bool _isRight)
    {
		isRight = _isRight;
		isMovingX = true;
    }
	public void SetVerticalMovement(bool _isUp)
	{
		isUp = _isUp;
		isMovingY = true;
	}
	public void DontAllowMovewment()
    {
		isMovingX = false;
		isMovingY = false;
    }
    void MoveRight()
    {
		facing = 0;
		rb.velocity = new Vector2(speed, rb.velocity.y);
		anim.CrossFade("Walk_0", 0); // 0 - right
		anim.speed = 1;
	}
	void MoveLeft()
	{
		facing = 2;
		rb.velocity = new Vector2(-speed, rb.velocity.y);
		anim.CrossFade("Walk_2", 0); // 2 - left
		anim.speed = 1;
	}
	void MoveUp()
    {
		facing = 1;
		rb.velocity = new Vector2(rb.velocity.x, speed);
		anim.CrossFade("Walk_1", 0); // 1 - up
		anim.speed = 1;
	}
	void MoveDown()
	{
		facing = 3;
		rb.velocity = new Vector2(rb.velocity.x, -speed);
		anim.CrossFade("Walk_3", 0); // 3 - down
		anim.speed = 1;
	}
	void Stop()
	{
		rb.velocity = new Vector2(0f, 0f);
		anim.CrossFade("Walk_" + facing, 0);
		anim.speed = 0;
	}

	public void Attack()
    {
		if (Time.time >= timeAttackNext)
		{
			mode = eMode.attack;
			timeAttackDone = Time.time + attackDuration;
			timeAttackNext = Time.time + attackDelay;
		}
    }

	// IFcaingMover methods

	public int GetFacing()
    {
		return facing;
    }
	public bool moving
    {
		get
        {
			return (mode == eMode.moving);
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
	public Vector2 GetRoomPosOnGrid (float mult = -1)
    {
		return inRm.GetRoomPosOnGrid(mult);
    }
}
