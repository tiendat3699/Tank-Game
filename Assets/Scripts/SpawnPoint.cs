using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private GameObject[] listEnemy;
    private GameManager gameManager;
    // Start is called before the first frame update
    private void Awake() {
        gameManager = GameManager.Instance;
        gameManager.onStartGame.AddListener(startSpawnEnemy);
    }
    void Start()
    {
        gameManager.SetSpawnPoint(transform);
        listEnemy = gameManager.listEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void startSpawnEnemy() {
        int enemyIndex = Random.Range(0, listEnemy.Length - 1);
        Instantiate(listEnemy[enemyIndex], transform.position, transform.rotation);
    }



}
