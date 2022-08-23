using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int _score = 0;
    private float _health = 0;
    private List<Transform> _listWalkPoint = new List<Transform>();
    private GameObject _player;
    public UnityEvent<int> updateScore;
    public UnityEvent<float> updateHealth;

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
        updateHealth?.Invoke(_health);
        updateScore?.Invoke(_score);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void setScore(int score, GameObject gameObj) {
        if(gameObj.tag != "Player") {
            _score += score;
            updateScore?.Invoke(_score);
        }
    }

    public void setHealth(float health) {
        _health = health;
        updateHealth?.Invoke(_health);

    }

    public float getHealthVal() {
       return _health; 
    }

    public void SetWalkPoint(Transform walkPoint) {
        _listWalkPoint.Add(walkPoint);
    }

    public List<Transform> GetListWalkPoint() {
        return _listWalkPoint;
    }

    public void SetPlayer(GameObject player) {
        _player = player;
    }

    public GameObject getPlayer() {
        return _player;
    }

}
