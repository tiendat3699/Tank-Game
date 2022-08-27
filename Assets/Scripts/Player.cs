using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Player : Target
{
    // Start is called before the first frame update
    [HideInInspector]
    public UnityEvent<float> onTakeDamage;
    public ParticleSystem itemEffect;
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

    public void healing(float healing, Material mat) {
        health = healing;
        gameManager.setHealth(healing);
        itemEffect.GetComponent<ParticleSystemRenderer>().material = mat;
        var main = itemEffect.main;
        main.loop = false;
        itemEffect.Play();
    }

    public void shield(float time, Material mat) {
        immortal = true;
        itemEffect.GetComponent<ParticleSystemRenderer>().material = mat;
        var main = itemEffect.main;
        main.loop = true;
        itemEffect.Play();
        StartCoroutine(StartActiveItem(time, ()=> {
            itemEffect.Stop();
            immortal = false;
        }));
    }

    public void doubleDamage(float time, Material mat) {
        var shooting = GetComponent<Shooting>();
        var damageOrigin =  shooting.damage;
        shooting.damage = damageOrigin * 2;
        itemEffect.GetComponent<ParticleSystemRenderer>().material = mat;
        var main = itemEffect.main;
        main.loop = true;
        itemEffect.Play();
        StartCoroutine(StartActiveItem(time, ()=> {
            itemEffect.Stop();
            shooting.damage = damageOrigin;
        }));

    }

    IEnumerator StartActiveItem(float time, Action callback) {
        yield return new WaitForSeconds(time);
        callback();
    }
}