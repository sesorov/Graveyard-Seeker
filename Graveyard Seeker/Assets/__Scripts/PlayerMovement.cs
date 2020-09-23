using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	public enum eMode { idle, moving, attack, transition }

	[Header("Set in Inspector")]
	[Range(0, 5)]
	public float speed = 2f;
	public float attackDuration = 0.25f; // в секундах

	public float attackDelay = 0.5f; // задержка между атаками
	[Header("Set Dynamically")]
	public eMode mode = eMode.idle;

	public int facing = 0;
	private float timeAttackDone = 0; // время, когда завершается анимация атаки
	private float timeAttackNext = 0; // время, когда игрок снова может атаковать

    private Rigidbody rb;

	private bool isMovingX = false;
	private bool isMovingY = false;
	private bool isRight = false;
	private bool isUp = false;
	private Animator anim;
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
	}
    private void Update()
    {
		Movement();
	}
	private void Movement()
    {
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
}
