using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class EnemyText : MonoBehaviour
{
    private Text enemyText;
    private int enemiesAmount;
    // Start is called before the first frame update
    private void Awake() {
        enemyText = GetComponent<Text>();
        GameManager.Instance.onUpdateEnemiesDetroyed.AddListener(updateEnemytext);
        enemiesAmount = GameManager.Instance.maxEnemiesAmount;
    }
    void Start()
    {
        enemyText.text = "Enemy:" + enemiesAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void updateEnemytext(int amount) {
        enemiesAmount -= 1;
        enemyText.text = "Enemy:" + enemiesAmount;
    }
}
