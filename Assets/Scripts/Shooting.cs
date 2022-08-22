using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform barrelEnd;
    public GameObject HitEffect;
    public TrailRenderer TracerBullet;
    public float range = 50f;
    public float damage = 10f;
    public float bulletSpeed = 300f;
    public float despawnTime = 3.0f;
    public bool shootAble = true;
    public float waitBeforeNextShot = 0.25f;
    private Transform targetT;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator ShootingYield ()
    {
        yield return new WaitForSeconds (waitBeforeNextShot);
        shootAble = true;
    }
    public void Shoot (Ray rayOrigin, bool isPlayer = true, int accuracy = 5)
    {
        if (shootAble) {
            shootAble = false;
            RaycastHit Hit;
            var tracer = Instantiate(TracerBullet, barrelEnd.position, Quaternion.identity);
            tracer.AddPosition(barrelEnd.position);
            Ray finalRay = rayOrigin;

            if(!isPlayer){
                Ray newRay = rayOrigin;
                newRay.direction = getRandomDirection(newRay.direction, accuracy);
                finalRay = newRay;
            }
            
            GetComponent<Rigidbody>().AddForce(-barrelEnd.forward * 0.2f, ForceMode.VelocityChange);
            if(Physics.Raycast(finalRay, out Hit, range)) {
                GameObject Effect =  Instantiate (HitEffect, Hit.point, Quaternion.LookRotation(Hit.normal));
                if(Hit.rigidbody && Hit.transform.tag != transform.tag) {
                    Hit.rigidbody.AddForce(-Hit.normal * 2f, ForceMode.VelocityChange);
                    Target target = Hit.transform.GetComponent<Target>();
                    if(target && target.isActiveAndEnabled) {
                        Hit.transform.GetComponent<Target>().TakeDamage(damage,-Hit.normal);
                    }

                }
                Destroy(Effect,2f);
            }
            tracer.transform.Translate(barrelEnd.forward * bulletSpeed * Time.deltaTime);

            barrelEnd.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
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
