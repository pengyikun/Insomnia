using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public bool isInAttackRange;
    public AttackState attackState;
    public IdleState idleState;
    AIController controller;

    private void Start()
    {
        controller = this.transform.parent.transform.parent.GetComponent<AIController>();
    }
    public override State RunCurrentState()
    {
        controller.EnviromentView();
        if (controller.m_IsPatrol)
        {
            return idleState;
        }

        //  The enemy is chasing the player
        controller.m_PlayerNear = false;                       //  Set false that hte player is near beacause the enemy already sees the player
        controller.playerLastPosition = Vector3.zero;          //  Reset the player near position

        if (!controller.m_CaughtPlayer)
        {
            controller.Move(controller.speedRun);
            controller.navMeshAgent.SetDestination(controller.m_PlayerPosition);          //  set the destination of the enemy to the player location
        }
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance)    //  Control if the enemy arrive to the player location
        {
            if (controller.m_WaitTime <= 0 && !controller.m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                controller.m_IsPatrol = true;
                controller.m_PlayerNear = false;
                controller.Move(controller.speedWalk);
                controller.m_TimeToRotate = controller.timeToRotate;
                controller.m_WaitTime = controller.startWaitTime;
                controller.navMeshAgent.SetDestination(controller.waypoints[controller.m_CurrentWaypointIndex].position);
                return idleState;
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    //  Wait if the current position is not the player position
                    controller.Stop();
                controller.m_WaitTime -= Time.deltaTime;
            }
        }
        return this;
        /*        if (isInAttackRange)
                {
                    return attackState;
                }
                return this;
            }*/
    }
}