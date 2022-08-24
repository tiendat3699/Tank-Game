using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingEnemy : MonoBehaviour
{
    public Image enemyIcon;
    private int _enemies;
    private List<Image> _listEnemyIcon = new List<Image>();
    // Start is called before the first frame update
    private void Awake() {
        GameManager.Instance.onUpdateEnemiesDetroyed.AddListener(updateEnemyIcon);
    }
    void Start()
    {
        _enemies = GameManager.Instance.maxEnemiesAmount;
        for(int i = 0; i < _enemies; i++) {
            Image enemyIconClone = Instantiate(enemyIcon);
            enemyIconClone.transform.SetParent(transform);

            _listEnemyIcon.Add(enemyIconClone);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void updateEnemyIcon(int enemiesCurrent) {
        _enemies -= 1;
        _listEnemyIcon[_enemies].color = Color.black;
    }
}
