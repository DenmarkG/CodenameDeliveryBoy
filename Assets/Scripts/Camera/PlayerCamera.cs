using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{
	#region Private Serialized Variables

	//tuneable private varibles for how the camera should move
	//and where it should be relative to the player
	[SerializeField]
	private float m_distanceAway = 3f; //How far away the camera should be
	[SerializeField]
	private float m_maxDistanceAway = 10f; //The max distance away from the player the camera can zoom
	[SerializeField]
	private float m_minDistanceAway = 1.5f; //The closest distance the camera can be to the player
	[SerializeField]
	private float m_zoomSpeed = 5f; //How fast the camera can zoom
	[SerializeField]
	private float m_offsetHeight = 1.5f; //How high the camera should be
	[SerializeField]
	[Range(0,50)]
	private float m_smooth = 10f; //smoothing value; controls the camera lag
	[SerializeField]
	private float m_snapSpeed = 20f; //how fast the camera should snap into position
	[SerializeField]
	private Transform m_target = null;
	[SerializeField]
	private float m_orbitSpeed = .01f;
	
	#endregion

	#region Private Variables
	//Storing the start values for resetting the camera
	private float m_startDistanceAway = 0f;
	private float m_startOffsetHeight = 0f;
	private float m_startSmoothSpeed = 0f;

	//State machine variables
    private StateMachineBase m_stateMachine = null;
    private State_Camera_Follow m_followState = null;
    private State_Camera_Orbit m_orbitState = null;

	//Variables for storing the look direction, the player position, and this transform
	private Vector3 m_targetPos = Vector3.zero;
	private Vector3 m_lookDir = Vector3.zero;
	private Transform m_transform = null;

	//variable for the singleton instance
	private static PlayerCamera m_instance = null;
	//private Vector3 m_desiredPosition = Vector3.zero;

	//constants
	private const float DEAD_ZONE = .1f;
	private const float MOVEMENT_STOP_SPEED = 3f; // how fast the camera's movement comes to a stop when an axis is zero

	#endregion

	void Awake()
	{
		//if the static instance of this camera is null, assign it
		if (m_instance == null)
			m_instance = this;

		//assign the states that this camera object will use
        m_stateMachine = new StateMachineBase();
        m_followState = new State_Camera_Follow(this);
		m_orbitState = new State_Camera_Orbit(this);

		//store the initial values for resetting the camera's position later. 
		SetDefaultValues();
	}

	void Start () 
	{
		//cache the transform component of this object
		m_transform = this.transform;

		//set the target of the camera to look at the camera target object's transform
		m_target = GameObject.FindGameObjectWithTag("PlayerCameraTarget").transform;

		//set the current state as the follow state
		m_stateMachine.SetCurrentState(m_followState);
	}

	void Update()
	{
		//reset the camera if the reset button is pressed
		if(  (Input.GetAxis("LEFT_TRIGGER") < -DEAD_ZONE || Input.GetKeyDown(KeyCode.L) ) && !IsInvoking("ResetCamera") )
			StartCoroutine("ResetCamera");

		//if the right mouse button is pressed or the right stick is moved, allow the camera to enter the free orbit state
		/*bool orbiting = Mathf.Abs(Input.GetAxis(GameControllerHash.RightStick.HORIZONTAL) ) > DEAD_ZONE || 
						Mathf.Abs(Input.GetAxis(GameControllerHash.RightStick.VERTICAL) ) > DEAD_ZONE;*/

		if ( (Input.GetKeyDown(KeyCode.F) /*|| orbiting == true*/) && m_stateMachine.CurrentState == m_followState )
			m_stateMachine.SetCurrentState(m_orbitState);
		else if ( (Input.GetKeyUp(KeyCode.F) /*|| orbiting == false*/) && m_stateMachine.CurrentState == m_orbitState)
			m_stateMachine.SetCurrentState(m_followState);

		//update the state machine's current state
		m_stateMachine.UpdateState();
	}

	void LateUpdate () 
	{
		//update the state machine
		m_stateMachine.LateUpdateState();
	}

	#region Custom Methods
	
	public void SetOrbitState()
	{
		//set the current state to the Orbit State
		m_stateMachine.SetCurrentState(m_orbitState);
	}
	
	public void SetFollowState()
	{
		//set the current state to the follow state
		m_stateMachine.SetCurrentState(m_followState);
	}
	
	public void SmoothLookAt()
	{
		// Create a vector from the camera towards the player.
		Vector3 relativePlayerPosition = m_target.position - transform.position;
		
		// Create a rotation based on the relative position of the player being the forward vector.
		Quaternion lookAtRotation = Quaternion.LookRotation(relativePlayerPosition);
		
		// Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
		transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, m_smooth * Clock.DeltaTime);
	}

	public float ClampAngle (float angle, float min, float max)
	{
		if(max > min)
		{
			if (angle > max)
				angle = max;

			if (angle < min)
				angle = min;
		}

		return angle;
	}

	IEnumerator ResetCamera()
	{
		RestoreDefaults();

		//calculate the desired relative position
		Vector3 relativePos = m_transform.position + (m_target.forward * -m_distanceAway);
		bool bHasReset = false;
		while (!bHasReset)
		{
			//lerp the distance toward the start distance
			m_distanceAway = Mathf.Lerp(m_distanceAway, m_distanceAway, m_snapSpeed * Clock.DeltaTime);

			//recalculate the desired position based on the new distance away
			relativePos = (m_target.position + (m_target.up * m_offsetHeight) + (m_target.forward * -m_distanceAway) );

			//move the camera closer to the default position
			m_transform.position = Vector3.Slerp(m_transform.position, relativePos, m_snapSpeed * Clock.DeltaTime);

			bHasReset = Vector3.Distance(m_transform.position, relativePos) <= DEAD_ZONE;

			//return null to prevent the game from hanging on this loop
			yield return null;
		}

		//set the position to the desired position since it's close enough not to jump
		m_transform.position = relativePos;

		//smoothly look at the desired target
		SmoothLookAt();
	}

	void SetDefaultValues()
	{
		//store the default values
		m_startDistanceAway = m_distanceAway;
		m_startOffsetHeight = m_offsetHeight;
		m_startSmoothSpeed = m_smooth;
	}

	void RestoreDefaults()
	{
		//reset any values to their original values
		m_distanceAway = m_startDistanceAway;
		m_offsetHeight = m_startOffsetHeight;
		m_smooth = m_startSmoothSpeed;
	}

	#endregion

	#region Properties

	public static PlayerCamera GetCamera
	{
		get { return m_instance; }
	}

	public float SmoothSpeed
	{
		get { return m_smooth; }
	}

	public float ZoomSpeed
	{
		get { return m_zoomSpeed; }
	}

	public float OrbitSpeed
	{
		get { return m_orbitSpeed; }
	}

	public float MoveStopSpeed
	{
		get { return MOVEMENT_STOP_SPEED; }
	}

	public float DeadZone
	{
		get { return DEAD_ZONE; }
	}

	public Transform CameraTarget
	{
		get { return m_target; }
	}

	public float OffsetHeight
	{
		get { return m_offsetHeight; }
	}

	public float DistanceAway
	{
		get { return m_distanceAway; }

		set 
		{
			if(value > m_maxDistanceAway)
				value = m_maxDistanceAway;
			else if (value < m_minDistanceAway)
				value = m_minDistanceAway;

			m_distanceAway = value;
		}
	}

	public Vector3 TargetPos
	{
		get { return m_targetPos; }
		set { m_targetPos = value; }
	}

	public Vector3 LookDirection
	{
		get { return m_lookDir; }
		set { m_lookDir = value; }
	}

	#endregion
}
