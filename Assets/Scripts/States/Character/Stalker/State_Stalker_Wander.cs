using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class State_Stalker_Wander : State_Base
{
    #region PRIVATE VARIABLES

    // The stalker this state refers to and the motor attached 
    private Character_Stalker m_stalker = null;
    private Motor_Stalker m_stalkerMotor = null;
    private NavMeshAgent m_stalkerAgent = null;

    // The current active points
    private Navigation.ActivePoints m_activePoints;
    private NavMeshPath m_path = new NavMeshPath();
    private int m_currentPathIndex = 0;

    #endregion

    #region PUBLIC FUNCTIONS

    // Constructor simply assigns references to the stalker passed in 
    public State_Stalker_Wander(Character_Stalker stalker)
    {
        m_stalker = stalker;
        m_stalkerMotor = m_stalker.GetComponent<Motor_Stalker>();
        m_stalkerAgent = m_stalker.GetComponent<NavMeshAgent>();
        m_activePoints = new Navigation.ActivePoints();
    }

    // Upon Entering the state, the stalker should find the best path to the next waypoint
    public override void EnterState()
    {
        m_activePoints.m_currentWaypoint = m_stalker.transform.position;

        Vector3 goal = Navigation.FindNearestWaypoint(ref m_activePoints);
        m_stalkerAgent.CalculatePath(goal, m_path);
    }

    public override void UpdateState()
    {
        if (m_stalkerMotor.TargetReached)
        {
            //Debug.Log("Target Reached");
            if (m_path.status != NavMeshPathStatus.PathInvalid && m_currentPathIndex < m_path.corners.Length)
            {
                m_stalkerMotor.SetNewTarget(m_path.corners[m_currentPathIndex++]);
            }
            else
            {
                // clear the current path
                m_path.ClearCorners();
                m_currentPathIndex = 0;

                // Find the next waypoint and find a path to it
                m_stalkerAgent.CalculatePath(Navigation.FindNearestWaypoint(ref m_activePoints), m_path);
            }
        }

        m_stalkerMotor.UpdateMotor();
    }

    public override void UpdateStateFixed()
    {
        if (m_stalkerMotor.ShouldCheckForNearbyObjects)
        {
            m_stalkerMotor.UpdateMotorFixed();
        }
    }

    public override void LateUpdateState()
    {
        //
    }

    public override void ExitState()
    {
        //
    }

    #endregion
}