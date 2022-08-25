using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DeadTank;

    [SerializeField]
    protected float health = 50f;
    [SerializeField]
    protected bool immortal = false;
    public ParticleSystem smokeEffect;
    protected GameManager gameManager;
    private float _InitHealth;
    [HideInInspector]
    public AudioSource audioComp;

    private void Awake() {
        gameManager = GameManager.Instance;
        _InitHealth = health;

        InitAudioEngine();
    }

    private void Start() {
        InitAudioEngine();
        InitStart();
    }

    // Update is called once per frame
    void Update()
    {
        activeEffectLowHp();
    }

    public virtual void TakeDamage(float damage, Vector3 vector) {
        if(!immortal) {
            health -= damage;
            if(health<=0) {
                die(vector);
            }
        }
    } 

    protected virtual void die(Vector3 vector) {
        Destroy(gameObject);
        GameObject deadBody = Instantiate(DeadTank, transform.position, transform.rotation);
        deadBody.GetComponent<detroyed>().explode(vector);
    }

    private void activeEffectLowHp() {
        if(health <= _InitHealth/2) {
            if(smokeEffect.isStopped) {
                smokeEffect.Play();
            }
        } else {
            if(smokeEffect.isPlaying) {
                smokeEffect.Stop();
            }
        }
    }

    protected virtual void InitStart() {

    }

    protected virtual void InitAudioEngine() {
        audioComp = SoundManager.Instance.add3DSound(gameObject);
        AudioClip audio = SoundManager.Instance.getAudioInResources(Sounds.engineIdle);
        audioComp.loop = true;
        audioComp.playOnAwake = true;
        audioComp.clip = audio;
        audioComp.Play();
    }
}
