using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using System;
using UnityChan;
using static Cinemachine.CinemachineTargetGroup;

public abstract class AgentAttack : MonoBehaviour
{
    [SerializeField] protected bool isPlayer = false;

    public Action Finish = ()=> { };

    abstract public void BeginAttack();

    protected bool TryDamage(GameObject target, int damage)
    {

        if (isPlayer)
        {
            AgentMovement agentMovement;
            if (target.TryGetComponent<AgentMovement>(out agentMovement))
            {
                if (agentMovement.state != AgentState.Death)
                {
                    agentMovement.Damage(damage);
                    return true;
                }
            }
        }
        else
        {
            UnityChanControlScriptWithRgidBody controller;
            if (target.TryGetComponent<UnityChanControlScriptWithRgidBody>(out controller))
            {
                controller.Damage(damage);
                return true;
            }
        }
        return false;
    }


    protected void DoTarget( Vector3 position, float radius )
    {
        foreach(var hit in Physics.SphereCastAll(position, radius, Vector3.forward,0))
        {
            AgentMovement agentMovement;
            if (hit.collider.gameObject.TryGetComponent<AgentMovement>(out agentMovement))
            {
                agentMovement.DoTarget();
            }
        }

    }
}
