using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Информация про сессию")]
    public int maxHp;
    public int currentHp;
    [Header("Координаты")]
    public Transform[] point;
    private Vector3[] pointCoords;
    public Transform startPos;
    PathType pathsystem = PathType.CatmullRom;
    [Header("Префабы врагов")]
    public GameObject[] preEnemy;
    [System.Serializable]
    public struct WaveInfo
    {
        public int WaveNumber,countEnemy;
        public float TimeBetweenMobs, TimeToNextWave;
    }
    [Header("Инфо про волны")]
    public WaveInfo[] waveInfo;
    GameObject enemy;
    
    [Header("UI")]
    public TextMeshProUGUI Lives;
    public TextMeshProUGUI Waves;
    public TextMeshProUGUI Money;
    public GameObject UITower;
    [System.Serializable]
    public struct TowerInfo
    {
        public int price, range;
        public float damage,speed;
    }
    [Header("Инфо про башни")]
    public TowerInfo[] Towers;
    [System.Serializable]
    public struct EnemyType
    {
        public int hp, damage,money;
        public float speed; //Increase to Slow
    }
    [Header("Инфо про врагов")]
    public EnemyType[] Enemys;
    public int money;
    public GameObject sellIcon;
    public Sprite default_sprite;
    private int _currentwave;
    void Awake()
    {
        Init();
        GetVectors();
        StartCoroutine(SpawnWave(waveInfo[_currentwave].countEnemy, waveInfo[_currentwave].TimeBetweenMobs, waveInfo[_currentwave].TimeToNextWave));
    }
    void GetVectors()
    {
        int i = 0;
        foreach(Transform point in point)
        {
            pointCoords[i] = point.position;
            i++;
        }
    }
    private void Update()
    {
        UpdateUI();
    }
    void Init()
    {
        currentHp = maxHp;
        _currentwave = 0;
        money = 100;
        pointCoords = new Vector3[point.Length];
        UpdateUI();
    }
    IEnumerator SpawnWave(int count,float TimeBetweenSpawn,float TimeToNewWave)
    {
        _currentwave++;
        UpdateUI();
        int i = 0;
        while (i<count)
        {
            CreateEnemy();
            i++;
            yield return new WaitForSeconds(TimeBetweenSpawn);
        }
        yield return new WaitForSeconds(TimeToNewWave);
        if (_currentwave < waveInfo.Length)
            StartCoroutine(SpawnWave(waveInfo[_currentwave].countEnemy, waveInfo[_currentwave].TimeBetweenMobs, waveInfo[_currentwave].TimeToNextWave));
        else
            ShowWinScreen();
    }
    void CreateEnemy()
    {
        int rEnemy = Random.Range(0, 4);
        enemy = Instantiate(preEnemy[rEnemy], startPos);
        enemy.GetComponent<EnemyInfo>().ParseValue(Enemys[rEnemy].hp, Enemys[rEnemy].damage, Enemys[rEnemy].speed,Enemys[rEnemy].money);
        enemy.name = _currentwave.ToString();
        Tween tween = enemy.transform.DOPath(pointCoords, Enemys[rEnemy].speed, pathsystem).SetEase(Ease.Linear).OnComplete(enemy.GetComponent<EnemyInfo>().Attack);
        enemy.transform.DOPlay();
        
    }
    void ShowLooseScreen()
    {

    }
    public void UpdateHP(int damage)
    {
        if (currentHp-damage > 0) currentHp-=damage;
        else ShowLooseScreen();
        UpdateUI();
    }
    void UpdateUI()
    {
        Waves.text = _currentwave.ToString() + "/" + waveInfo.Length.ToString();
        Lives.text = currentHp.ToString() + "/" + maxHp.ToString();
        Money.text = money.ToString();
    }
    void ShowWinScreen()
    {
        Debug.Log("win");
    }
    private GameObject clickedCell;
    public void ShowTower(GameObject cell)
    {
        clickedCell = cell;
        UITower.transform.DOMoveY(-4.63f, 1.5f);
        if (clickedCell.GetComponent<TowerShoot>().isTower)
            sellIcon.SetActive(true);
    }
    public void Build(int type,Sprite sprite)
    {
        if (sellIcon) sellIcon.SetActive(false);
        if (money >= Towers[type].price)
        {
            clickedCell.GetComponent<SpriteRenderer>().sprite = sprite;
            money -= Towers[type].price;
            clickedCell.GetComponent<TowerShoot>().Init(Towers[type]);
            clickedCell.GetComponent<ClickOnSlot>().type = type;
            UpdateUI();
        }
    }
    public void Sell()
    {
        money += Towers[clickedCell.GetComponent<ClickOnSlot>().type].price / 2;
        clickedCell.GetComponent<TowerShoot>().StopAllCoroutines();
        clickedCell.GetComponent<SpriteRenderer>().sprite = default_sprite;
    }
}
