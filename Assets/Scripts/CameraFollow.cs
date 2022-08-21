using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 tankVector3;
    // Start is called before the first frame update
    void Start()
    {
        tankVector3 = target.transform.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Ray rayOrigin = new Ray(target.position, target.forward);
        transform.position = target.transform.position - target.transform.rotation * tankVector3;
        transform.rotation = Quaternion.LookRotation(rayOrigin.direction);
    }
}
