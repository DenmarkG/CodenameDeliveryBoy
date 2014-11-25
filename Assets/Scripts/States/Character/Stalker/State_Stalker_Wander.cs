using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class State_Stalker_Wander : State_Base
{
    #region PRIVATE VARIABLES

    // The stalker this state refers to 
    Character_Stalker m_stalker = null;

    List<Vector3> m_wanderPath = null;

    #endregion

    #region PUBLIC FUNCTIONS

    public State_Stalker_Wander(Character_Stalker stalker)
    {
        m_stalker = stalker;
        m_wanderPath = new List<Vector3>();
    }

    public override void EnterState()
    {
        //PerformAStarSearch(ref m_wanderPath);
        m_wanderPath.Add(GameObject.FindGameObjectWithTag("Waypoint").transform.position);
    }

    public override void UpdateState()
    {
        //
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

    private void PerformAStarSearch(ref List<Vector3> path)
    {
        List<Vector3> closedSet = new List<Vector3>();
        Queue<Vector3> openSet = new Queue<Vector3>();

        // Enqueue the start position (the current position)
        openSet.Enqueue(m_stalker.transform.position);

        //
    }

    #endregion
}