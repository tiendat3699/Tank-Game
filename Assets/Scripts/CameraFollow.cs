using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public GameObject MainCam;
    public GameObject SecondCam;
    private Vector3 tankVector3;
    private Transform CamT;

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
        
        ViewObstructed();
    }

    private void ViewObstructed() {
        RaycastHit hit;
        if(Physics.Linecast(MainCam.transform.position, target.position, out hit)) {
            if(hit.transform.gameObject.tag != "Player") {
                SecondCam.SetActive(true);
                MainCam.SetActive(false);
                CamT = SecondCam.transform;
            } else{
                SecondCam.SetActive(false);
                MainCam.SetActive(true);
                CamT = MainCam.transform;
            }
        }
    }

    public Transform GetTransformCam() {
        return CamT;
    }

}
