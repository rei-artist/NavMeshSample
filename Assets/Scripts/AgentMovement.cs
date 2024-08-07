using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System;
using System.Collections;

public enum AgentState
{
    Idle, Patroll, Chase, Attack, Death
}


public class AgentMovement : MonoBehaviour
{
    [SerializeField] private int hp = 1;
    [field: SerializeField] public List<Transform> wayPoints { get; set; }
    [SerializeField] private int wayPointIndex;
    [SerializeField] private Transform target;
    [SerializeField] private float attackDistance = 10f;
    [SerializeField] private float viewDistance = 50f;
    [SerializeField] private float viewAngle = 180f;
    [SerializeField] private float idleTime = 3.0f;
    [SerializeField] private float patrollSpeed = 3.0f;
    [SerializeField] private float chaseSpeed = 5.0f;
    [SerializeField] public AgentState state { get; private set; } = AgentState.Patroll;
    [SerializeField] private AgentAttack agentAttack = null;

    private NavMeshAgent agent;
    private Animator anim;
    private Rigidbody rb;

    public void Damage(int n)
    {
        hp = Math.Max(0,hp-n);
        anim.SetTrigger(hp > 0 ? "GetHit" : "Dead");
    }

    public void DoTarget()
    {
        if (state == AgentState.Patroll || state == AgentState.Idle)
        {
            Vector3 pos = target.position;
            pos.y = transform.position.y;
            transform.LookAt(pos);
            ChangeTo(AgentState.Chase);
        }
    }

    void Start()
    {
        wayPointIndex = 0;
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(wayPoints[wayPointIndex].position);
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (agentAttack == null) agentAttack = GetComponent<AgentAttack>();
        agentAttack.Finish += EndAttack;
        ChangeTo(AgentState.Patroll);
    }

    void Update()
    {
        if (hp == 0 && state != AgentState.Death)
        {  
            ChangeTo(AgentState.Death);
            return;
        }
        switch (state)
        {
            case AgentState.Patroll:
                UpdatePatroll();
                break;
            case AgentState.Chase:
                UpdateChase();
                break;
        }
        if (agent.enabled)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
            anim.SetFloat("Direction", 0);
        }
    }

/*    private void FixedUpdate()
    {
        float h = terrain.SampleHeight(transform.position);
        if (transform.position.y < h)
        {
            Debug.Log(gameObject.name+ " SetY:" + transform.position.y+" "+h);
            Vector3 pos = transform.position;
            pos.y = h;
            transform.position = pos;
        }
    }
*/
    public void ChangeTo(AgentState newState)
    {
        Debug.Log(gameObject.name+" ChangeTo:"+newState+" from:"+ state);
        if (state == newState) return;
        state = newState;
        switch (newState)
        {
            case AgentState.Idle:
                BeginIdle();
                break;
            case AgentState.Patroll:
                BeginPatroll();
                break;
            case AgentState.Chase:
                BeginChase();
                break;
            case AgentState.Attack:
                BeginAttack();
                break;
            case AgentState.Death:
                BeginDeath();
                break;
        }
    }

    private void BeginIdle()
    {
        StartCoroutine(Idle());
    }

    IEnumerator Idle()
    {
        float time = Time.time + idleTime;
        while(Time.time < time)
        {
            if (IsTargetInFieldOfView())
            {
                ChangeTo(AgentState.Chase);
            }
            if(state != AgentState.Idle) yield break;
            yield return null;    
        }
        ChangeTo(AgentState.Patroll);
    }

    private void BeginPatroll()
    {
        agent.speed = patrollSpeed;
        agent.SetDestination(wayPoints[wayPointIndex].position);
    }

    private void UpdatePatroll()
    {
        if (IsTargetInFieldOfView())
        {
            ChangeTo(AgentState.Chase);
            return;
        }
        if (agent.remainingDistance < 3f)
        {
            wayPointIndex = (wayPointIndex + 1) % wayPoints.Count;
            ChangeTo(AgentState.Idle);
            return;
        }
    }

    private void BeginChase()
    {
        agent.SetDestination(target.position);
        agent.speed = chaseSpeed;
    }

    private void UpdateChase()
    {
        if(IsTargetInAttackZone())
        {
            ChangeTo(AgentState.Attack);
            return;
        }
        if (!IsTargetInFieldOfView())
        {
            ChangeTo(AgentState.Patroll);
            return;
        }
        agent.SetDestination(target.position);
    }

    private void BeginAttack()
    {
        agent.enabled = false;
        rb.isKinematic = false;

        Vector3 pos = target.position;
        pos.y = transform.position.y;
        transform.LookAt(pos);

        agentAttack.BeginAttack();
    }

    private void EndAttack()
    {
        if (state != AgentState.Death)
        {
            rb.isKinematic = true;
            agent.enabled = true;
            ChangeTo(AgentState.Chase);
        }
    }

    private void BeginDeath()
    {
        tag = "Untagged";
        agent.enabled = false;
        rb.isKinematic = false;
        rb.constraints = 0;
        rb.mass = 50f;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        var collider = gameObject.AddComponent<BoxCollider>();
        collider.center = Vector3.up * 0.1f;
        Vector3 size = collider.size;
        size.y = 0.2f;
        collider.size = size;
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    public bool IsTargetInAttackZone()
    {
        return Vector3.Distance(target.position, transform.position) < attackDistance;
    }

    private bool IsTargetInFieldOfView()
    {
        Vector3 pos = transform.position + Vector3.up;
        Vector3 targetPos = target.position + Vector3.up;
        Vector3 directionToTarget = targetPos - pos;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        if (angleToTarget <= viewAngle / 2 && directionToTarget.magnitude <= viewDistance)
        {
            if (Physics.Raycast(pos, directionToTarget, out RaycastHit hit, viewDistance))
            {
                if (hit.transform == target)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public Transform GetTarget() { return target; }

}
