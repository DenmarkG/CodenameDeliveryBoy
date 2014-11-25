using UnityEngine;
using System.Collections;

public class Motor_Base : MonoBehaviour
{
	//protected variables
	[SerializeField]
	protected const float DEAD_ZONE = .01f;	//in specific cases, values less than this will be ignored

    // holds a reference to the character controller attached to this game object
    protected CharacterController m_charController = null;

	//variables for storing input information
	protected float m_horizontal = 0f;
	protected float m_vertical = 0f;

	//tells whether or not the player is running
	protected bool m_isRunning = false;

	//variables to help with animation blending. These correspond to variables in the Animation Controller 
	protected float m_direction = 0f;
	protected float m_speed = 0f;
	protected const float RUN_SPEED = 2f;
	protected const float RUN_TRANSITION_SPEED = 3f;
	protected float m_angle = 0f;
	protected float m_rotationSpeed = 3f;

	//variables for setting up the Animator
	protected Animator m_animator = null;
	protected AnimatorStateInfo m_animStateInfo;
	protected AnimatorTransitionInfo m_animTransistionInfo;
	protected AnimatorStateInfo m_stateInfo;

	//reference to the game's camera
	protected PlayerCamera m_camera = null;

	protected bool m_bLocked = false; //when true the character cannot move

	//private variables

	//Callbacks
	protected virtual void Awake()
	{
        m_animator = this.GetComponent<Animator>();
	}

	protected virtual void Start()
	{
		m_camera = PlayerCamera.GetCamera;
	}

	//Public Functions
	public virtual void UpdateMotor()
	{
		//
	}

	public virtual void UpdateMotorFixed()
	{
		//
	}

	//Protected Functions

	public virtual void LockMotion()
	{
		m_bLocked = true;
	}
	
	public virtual void UnlockMotion()
	{
		m_bLocked = false;
	}
}