using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject[] listEnemy;
    public int maxEnemiesAmount;
    private int _enemiesAmount;
    private int _enemiesCurrent;
    private int _score = 0;
    private float _health = 0;
    private List<Transform> _listWalkPoint = new List<Transform>();
    private List<Transform> _listSpawnPoint = new List<Transform>();
    private GameObject _player;
    public UnityEvent<int> onUpdateScore;
    public UnityEvent<float> onUpdateHealth;
    public UnityEvent<int> onUpdateEnemiesAmount;
    public UnityEvent<int> onUpdateEnemiesDetroyed;

    // Start is called before the first frame update
    private void Awake() {
        if(Instance != null && Instance !=this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    private void Start()
    {
        onUpdateHealth?.Invoke(_health);
        onUpdateScore?.Invoke(_score);
    }

    // Update is called once per frame
    private void Update()
    {
        if(_enemiesAmount >= maxEnemiesAmount && _enemiesCurrent == 0) {
            Debug.Log("you win!!");
        }
    }

    private void OnDestroy() {
        StopAllCoroutines();
    }

    public void setScore(int score, GameObject gameObj) {
        if(gameObj.tag != "Player") {
            _score += score;
            onUpdateScore?.Invoke(_score);
        }
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
        onUpdateEnemiesDetroyed?.Invoke(_enemiesCurrent);
        if(_enemiesAmount < maxEnemiesAmount) {
            StartCoroutine(AutoSpawnEnemyYield());
        }
    }

    public void SetPlayer(GameObject player) {
        _player = player;
    }

    public GameObject getPlayer() {
        return _player;
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

}
