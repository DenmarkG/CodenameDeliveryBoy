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
	private float m_offsetHeight = 1.5f; //How high the camera should be
	[SerializeField]
	[Range(0,10)]
	private float m_smooth = 0.25f; //smoothing value; controls the camera lag
	[SerializeField]
	private float m_snapSpeed = 20f; //how fast the camera should snap into position
	[SerializeField]
	private Transform m_target = null;

	#endregion

	#region Private Variables

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

		//if the right mouse button is pressed, allow the camera to enter the free orbit state
		if (Mathf.Abs(Input.GetAxis("Fire2") ) > DEAD_ZONE && m_stateMachine.CurrentState == m_followState )
			m_stateMachine.SetCurrentState(m_orbitState);
		else if (Mathf.Abs(Input.GetAxis("Fire2") ) < DEAD_ZONE && m_stateMachine.CurrentState == m_orbitState)
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
		Vector3 relPlayerPosition = m_target.position - transform.position;
		
		// Create a rotation based on the relative position of the player being the forward vector.
		Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
		
		// Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
		transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, m_smooth * Time.deltaTime);
	}

	float ClampAngle(float angle, float min, float max)
	{
		while (angle < -360 || angle > 360)
		{
			if (angle > 360)
			{ angle -= 360; }
			if (angle < 360)
			{ angle += 360; }
		}
		
		return Mathf.Clamp(angle, min, max);
	}

	IEnumerator ResetCamera()
	{
		Vector3 lookDir = m_target.forward;
		Vector3 relativePos = m_transform.position + (-lookDir * m_distanceAway); 

		while (Vector3.Distance(m_transform.position, relativePos) > DEAD_ZONE)
		{
			relativePos = m_target.position + ( (m_target.up * m_offsetHeight) + (-lookDir * m_distanceAway) );
			m_transform.position = Vector3.Lerp(m_transform.position, relativePos, m_snapSpeed * Time.deltaTime);
			SmoothLookAt();
			yield return null;
		}
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
