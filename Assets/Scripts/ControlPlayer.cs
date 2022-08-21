using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    public Transform turret;

    // Start is called before the first frame update
    [Range(10, 6000)]
    public float speed = 3000;
    [Range(10, 6000)]
    public float rotateSpeed = 1000;
    [Range(10, 6000)]
    public float brakeForce = 6000;
    [Range(1,5)]
    public float rotateSpeedMouse = 1;

    [Range(-50, 50)]
    public float rotateXRangeMax = 30f;

     [Range(-50, 50)]
    public float rotateXRangeMin = -30f;

    private float rotationYCanon = 0f;

    public Transform BackRightWheelTransform,BackLeftWheelTransform,FrontRightWheelTransform, FrontLeftWheelTransform;
    public WheelCollider BackRightWheelCollider,BackLeftWheelCollider,FrontRightWheelCollider, FrontLeftWheelCollider;
    public Camera Cam;

    public GameObject SmokeEffectLeft;
    public GameObject SmokeEffectRight;

    private Rigidbody rigidbodyTank;

    private bool isBraking = false;

    private float VerticalInput;
    private float HorizontalInput;

    private void Awake() {
        rigidbodyTank = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float y = rotateSpeedMouse * Input.GetAxis("Mouse X");
        rotationYCanon += rotateSpeedMouse * Input.GetAxis("Mouse Y");

        VerticalInput = Input.GetAxis("Vertical");
        HorizontalInput = Input.GetAxis("Horizontal");

        turret.transform.Rotate(0, y, 0);

        var cannonTransform = turret.transform.GetChild(0).transform;

        rotationYCanon = Mathf.Clamp(rotationYCanon, rotateXRangeMin, rotateXRangeMax);

        Vector3 rotAngles = cannonTransform.eulerAngles;
        rotAngles.x = Mathf.MoveTowards(rotAngles.x, -rotationYCanon, rotateSpeed);
        cannonTransform.eulerAngles = rotAngles;

        if (Input.GetKey (KeyCode.Mouse0)) {
            GetComponent<Shooting>().Shoot(Cam.transform, true);
        }



        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate() {
        
        // rigidbodyTank.AddForce(transform.forward * VerticalInput);

        if(isBraking) {
            FrontLeftWheelCollider.brakeTorque = brakeForce;
            FrontRightWheelCollider.brakeTorque = brakeForce;
            BackLeftWheelCollider.brakeTorque = brakeForce;
            BackRightWheelCollider.brakeTorque = brakeForce;
        } else {
            if(HorizontalInput != 0) {
                FrontLeftWheelCollider.motorTorque = HorizontalInput * rotateSpeed;
                FrontRightWheelCollider.motorTorque = -HorizontalInput * rotateSpeed;
                BackLeftWheelCollider.motorTorque = HorizontalInput * rotateSpeed;
                BackRightWheelCollider.motorTorque = -HorizontalInput * rotateSpeed;
            } else {
                FrontLeftWheelCollider.motorTorque = VerticalInput * speed;
                FrontRightWheelCollider.motorTorque = VerticalInput * speed;
            }
            FrontLeftWheelCollider.brakeTorque = 0;
            FrontRightWheelCollider.brakeTorque = 0;
            BackLeftWheelCollider.brakeTorque = 0;
            BackRightWheelCollider.brakeTorque = 0;
        }

        activeEffect();

        UpdateWheelPose(FrontLeftWheelCollider, FrontLeftWheelTransform);
        UpdateWheelPose(FrontRightWheelCollider, FrontRightWheelTransform);
        UpdateWheelPose(BackLeftWheelCollider, BackLeftWheelTransform);
        UpdateWheelPose(BackRightWheelCollider, BackRightWheelTransform);
    }


    private void UpdateWheelPose( WheelCollider collider, Transform transform) {
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation;

        collider.GetWorldPose(out pos, out quat);

        transform.position = pos;
        transform.rotation = quat;
    }

    private void activeEffect() {
        var isRotating = FrontRightWheelCollider.rpm + FrontLeftWheelCollider.rpm  > 0;
        bool activeSmokeEffect = (Mathf.Abs(HorizontalInput) > 0.3f || isBraking ) && isRotating;

        if(activeSmokeEffect) {
            SmokeEffectRight.GetComponent<ParticleSystem>().Play();
            SmokeEffectLeft.GetComponent<ParticleSystem>().Play();
        }
    }

}
