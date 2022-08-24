using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Player : Target
{
    // Start is called before the first frame update
    public UnityEvent<float> onTakeDamage;
    void Start()
    {
        gameManager.SetPlayer(gameObject);
        gameManager.setHealth(health);
    }

    public override void TakeDamage(float damage, Vector3 vector) {
        onTakeDamage?.Invoke(damage);
        if(!immortal) {
            health -= damage;
            gameManager.setHealth(health);
            if(health<=0) {
                die(vector);
            }
        } 
    }
}