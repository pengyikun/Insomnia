using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public bool isPlayerVisible;
    public ChaseState chaseState;
    AIController controller;

    private void Start()
    {
        controller = this.transform.parent.transform.parent.GetComponent<AIController>();
    }
    public override State RunCurrentState()
    {
        controller.EnviromentView();
        if (!controller.m_IsPatrol)
        {
            return chaseState;
        }

        // Patrol
        if (controller.m_PlayerNear)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position
            if (controller.m_TimeToRotate <= 0)
            {
                controller.Move(controller.speedWalk);
                controller.LookingPlayer(controller.playerLastPosition);
            }
            else
            {
                //  The enemy wait for a moment and then go to the last player position
                controller.Stop();
                controller.m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            controller.m_PlayerNear = false;           //  The player is no near when the enemy is platroling
            controller.playerLastPosition = Vector3.zero;
            controller.navMeshAgent.SetDestination(controller.waypoints[controller.m_CurrentWaypointIndex].position);    //  Set the enemy destination to the next waypoint
            if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                if (controller.m_WaitTime <= 0)
                {
                    controller.NextPoint();
                    controller.Move(controller.speedWalk);
                    controller.m_WaitTime = controller.startWaitTime;
                }
                else
                {
                    controller.Stop();
                    controller.m_WaitTime -= Time.deltaTime;
                }
            }
        }

        return this;

        /*        if (isPlayerVisible)
                {
                    return chaseState;
                }
                return this;*/
    }
}
