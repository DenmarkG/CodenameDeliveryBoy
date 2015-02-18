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
        // reset the path index
        m_currentPathIndex = 0;
    }

    public override void UpdateState()
    {
        // Draw the path in the editor window
        for (int i = 0; i < m_path.corners.Length - 1; ++i)
        {
            Debug.DrawLine(m_path.corners[i], m_path.corners[i + 1], Color.red);
        }

        // Update the LOS
        m_stalkerMotor.UpdateLOS();

        // Update the position if the player is still in sight and has moved
        if (m_stalkerMotor.CanSeePlayer)
        {
            if (Vector3.Distance(m_stalker.transform.position, m_target.position) < m_stalkerMotor.AttackRange)
            {
                m_stalkerMotor.SetAttacking(true);
                return;
            }
            else
            {
                m_stalkerMotor.SetAttacking(false);
            }

            if (m_target.position != m_goalPosition)
            {
                UpdateGoalPositionAndPath(m_target.position);
                m_stalkerMotor.SetNewTarget(m_path.corners[0]);
            }
        }
        else if (m_stalkerMotor.TargetReached)
        {
            // Continue Along path until goal is reached
            if (m_path.status != NavMeshPathStatus.PathInvalid && m_currentPathIndex < m_path.corners.Length)
            {
                m_stalkerMotor.SetNewTarget(m_path.corners[m_currentPathIndex++]);
            }
            else
            {
                m_stalker.SetWanderState();
            }
        }

        m_stalkerMotor.UpdateMotor();
    }

    public override void UpdateStateFixed()
    {
        //
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
    }

    public void SetTarget(Transform target)
    {
        m_target = target;
        UpdateGoalPositionAndPath(target.position);
    }

    private void UpdateGoalPositionAndPath(Vector3 position)
    {
        m_goalPosition = position;
        m_stalkerAgent.CalculatePath(m_goalPosition, m_path);
        m_currentPathIndex = 0;
    }
}
