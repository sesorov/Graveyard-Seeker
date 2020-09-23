using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	[Range(0, 5)]
	public float speed = 2f;

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
		if (!isMovingX && !isMovingY) Stop();
		else if (isMovingX)
		{
			if (isRight) MoveRight();
			else MoveLeft();
		}
		else if (isMovingY)
        {
			if (isUp) MoveUp();
			else MoveDown();
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
    public void MoveRight()
    {
		rb.velocity = new Vector2(speed, rb.velocity.y);
		anim.CrossFade("WalkRight_0", 0);
		anim.speed = 1;
	}
	public void MoveLeft()
	{
		rb.velocity = new Vector2(-speed, rb.velocity.y);
		anim.CrossFade("WalkLeft_2", 0);
		anim.speed = 1;
	}
	public void MoveUp()
    {
		rb.velocity = new Vector2(rb.velocity.x, speed);
		anim.CrossFade("WalkUp_1", 0);
		anim.speed = 1;
	}
	public void MoveDown()
	{
		rb.velocity = new Vector2(rb.velocity.x, -speed);
		anim.CrossFade("WalkDown_3", 0);
		anim.speed = 1;
	}
	public void Stop()
	{
		rb.velocity = new Vector2(0f, 0f);
		anim.speed = 0;
	}
}
