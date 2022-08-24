using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Target
{
    public int Score = 100;
    private bool _getHit = false;
    // Start is called before the first frame update

    private void Start() {
        gameManager.UpdateEnemiesAmount();
        gameManager.UpdateEnemieCurrent();
    }

    public override void TakeDamage(float damage, Vector3 vector) {
        if(!immortal) {
            health -= damage;
            if(health<=0) {
                die(vector);
                gameManager.setScore(Score, gameObject);
            }
            _getHit = true;
        } 
    }

     protected override void die(Vector3 vector) {
        Destroy(gameObject);
        GameManager.Instance.UpdateEnemiesDetroyed();
        GameObject deadBody = Instantiate(DeadTank, transform.position, transform.rotation);
        deadBody.GetComponent<detroyed>().explode(vector);
    }

    public bool GetHit() {
        return _getHit;
    }

    public void ResetHit() {
        _getHit = false;
    }

}
