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
    private NavMeshPath m_path = null;


    public State_Stalker_Pursue(Character_Stalker stalker)
    {
        m_stalker = stalker;
        m_stalkerMotor = m_stalker.GetComponent<Motor_Stalker>();
        m_stalkerAgent = m_stalker.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        //find the path to the target (a sound/ player/ etc.)
        m_stalkerAgent.CalculatePath(m_goalPosition, m_path);
    }

    public override void UpdateState()
    {
        if (m_stalkerMotor.TargetReached)
        {
            // if the the last point was reached
            if (m_stalkerMotor.CurrentTarget == m_goalPosition)
            {
                // check for the player
                // if player, attack
                // if no player, go back to wandering
            }
            else
            {
                //move to the next point in the path
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
        //
    }

    public void SetGoalPosition(Vector3 target)
    {
        //
    }

    private bool LookForPlayer()
    {
        return false;
    }
}
