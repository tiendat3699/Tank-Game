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
    public float limitTimeChase = 2f;
    private bool PlayerInSignRange, PlayerInAttackRange;
    private NavMeshAgent agent;
    private float timeChase;
    private Vector3 walkPoint;
    private Transform PlayerTransform;

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
        timeChase = timeChase<=limitTimeChase?timeChase+Time.time:0;
        bool PlayerCanSee = false;
        PlayerInSignRange = Physics.CheckSphere(transform.position, SignRange, Player);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, Player);
        
        Vector3 direction  = (PlayerTransform.position - transform.position);
        RaycastHit target;
        if(Physics.Raycast(transform.position, direction, out target, SignRange)) {
            PlayerCanSee = target.transform.gameObject.layer == LayerMask.NameToLayer("Player") && timeChase <= limitTimeChase;
        }
        

        if(!PlayerInSignRange && !PlayerInAttackRange || !PlayerCanSee) {
            HandlePatrolling();
        }
        if(PlayerCanSee) {
            if(PlayerInSignRange && !PlayerInAttackRange) {
                HandleChase();
            }
            if(PlayerInAttackRange && PlayerInAttackRange){
                HandleAttack();
            }
        }
    }

    private void HandlePatrolling() {
        agent.SetDestination(walkPoint);
        if(Vector3.Distance(transform.position,walkPoint) <=2) {
            walkPoint = GetRandomWalkPoint();
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
        int walkPointInd = Random.Range(0,ListWalkPoint.Count);
        return ListWalkPoint[walkPointInd].position;
    }
}
