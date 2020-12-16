using UnityEngine;

public class TouchControls : MonoBehaviour
{
    [Header("Touch Indicator GameObject")]
    public GameObject touch; // геймобджект кружочка, показывающего место касания
    [Header("Joystick Radius GameObject")]
    public Transform threshold; // геймобджект радиуса джойстика
    [Header("Player's Speed")]
    public float speed; // скорость игрока
    [Header("Current Direction")]
    [SerializeField]
    private Vector3 direction; // вектор направления касания
    private bool isMoving;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        touch.transform.position = threshold.position;
        isMoving = false;
        anim.speed = 1;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // вычисляем координаты касания игрока
            direction = touchPos - threshold.position; // вычисляем направление движения путем разности вектора касания и вектора начальной точки джойстика
            touchPos.z = 0; // зануляем третью координату, чтоб не было багов
            direction.z = 0;

            if (direction.magnitude < Mathf.Sqrt(2)) // ограничение радиуса джойстика
            {
                touch.transform.position = touchPos; // если касание в джойстике - перемещаем индикатор
                isMoving = true;
            }
            if (isMoving)
                Move(direction); // двигаем в любом случае, если есть касание, чтобы игрок мог выходить пальцем за пределы ограничительного радиуса
        }
        else
        {
            isMoving = false;
            touch.transform.position = threshold.position; // нет касания - возвращаем индикатор касания на место
            Move(Vector3.zero); // останавливаем игрока
            anim.CrossFade("Idle", 0);
        }
    }
    private void Move(Vector3 dir)
    {
        transform.Translate(dir.normalized * speed * Time.deltaTime);
        if (isMoving)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = dir.x < 0;
            anim.CrossFade("Walk", 0);
        }
        else
            anim.CrossFade("Idle", 0);
    }
}