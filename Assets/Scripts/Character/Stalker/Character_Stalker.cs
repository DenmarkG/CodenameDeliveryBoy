using UnityEngine;
using System.Collections;

public class Character_Stalker : CharacterBase
{
    #region PRIVATE VARIABLES

    private State_Stalker_Pursue m_pursuitState = null;

    #endregion

    #region UNITY FUNCTIONS

    protected override void Awake()
    {
        m_motor = this.GetComponent<Motor_Stalker>();
        if (m_motor == null)
        {
            m_motor = this.gameObject.AddComponent<Motor_Stalker>();
        }

        m_animator = this.gameObject.GetComponent<Animator>();
        m_overWorldState = new State_Stalker_Wander(this);
        m_pauseState = new State_Pause(m_animator);
        m_pursuitState = new State_Stalker_Pursue(this);

        AddToGameManagerPauseEvent();
    }

    protected override void Start()
    {
        m_stateMachine.SetCurrentState(m_overWorldState);
    }

    protected override void Update() 
	{
        if (!m_isPaused)
        {
            m_stateMachine.UpdateState();
        }
    }

    protected override void FixedUpdate()
    {
        if (!m_isPaused)
        {
            m_stateMachine.UpdateStateFixed();
        }
    }

    #endregion

    #region PUBLIC FUNCTIONS

    public void BeginLookForTarget(GameObject target)
    {
        //
    }

    public void EndLookForTarget()
    {
        //
    }

    public void SetWanderState()
    {
        m_stateMachine.SetCurrentState(m_overWorldState);
    }

    public void SetPursuitState(Vector3 goalPos)
    {
        m_stateMachine.SetCurrentState(m_pursuitState);
        m_pursuitState.SetGoalPosition(goalPos);
    }

    #endregion

    #region PROTECTED FUNCTIONS

    #endregion

    #region PRIVATE FUNCTIONS

    #endregion

    #region PROPERTIES

    #endregion
}