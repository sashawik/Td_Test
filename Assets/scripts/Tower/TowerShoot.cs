using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    public GameObject target;
    public GameManager manager;
    public bool isTower = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(GameManager.TowerInfo tower)
    {
        isTower = true;
        StartCoroutine(PreShoot(tower.range,tower.damage,tower.speed));
    }
    IEnumerator PreShoot(int range,float damage,float speed)
    {
        if (!target) GetTarget(range);
        else Shoot(damage);
        yield return new WaitForSeconds(1);
        StartCoroutine(PreShoot(range, damage, speed));
    }
    void GetTarget(int range)
    {
        foreach (EnemyInfo enemy in FindObjectsOfType<EnemyInfo>())
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.position) < range) { target = enemy.gameObject; break; };
        }
    }
    void Shoot(float damage)
    {
        target.GetComponent<EnemyInfo>().hp-=damage;
        if (target.GetComponent<EnemyInfo>().hp < 0) { manager.money += target.GetComponent<EnemyInfo>().money; Destroy(target); target = null;  };
    }
}
