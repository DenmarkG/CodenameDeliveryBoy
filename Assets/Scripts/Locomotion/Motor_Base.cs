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
	[SerializeField]
    protected float m_rotationSpeed = 3f;

    // The transform attached to this game object
    protected Transform m_transform = null;

	//variables for setting up the Animator
	protected Animator m_animator = null;
	protected AnimatorStateInfo m_animStateInfo;
	protected AnimatorTransitionInfo m_animTransistionInfo;
	protected AnimatorStateInfo m_stateInfo;

	protected bool m_bLocked = false; //when true the character cannot move

    // Cover taller than .75 will automaticall cause the player to stand, lower will make the player crouch
    // For the enemy this will represent the camera's vertical position
    protected const float EYE_HEIGHT = 1.3f;

	//private variables

	//Callbacks
	protected virtual void Awake()
	{
        m_animator = this.GetComponent<Animator>();
        m_transform = this.transform;
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

	public virtual void LockMotion()
	{
		m_bLocked = true;
	}
	
	public virtual void UnlockMotion()
	{
		m_bLocked = false;
	}

    public float Speed
    {
        get { return m_speed; }
    }

    protected void Rotate(Vector3 targetDir)
    {
        //create a step value to rotate the player over time
        float step = m_rotationSpeed * Clock.DeltaTime;

        //create a rotation with the forward vector the same as the move direction
        Quaternion qTargetDir = Quaternion.LookRotation(targetDir, Vector3.up);

        //set the rotation to the lerp between the current and the desired direction
        transform.rotation = Quaternion.Lerp(this.transform.rotation, qTargetDir, step);
    }
}