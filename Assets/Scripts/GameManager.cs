using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject[] listEnemy;
    public int maxEnemiesAmount;
    private int _enemiesAmount = 0;
    private int _enemiesCurrent = 0;
    private int _enemiesDetroyed = 0;
    private int _score = 0;
    private float _health = 0;
    private List<Transform> _listWalkPoint = new List<Transform>();
    private List<Transform> _listSpawnPoint = new List<Transform>();
    private GameObject _player;
    [HideInInspector]
    public UnityEvent<int> onUpdateScore;
    [HideInInspector]
    public UnityEvent<float> onUpdateHealth;
    [HideInInspector]
    public UnityEvent<int> onUpdateEnemiesAmount;
    [HideInInspector]
    public UnityEvent<int> onUpdateEnemiesDetroyed;
    [HideInInspector]
    public UnityEvent<int> onCounttime;
    [HideInInspector]
    public UnityEvent onStartGame;
    [HideInInspector]
    public UnityEvent<bool> onGameover;
    [HideInInspector]
    public UnityEvent onReset;
    private int timeCount = 3;
    private bool isWin;

    // Start is called before the first frame update
    private void Awake() {
        if(Instance != null && Instance !=this) {
            Destroy(this);
        } else {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        onReset.AddListener(Init);
    }

    private void Start()
    {

    }
    

    // Update is called once per frame
    private void Update()
    {
        if(timeCount == -1) {
            onStartGame?.Invoke();
            timeCount -= 1;
        }

        if(_enemiesAmount >= maxEnemiesAmount && _enemiesCurrent == 0) {
            isWin = true;
            onGameover?.Invoke(isWin);
        } 

        if(_health <= 0) {
            isWin = false;
            onGameover?.Invoke(isWin);
        }
    }

    private void OnDestroy() {
        StopAllCoroutines();
    }

    private void Init() {
        StartCoroutine(InitYield());
    }


    IEnumerator InitYield() {
        yield return new WaitUntil(()=> SceneManager.GetActiveScene().name == "tankScene");
        onUpdateHealth?.Invoke(_health);
        onUpdateScore?.Invoke(_score);
        startCountTime();
    }

    public void ResetGame() {
        Instance.enabled = false;
        _enemiesAmount = 0;
        _enemiesCurrent = 0;
        _enemiesDetroyed = 0;
        _score = 0;
        timeCount = 3;
        _listWalkPoint.Clear();
        _listSpawnPoint.Clear();
        Instance.enabled = true;
        onReset?.Invoke();
    }

    public void setScore(int score) {
            _score += score;
            onUpdateScore?.Invoke(_score);
    }

    public int getScore() {
        return _score;
    }

    public void setHealth(float health) {
        _health = health;
        onUpdateHealth?.Invoke(_health);

    }

    public float getHealth() {
        return _health;

    }

    public void SetWalkPoint(Transform walkPoint) {
        _listWalkPoint.Add(walkPoint);
    }


    public List<Transform> GetListWalkPoint() {
        return _listWalkPoint;
    }

    public void SetSpawnPoint(Transform SpawnPoint) {
        _listSpawnPoint.Add(SpawnPoint);
    }

    public List<Transform> GetSpawnPoint() {
        return _listSpawnPoint;
    }

    public void UpdateEnemiesAmount() {
        _enemiesAmount += 1;
        onUpdateEnemiesAmount?.Invoke(_enemiesAmount);
    }

    public void UpdateEnemieCurrent() {
        _enemiesCurrent += 1;
    }

    public void UpdateEnemiesDetroyed() {
        _enemiesCurrent -= 1;
        _enemiesDetroyed += 1;
        onUpdateEnemiesDetroyed?.Invoke(_enemiesCurrent);
        if(_enemiesAmount < maxEnemiesAmount) {
            StartCoroutine(AutoSpawnEnemyYield());
        }
    }

    public int GetEnemiesDetroyed() {
        return _enemiesDetroyed;
    }

    public void SetPlayer(GameObject player) {
        _player = player;
    }

    public GameObject getPlayer() {
        return _player;
    }

    public bool IsWin() {
        return isWin;
    }

    private void AddRandomEnemy() {
        int enemyIndex = Random.Range(0, listEnemy.Length);
        int spawnPointIndex = Random.Range(0, _listSpawnPoint.Count);
        Instantiate(listEnemy[enemyIndex], _listSpawnPoint[spawnPointIndex].position, _listSpawnPoint[spawnPointIndex].rotation);
    }
    
    IEnumerator AutoSpawnEnemyYield() {
        yield return new WaitForSeconds(3f);
        AddRandomEnemy();
    }

    private void startCountTime() {
        onCounttime?.Invoke(timeCount);
        StartCoroutine(WaitForStart());
    }
    

    IEnumerator WaitForStart() {
        yield return new WaitForSeconds(1f);
        if(timeCount >= 0) {
            timeCount -= 1;
            startCountTime();
        }
    }
}
