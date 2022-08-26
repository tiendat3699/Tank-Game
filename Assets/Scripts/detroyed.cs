using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detroyed : MonoBehaviour
{
    public ParticleSystem Smoke, Explore;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void explode(Vector3 vector) {
        GetComponent<Rigidbody>().AddForce(transform.up * 5f + vector * 10f, ForceMode.VelocityChange);
        transform.GetChild(0).GetComponent<Rigidbody>().AddForce(transform.up * 5f + vector * 10f,ForceMode.VelocityChange);
        Explore.Play();
        SoundManager.Instance.play3DSound(gameObject, Sounds.explosionSound);
        StartCoroutine(PlaySmoke(Explore.main.duration));
    }

    IEnumerator PlaySmoke(float time) {
        yield return new WaitForSeconds(time);
        if(Smoke.isStopped) {
            Smoke.Play();
        }
        if(!(gameObject.tag == "player")) {
            Destroy(gameObject, 5f);
        }
    }

}
