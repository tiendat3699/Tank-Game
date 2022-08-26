using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Player : Target
{
    // Start is called before the first frame update
    [HideInInspector]
    public UnityEvent<float> onTakeDamage;
    public ParticleSystem healingEffect;
    protected override void InitStart()
    {
        gameManager.SetPlayer(gameObject);
        gameManager.setHealth(health);
        gameManager.setInitHealth(health);
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

    protected override void die(Vector3 vector) {
        gameObject.SetActive(false);
        GameObject deadBody = Instantiate(DeadTank, transform.position, transform.rotation);
        deadBody.GetComponent<detroyed>().explode(vector);
    }

    public void healing(float healing) {
        health = healing;
        gameManager.setHealth(healing);
        healingEffect.Play();
    }
}