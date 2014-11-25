using UnityEngine;
using System.Collections;

public class Character_Stalker : CharacterBase
{
    #region PRIVATE VARIABLES

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

        AddToGameManagerPauseEvent();
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

    #endregion

    #region PROTECTED FUNCTIONS

    #endregion

    #region PRIVATE FUNCTIONS

    #endregion

    #region PROPERTIES

    #endregion
}