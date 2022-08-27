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
    public Transform Cam;

    public GameObject SmokeEffectLeft;
    public GameObject SmokeEffectRight;

    private Rigidbody rigidbodyTank;

    private bool isBraking = false;

    private float VerticalInput;
    private float HorizontalInput;
    private AudioSource audioSource; 
    private AudioSource audioSourceRuning; 
    private AudioSource audioSourceDrift; 
    private bool enabledControl = false;

    private void Awake() {
        rigidbodyTank = GetComponent<Rigidbody>();
        GameManager.Instance.onStartGame.AddListener(()=> enabledControl = true);
    }
    void Start()
    {
        audioSource = GetComponent<Player>().audioComp;
        InitEffectSound();
    }

    // Update is called once per frame
    void Update()
    {
        if(enabledControl) {
            controlCannon();
            
            if (Input.GetKey(KeyCode.Mouse0)) {
                Transform camT = Cam.GetComponent<CameraFollow>().GetTransformCam();
                Ray ray = new Ray(camT.position, camT.forward);
                GetComponent<Shooting>().PlayerShoot(ray);
            }

            isBraking = Input.GetKey(KeyCode.Space);
        }
        activeSound();
    }

    private void FixedUpdate() {

        if(isBraking) {
            brake(brakeForce);
        } else {
            if(HorizontalInput != 0) {
                turn();
            } else if(VerticalInput != 0) {
                MoveToward();
            }

            if(VerticalInput == 0 && HorizontalInput == 0) {
                brakeFront(800f);
            } else {
                brake(0);
            }
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
        var isRotating = FrontRightWheelCollider.rpm + FrontLeftWheelCollider.rpm > 0;
        bool activeSmokeEffect = (Mathf.Abs(HorizontalInput) > 0.3f || isBraking ) && isRotating;

        if(activeSmokeEffect) {
            if(SmokeEffectRight.GetComponent<ParticleSystem>().isStopped) {
                SmokeEffectRight.GetComponent<ParticleSystem>().Play();
            }
            if(SmokeEffectLeft.GetComponent<ParticleSystem>().isStopped) {
                SmokeEffectLeft.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    private void controlCannon() {
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
    }

    private void brake(float brakeForce) {
        FrontLeftWheelCollider.brakeTorque = brakeForce;
        FrontRightWheelCollider.brakeTorque = brakeForce;
        BackLeftWheelCollider.brakeTorque = brakeForce;
        BackRightWheelCollider.brakeTorque = brakeForce;
    }

    private void brakeFront(float brakeForce) {
        FrontLeftWheelCollider.brakeTorque = brakeForce;
        FrontRightWheelCollider.brakeTorque = brakeForce;
    }

    private void turn() {
        FrontLeftWheelCollider.motorTorque = HorizontalInput * rotateSpeed;
        FrontRightWheelCollider.motorTorque = -HorizontalInput * rotateSpeed;
        BackLeftWheelCollider.motorTorque = HorizontalInput * rotateSpeed;
        BackRightWheelCollider.motorTorque = -HorizontalInput * rotateSpeed;
    }

    private void MoveToward() {
        FrontLeftWheelCollider.motorTorque = VerticalInput * speed;
        FrontRightWheelCollider.motorTorque = VerticalInput * speed;
    }

    private void activeSound() {
        if((Mathf.Abs(VerticalInput)) > 0) {
            audioSource.volume = 0f;
            audioSourceDrift.volume = 0f;
            audioSourceRuning.volume = Mathf.Clamp(Mathf.Abs(VerticalInput) , 0f, 0.6f);
        } else if(Mathf.Abs(HorizontalInput) > 0) {
            audioSource.volume = 0f;
            audioSourceDrift.volume = 0f;
            audioSourceRuning.volume = Mathf.Clamp(Mathf.Abs(HorizontalInput), 0f, 0.5f);
        } else {
            audioSource.volume = 0.8f;
            audioSourceDrift.volume = 0f;
            audioSourceRuning.volume = 0f;
        }

        if( isBraking && FrontRightWheelCollider.rpm + FrontLeftWheelCollider.rpm > 0) {
            audioSource.volume = 0.8f;
            audioSourceRuning.volume = 0f;
            audioSourceDrift.volume = 0.1f;
        }
    }

    private void InitEffectSound() {
        audioSourceRuning = SoundManager.Instance.add3DSound(gameObject,true);
        audioSourceDrift = SoundManager.Instance.add3DSound(gameObject,true);

        audioSourceRuning.clip = SoundManager.Instance.getAudioInResources(Sounds.engineRunning);
        audioSourceDrift.clip = SoundManager.Instance.getAudioInResources(Sounds.TireScreeching);

        audioSourceRuning.volume = 0f;
        audioSourceDrift.volume = 0f;
        audioSourceRuning.playOnAwake = false;
        audioSourceDrift.playOnAwake = false;
        audioSourceRuning.loop = true;
        audioSourceDrift.loop = true;
        audioSourceRuning.Play();
        audioSourceDrift.Play();
    }

}
