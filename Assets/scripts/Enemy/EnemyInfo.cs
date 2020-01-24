using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public float hp, speed;
    public int damage;
    public int money;
    public GameManager manager;
    public void ParseValue(int hp, int damage, float speed, int money)
    {
        this.hp = hp;
        this.damage = damage;
        this.speed = speed;
        this.money = money;
    }
    void Start()
    {
        Init();
    }
    void Init()
    {
        if (!manager) manager = GameManager.FindObjectOfType<GameManager>();
    }
    public void Attack()
    {
        Destroy(this.gameObject);
        manager.UpdateHP(damage);
    }
}
