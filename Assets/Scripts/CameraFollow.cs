using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 tankVector3;
    private Transform Obstruction;
    // Start is called before the first frame update
    void Start()
    {
        Obstruction = target;
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
        if(Physics.Linecast(transform.position, target.position, out hit)) {
            if(hit.transform.gameObject.tag != "Player") {
                Obstruction = hit.transform;
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            } else{
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
    }

}
