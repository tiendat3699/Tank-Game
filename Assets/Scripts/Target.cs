using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DeadTank;
    public float health = 50f;
    public int Score = 100;
    [SerializeField]
    private bool immortal = false;
    public ParticleSystem smokeEffect;
    private GameManager gameManager;
    private float _InitHealth;
    private bool _getHit = false;

    private void Awake() {
        _InitHealth = health;
    }
    void Start()
    {
        gameManager = GameManager.Instance;
        setHealthPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        activeEffectLowHp();
    }

    public void TakeDamage(float damage, Vector3 vector) {
        if(!immortal) {
            health -= damage;
            setHealthPlayer();
            if(health<=0) {
                die(vector);
                gameManager.setScore(Score, gameObject);
            }
        }
        _getHit = true;
    } 

    private void die(Vector3 vector) {
        Destroy(gameObject);
        GameObject deadBody = Instantiate(DeadTank, transform.position, transform.rotation);
        deadBody.GetComponent<detroyed>().explode(vector);
    }

    private void setHealthPlayer() {
        if(gameObject.tag == "Player") {
            gameManager.setHealth(health);
        }
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

    public bool GetHit() {
        return _getHit;
    }

    public void ResetHit() {
        _getHit = false;
    }
}
