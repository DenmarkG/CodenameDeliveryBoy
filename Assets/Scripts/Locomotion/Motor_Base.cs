using UnityEngine;
using System.Collections;

public class Motor_Base : MonoBehaviour
{
	//protected variables
	[SerializeField]
	protected const float DEAD_ZONE = .01f;	//in specific cases, values less than this will be ignored

	//variables for storing input information
	protected float m_horizontal = 0f;
	protected float m_vertical = 0f;

	//tells whether or not the player is running
	protected bool m_isRunning = false;

	//variables to help with animation blending. These correspond to variables in the Animation Controller 
	protected float m_direction = 0f;
	protected float m_speed = 0f;
	protected const float RUN_SPEED = 2f;
	protected const float RUN_TRANSITION_SPEED = 2f;
	protected float m_angle = 0f;

	//variables for setting up the Animator
	protected Animator m_animator = null;
	protected AnimatorStateInfo m_animStateInfo;
	protected AnimatorTransitionInfo m_animTransistionInfo;
	protected int m_locomotionId = 0;
	protected int m_locomotionPivot_R = 0;
	protected int m_locomotionPivot_L = 0;
	protected AnimatorStateInfo m_stateInfo;

	//reference to the game's camera
	protected PlayerCamera m_camera = null;

	//private variables

	//Callbacks
	protected virtual void Awake()
	{
		SetUpAnimator();
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

	//Private Functions
	private void SetUpAnimator()
	{
		///get the animator component attached to this object
		m_animator = this.GetComponent<Animator>();

		///has the id's for fast access later
		m_locomotionId = Animator.StringToHash("Base Layer.Locomotion");
		m_locomotionPivot_R = Animator.StringToHash ("Base Layer.LocomotionPivot_R");
		m_locomotionPivot_R = Animator.StringToHash ("Base Layer.LocomotionPivot_R");
	}
}