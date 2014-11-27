using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class State_Stalker_Wander : State_Base
{
    #region PRIVATE VARIABLES

    // The stalker this state refers to and the motor attached 
    Character_Stalker m_stalker = null;
    Motor_Stalker m_stalkerMotor = null;

    // The path this should follow
    List<Vector3> m_wanderPath = null;

    #endregion

    #region PUBLIC FUNCTIONS

    public State_Stalker_Wander(Character_Stalker stalker)
    {
        m_stalker = stalker;
        m_stalkerMotor = m_stalker.GetComponent<Motor_Stalker>();
        m_wanderPath = new List<Vector3>();
    }

    public override void EnterState()
    {
        //m_wanderPath.Add(GameObject.FindGameObjectWithTag("Waypoint").transform.position);
    }

    public override void UpdateState()
    {
        if (m_stalkerMotor.TargetReached)
        {
            m_stalkerMotor.SetNewTarget(Waypoint.FindNearestWaypoint(m_stalkerMotor.CurrentTarget));
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
        m_wanderPath = null;
    }

    #endregion


    #region PRIVATE FUNCTIONS

    private void FindNextWayPoint()
    {
        //
    }

    #endregion
}