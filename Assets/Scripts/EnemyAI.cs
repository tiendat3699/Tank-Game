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
    [SerializeField]
    private Animator runAnimation;
    private bool PlayerInSignRange, PlayerInAttackRange;
    private NavMeshAgent agent;
    private Vector3 walkPoint;
    private Transform PlayerTransform;
    private bool PlayerCanSee = false;
    private bool DetectedPlayer= false;
    private bool getHit = false;
    private Vector3 lastPosition;
    private enum StateType {
        PATROLL,
        LOOKING,
        CHASE,
        ATTACK
    }
    

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        walkPoint = GetRandomWalkPoint();
        PlayerTransform = GameManager.Instance.getPlayer().transform;

        lastPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        activeAnimationRun();

        getHit = transform.GetComponent<Enemy>().GetHit();
        DoAction(StateType.LOOKING);

        if(!DetectedPlayer && !getHit) {
            if(!PlayerInSignRange && !PlayerInAttackRange) {
                DoAction(StateType.PATROLL);
            }
        }
        if(DetectedPlayer) {
            if(PlayerInSignRange && !PlayerInAttackRange) {
                DoAction(StateType.CHASE);
            }
            if(PlayerInSignRange && PlayerInAttackRange){
                DoAction(StateType.ATTACK);
            }
        }
    }

    private void OnDestroy() {
        StopAllCoroutines();
    }

    private void HandlePatrolling() {
        agent.SetDestination(walkPoint);
        if(Vector3.Distance(transform.position,walkPoint) <= 5f) {
            walkPoint = GetRandomWalkPoint();
        }
        Debug.DrawLine(transform.position, walkPoint, Color.red);
    }

    private void HandleLooking() {
        PlayerInSignRange = Physics.CheckSphere(transform.position, SignRange, Player);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, Player);
        
        Vector3 direction  = (PlayerTransform.position - transform.position);
        RaycastHit target;

        if(getHit && !DetectedPlayer) {
            agent.SetDestination(PlayerTransform.position);
        }


        if(Physics.Raycast(transform.position, direction, out target, SignRange)) {
            PlayerCanSee = target.transform.gameObject.layer == LayerMask.NameToLayer("Player");
            DetectedPlayer = PlayerCanSee;
            StartCoroutine(StartLooking());
        }
    } 

    private void HandleChase() {
        agent.SetDestination(PlayerTransform.position);
    }

    private void HandleAttack() {
        agent.SetDestination(transform.position);
        Ray ray = new Ray(ShootPoint.position, ShootPoint.forward);
        Quaternion rot = Quaternion.LookRotation(PlayerTransform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5f* Time.deltaTime);
        GetComponent<Shooting>().EnemyShoot(ray, accuracy);
    }

    private Vector3 GetRandomWalkPoint() {
        List<Transform> ListWalkPoint =  GameManager.Instance.GetListWalkPoint();
        int walkPointInd = UnityEngine.Random.Range(0,(ListWalkPoint.Count));
        return ListWalkPoint[walkPointInd].position;
    }

    private void activeAnimationRun() {

        if(Vector3.Distance(transform.position, lastPosition) > 0.01f) {
            if(Vector3.Dot(transform.forward, transform.position - lastPosition ) >= 0.01f) {
                runAnimation.SetTrigger("Forward");
            } else if(Vector3.Dot(transform.forward, transform.position - lastPosition ) <= -0.01f) {
                runAnimation.SetTrigger("Backward");;
            }
        } else {
            runAnimation.SetTrigger("Stop");
        }
        lastPosition = transform.position;
    }


    private void DoAction(StateType state) {
        switch (state) {
            case StateType.PATROLL:
                HandlePatrolling();
                break;
            case StateType.LOOKING:
                HandleLooking();
                break;
            case StateType.CHASE:
                HandleChase();
                break;
            case StateType.ATTACK:
                HandleAttack();
                break;
        }
    }

    IEnumerator StartLooking() {
        yield return new WaitForSeconds(limitTimeLooking);
        if(PlayerCanSee) {
            DoAction(StateType.LOOKING);
        } else {
            DetectedPlayer = false;
            transform.GetComponent<Enemy>().ResetHit();
        }
    }
}
