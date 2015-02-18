using UnityEngine;
using System.Collections;

public class CharacterBase : MonoBehaviour
{
    #region Protected variables

    //Each character will have a mission manager. The type will vary between player an NPC so it will be null here
    protected MissionManager m_missionManager = null;

    //When the game is paused, OnPause will be called, m_isPaused will be true, and the previous state will be saved
    protected bool m_isPaused = false;
    protected State_Pause m_pauseState = null;
    protected State_Base m_previousState = null;

    //Each character will aslo have a stateMachine, initialized here
    protected StateMachineBase m_stateMachine = new StateMachineBase();

    //The variable to hold a reference to the animator component attached to this character
    protected Animator m_animator = null;

    //The variable to hold a reference to the motor component that will be attached to this character
    protected Motor_Base m_motor = null;

    //Each player will have an overworld state, which will be the primary state for all characters
    protected State_Base m_overWorldState = null;
    protected State_Conversation m_conversationState = null;

    // The health component attached to this character
    protected Health m_health = null;

    #endregion

    #region Protected Methods

    protected virtual void Awake()
    {
        m_conversationState = new State_Conversation(this);

        m_animator = this.gameObject.GetComponent<Animator>();
        m_pauseState = new State_Pause(m_animator);

        // Set up the pause functionality
        AddToGameManagerPauseEvent();
    }

    protected virtual void Start()
    {
        // Does nothing in the base class but may be needed in subclasses
    }

    protected virtual void Update()
    {
        //
    }

    protected virtual void FixedUpdate()
    {
        //
    }

    protected void AddToGameManagerPauseEvent()
    {
        //add the pause and resume functions to the game manager's delegate
        GameManager.OnPause += OnPause;
        GameManager.OnUnPause += OnUnPause;
    }

    //Whenever the game is paused, this function will be called
    protected void OnPause()
    {
        //pause the current Animation
        m_animator.speed = Clock.TimeScale;

        //save the previous state, so the character will return to the state when the game the is resumed
        m_previousState = m_stateMachine.CurrentState;

        //set the current state to the pause state
        m_stateMachine.SetCurrentState(m_pauseState);

        //set the paused boolean to be true
        m_isPaused = true;
    }

    protected void OnUnPause()
    {
        //set the current state to the state that the character was in before the game was paused
        m_stateMachine.SetCurrentState(m_previousState);

        //set the previous state to null
        m_previousState = null;

        //set the paused boolean to be false
        m_isPaused = false;

        //unpause the current Animation
        m_animator.speed = Clock.TimeScale;

        m_motor.UnlockMotion();
    }

    #endregion

    #region PUBLIC FUNCTIONS

    public virtual void OnSpeak()
    {
        m_motor.LockMotion();
        m_stateMachine.SetCurrentState(m_conversationState);
    }

    public virtual void OnEndSpeak()
    {
        m_motor.UnlockMotion();
        m_stateMachine.SetCurrentState(m_overWorldState);
    }

    public virtual void Die()
    {
        m_animator.SetBool("IsAlive", false);
    }

    #endregion

    #region Properties

    public bool IsPaused
    {
        get { return m_isPaused; }
    }

    public Motor_Base Motor
    {
        get { return m_motor; }
        set { m_motor = value; }
    }

    public Animator GetAnimator
    {
        get { return m_animator; }
    }

    #endregion
}
