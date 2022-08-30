using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform barrelEnd;
    public GameObject HitEffect;
    public GameObject MissEffect;
    public float range = 50f;
    public float damage = 10f;
    public float bulletSpeed = 300f;
    public bool shootAble = true;
    public float waitBeforeNextShot = 0.25f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy() {
        StopAllCoroutines();
    }

    IEnumerator ShootingYield ()
    {
        yield return new WaitForSeconds (waitBeforeNextShot);
        shootAble = true;
    }

    public void PlayerShoot(Ray rayOrigin)
    {   
        if (shootAble) {
            shootAble = false;
            RaycastHit Hit;
            barrelEnd.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            SoundManager.Instance.play3DSound(barrelEnd.gameObject, Sounds.shootSound);

            GetComponent<Rigidbody>().AddForce(-barrelEnd.forward * 0.2f, ForceMode.VelocityChange);
            if(Physics.Raycast(rayOrigin, out Hit, range)) {
                GameObject hitShot = Instantiate(HitEffect, Hit.point, Quaternion.LookRotation(Hit.normal));
                SoundManager.Instance.play3DSound(hitShot, Sounds.hitSound);

                if(Hit.rigidbody && Hit.transform.tag != transform.tag) {
                    Hit.rigidbody.AddForce(-Hit.normal * 2f, ForceMode.VelocityChange);
                    if(Hit.transform.tag == "Enemy") {
                        Hit.transform.GetComponent<Target>().TakeDamage(damage,-Hit.normal);
                    }
                }
                Destroy(hitShot,2f);
            } else {
                GameObject missShot = Instantiate (MissEffect, rayOrigin.GetPoint(range), Quaternion.LookRotation(rayOrigin.origin));
                Destroy(missShot,2f);
            }

            StartCoroutine (ShootingYield ());
        }
    }

    public void EnemyShoot(Ray rayOrigin, int accuracy = 5)
    {   
        if (shootAble) {
            shootAble = false;
            RaycastHit Hit;
            Ray newRay = rayOrigin;
            newRay.direction = getRandomDirection(newRay.direction, accuracy);
            barrelEnd.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            SoundManager.Instance.play3DSound(barrelEnd.gameObject, Sounds.shootSound);

            GetComponent<Rigidbody>().AddForce(-barrelEnd.forward * 0.2f, ForceMode.VelocityChange);
            if(Physics.Raycast(newRay, out Hit, range)) {
                GameObject hitShot = Instantiate(HitEffect, Hit.point, Quaternion.LookRotation(Hit.normal));
                SoundManager.Instance.play3DSound(hitShot, Sounds.hitSound);

                if(Hit.rigidbody && Hit.transform.tag != transform.tag) {
                    Hit.rigidbody.AddForce(-Hit.normal * 2f, ForceMode.VelocityChange);
                    if(Hit.transform.tag == "Player") {
                        Hit.transform.GetComponent<Target>().TakeDamage(damage,-Hit.normal);
                    }
                }
                Destroy(hitShot,2f);
            } else {
                GameObject missShot = Instantiate (MissEffect, newRay.GetPoint(range), Quaternion.LookRotation(newRay.origin));
                Destroy(missShot,2f);
            }

            StartCoroutine (ShootingYield ());
        }
    }

    private Vector3 getRandomDirection(Vector3 directionOrigin, int accuracy) {
        float range = (float)(accuracy+7)/10;
        range = 1.0f - range;
        float y  = Random.Range(-range,range);
        float x = Random.Range(-range,range);
        return directionOrigin + new Vector3(x,y,0);
    }
}
