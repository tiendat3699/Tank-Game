using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask Player;
    public Transform ShootPoint;
    [SerializeField]
    public float SignRange;
    public float AttackRange;
    [Range(1,3)]
    public int accuracy = 2;
    public float limitTimeLooking = 2f;
    private bool PlayerInSignRange, PlayerInAttackRange;
    private NavMeshAgent agent;
    private Vector3 walkPoint;
    private Transform PlayerTransform;
    private bool PlayerCanSee = false;
    private bool DetectPlayer= false;
    private bool getHit = false;
    const string PATROLL = "patroll";
    const string LOOKING = "looking";
    const string CHASE = "chase";
    const string ATTACK = "attack";
    

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        walkPoint = GetRandomWalkPoint();
        PlayerTransform = GameManager.Instance.getPlayer().transform;

    }

    // Update is called once per frame
    void Update()
    {

        getHit = transform.GetComponent<Target>().GetHit();
        DoAction(LOOKING);

        if(!DetectPlayer && !getHit) {
            if(!PlayerInSignRange && !PlayerInAttackRange) {
                DoAction(PATROLL);
            }
        }
        if(DetectPlayer) {
            if(PlayerInSignRange && !PlayerInAttackRange) {
                DoAction(CHASE);
            }
            if(PlayerInAttackRange && PlayerInAttackRange){
                DoAction(ATTACK);
            }
        }
    }

    private void HandlePatrolling() {
        agent.SetDestination(walkPoint);
        if(Vector3.Distance(transform.position,walkPoint) <=2) {
            walkPoint = GetRandomWalkPoint();
        }
    }

    private void HandleLooking() {
        PlayerInSignRange = Physics.CheckSphere(transform.position, SignRange, Player);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, Player);
        
        Vector3 direction  = (PlayerTransform.position - transform.position);
        RaycastHit target;

        if(getHit) {
            agent.SetDestination(PlayerTransform.position);
        }

        if(Physics.Raycast(transform.position, direction, out target, SignRange)) {
            PlayerCanSee = target.transform.gameObject.layer == LayerMask.NameToLayer("Player");
            DetectPlayer = PlayerCanSee;
            StartCoroutine(StartLooking());
        }
    } 

    private void HandleChase() {
        agent.SetDestination(PlayerTransform.position);
    }

    private void HandleAttack() {
        agent.SetDestination(transform.position);
        transform.LookAt(PlayerTransform);
        Ray ray = new Ray(ShootPoint.position, ShootPoint.forward);
        GetComponent<Shooting>().Shoot(ray, false, accuracy);
    }

    private Vector3 GetRandomWalkPoint() {
        List<Transform> ListWalkPoint =  GameManager.Instance.GetListWalkPoint();
        int walkPointInd = UnityEngine.Random.Range(0,ListWalkPoint.Count);
        return ListWalkPoint[walkPointInd].position;
    }


    private void DoAction(string state) {
        switch (state) {
            case PATROLL:
                HandlePatrolling();
                break;
            case LOOKING:
                HandleLooking();
                break;
            case CHASE:
                HandleChase();
                break;
            case ATTACK:
                HandleAttack();
                break;
        }
    }

    IEnumerator StartLooking() {
        yield return new WaitForSeconds(limitTimeLooking);
        if(PlayerCanSee) {
            DoAction(LOOKING);
        } else {
            DetectPlayer = false;
            transform.GetComponent<Target>().ResetHit();
        }
    }
}
