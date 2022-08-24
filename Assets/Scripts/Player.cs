using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Target
{
    // Start is called before the first frame update
    void Start()
    {
        gameManager.SetPlayer(gameObject);
        gameManager.setHealth(health);
    }

    public override void TakeDamage(float damage, Vector3 vector) {
        if(!immortal) {
            health -= damage;
            gameManager.setHealth(health);
            if(health<=0) {
                die(vector);
            }
        } 
    }
}