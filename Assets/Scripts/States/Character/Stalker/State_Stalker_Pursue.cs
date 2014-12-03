using UnityEngine;
using System.Collections;

public class State_Stalker_Pursue : State_Base
{
    // The stalker this state refers to and the motor attached 
    private Character_Stalker m_stalker = null;
    private Motor_Stalker m_stalkerMotor = null;
    private NavMeshAgent m_stalkerAgent = null;

    // Where this stalker should navigate toward
    private Vector3 m_goalPosition = Vector3.zero;
    private NavMeshPath m_path = new NavMeshPath();
    private int m_currentPathIndex = 0;

    private Transform m_target = null;

    public State_Stalker_Pursue(Character_Stalker stalker)
    {
        m_stalker = stalker;
        m_stalkerMotor = m_stalker.GetComponent<Motor_Stalker>();
        m_stalkerAgent = m_stalker.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        //find the path to the target (a sound/ player/ etc.)
        //m_stalkerAgent.CalculatePath(m_goalPosition, m_path);
        m_currentPathIndex = 0;
    }

    public override void UpdateState()
    {
        if (m_stalkerMotor.TargetReached)
        {
            // Check to see if the player is still visible
            if (m_stalkerMotor.CanSeePlayer)
            {
                // If we are in attack range, attack
                if (Vector3.Distance(m_stalkerMotor.transform.position, m_goalPosition) < m_stalkerMotor.AttackRange)
                {
                    //
                }
                // if the player has moved, update the goal position and calculate path
                if (m_target.position != m_goalPosition)
                {
                    m_goalPosition = m_target.position;
                    m_stalkerAgent.CalculatePath(m_goalPosition, m_path);
                }
            }
            else
            {
                if (m_path.status != NavMeshPathStatus.PathInvalid && m_currentPathIndex < m_path.corners.Length)
                {
                    m_stalkerMotor.SetNewTarget(m_path.corners[m_currentPathIndex++]);
                }
                else // if this is the end of the path, go back to wandering
                {
                    // Return to wander state
                    m_stalker.SetWanderState();

                    // may have to early out here
                    // return;
                }
            }
        }

        m_stalkerMotor.UpdateMotor();
    }

    public override void UpdateStateFixed()
    {
        m_stalkerMotor.UpdateMotorFixed();
    }

    public override void LateUpdateState()
    {
        //
    }

    public override void ExitState()
    {
        m_path.ClearCorners();
        m_currentPathIndex = 0;
        m_target = null;

        Debug.Log("Back to wandering");
    }

    public void SetGoalPosition(Transform target)
    {
        m_target = target;
        m_goalPosition = target.position;
        m_stalkerAgent.CalculatePath(m_goalPosition, m_path);
    }

    private bool LookForPlayer()
    {
        return false;
    }
}
