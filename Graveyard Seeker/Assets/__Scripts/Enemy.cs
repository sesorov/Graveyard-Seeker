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
    public float knockbackSpeed = 10;
    public float knockbackDuration = 0.25f;
    public float invicibleDuration = 0.5f;

    [Header("Set Dynamically: Enemy")]
    public float health;
    public bool invicible = false;
    public bool knockback = false;

    private float invicibleDone = 0;
    private float knockbackDone = 0;
    private Vector3 knockbackVel;

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
    protected virtual void Update()
    {
        // проверим состояние неуязвимости и необходимость отбрасывания
        if (invicible && Time.time > invicibleDone) invicible = false; // проверяем окончание неуязвимости
        sRend.color = invicible ? Color.red : Color.white; // если враг неуязвим, окрашиваем его в красный
        if (knockback)
        {
            rigid.velocity = knockbackVel;
            if (Time.time < knockbackDone) return;
        }

        anim.speed = 1;
        knockback = false;
    }
    private void OnTriggerEnter(Collider colld)
    {
        if (invicible) return; // враг неуязвим - выходим
        DamageEffect dEff = colld.gameObject.GetComponent<DamageEffect>();
        if (dEff == null) return; // если нет DamageEffect - выходим

        health -= dEff.damage;
        if (health <= 0) Die();
        invicible = true;
        invicibleDone = Time.time + invicibleDuration;

        if (dEff.knockback) // выполняем отбрасывание от удара
        {
            Vector3 delta = transform.position - colld.transform.position;
            if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y)) // отбрасывание по горизонтали
            {
                delta.x = (delta.x > 0) ? 1 : -1;
                delta.y = 0;
            }
            else // отбрасывание по вертикали
            {
                delta.x = 0;
                delta.y = (delta.y > 0) ? 1 : -1;
            }

            knockbackVel = delta * knockbackSpeed;
            rigid.velocity = knockbackVel; // применяем величину отбрасывания к игроку
            knockback = true;
            knockbackDone = Time.time + knockbackDuration;
            anim.speed = 0;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
