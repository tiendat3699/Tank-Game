using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask Ground, Player;
    public Transform PlayerTransform;
    public Transform ShootPoint;
    public float SignRange;
    public float AttackRange;
    [Range(5, 100)]
    public float WalkPointRange = 10;
    [Range(1,10)]
    public int accuracy = 10;
    private bool PlayerInSignRange, PlayerInAttackRange;

    private NavMeshAgent agent;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool PlayerCanSee = false;
        PlayerInSignRange = Physics.CheckSphere(transform.position, SignRange, Player);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, Player);
        
        Vector3 direction  = (PlayerTransform.position - transform.position);
        RaycastHit target;
        if(Physics.Raycast(transform.position, direction, out target, SignRange)) {
            PlayerCanSee = target.transform.gameObject.layer == LayerMask.NameToLayer("Player");
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
        Vector3 point = GetRandomWalkPoint();
        Vector3 distance = transform.position - point;
        while(distance.magnitude < 1f) {
            point = GetRandomWalkPoint();
            distance = transform.position - point;
        }
        agent.SetDestination(point);
    }
    private void HandleChase() {
        agent.SetDestination(PlayerTransform.position);
    }
    private void HandleAttack() {
        agent.SetDestination(transform.position);
        transform.LookAt(PlayerTransform);
        GetComponent<Shooting>().Shoot(ShootPoint, false, accuracy);
    }

    private Vector3 GetRandomWalkPoint() {
        bool invalidWalkPoint = true;
        Vector3 walkPoint = transform.position;
        while(invalidWalkPoint) {
            float randomZ = Random.Range(-WalkPointRange, WalkPointRange);
            float randomX = Random.Range(-WalkPointRange, WalkPointRange);
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            if(Physics.Raycast(walkPoint, -Vector3.up, 2f, Ground)) {
                invalidWalkPoint = false;
            };
        }
        return walkPoint;
    }
}
