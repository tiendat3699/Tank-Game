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
    }
    void Start()
    {
        gameManager.SetSpawnPoint(transform);
        listEnemy = gameManager.listEnemy;

        int enemyIndex = Random.Range(0, listEnemy.Length - 1);
        Instantiate(listEnemy[enemyIndex], transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
