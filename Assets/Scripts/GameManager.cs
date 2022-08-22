using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Text ScoreText;
    public Text HealthText;
    private int _score = 0;
    private float _health = 0;
    private List<Transform> _listWalkPoint = new List<Transform>();

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

    }

    // Update is called once per frame
    private void Update()
    {
        ScoreText.text = "Score:" + _score;
    }

    public void setScore(int score, GameObject gameObj) {
        if(gameObj.tag != "Player") {
            _score += score;
            ScoreText.text = "Score:" + _score;
        }
    }

    public void setHealth(float health) {
        _health = health;
        HealthText.text = "HP:" + (int)_health;
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

}
