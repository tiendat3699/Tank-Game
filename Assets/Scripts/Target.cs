using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DeadTank;
    public float health = 50f;
    public bool immortal = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage, Vector3 vector) {
        if(!immortal) {
            health -= damage;
            if(health<=0) {
                die(vector);
            }
        }
    } 

    private void die(Vector3 vector) {
        Destroy(gameObject);
        GameObject deadBody = Instantiate(DeadTank, transform.position, transform.rotation);
        deadBody.GetComponent<detroyed>().explode(vector);
    }
}
