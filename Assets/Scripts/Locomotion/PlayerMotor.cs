using UnityEngine;
using System.Collections;


public class PlayerMotor : MonoBehaviour 
{
	#region Serialized Private Variables
	[SerializeField]
	[Range(0,1)]
	private float m_dirDamp = 0.25f;
	[SerializeField]
	[Range(0,1)]
	private float m_speedDamp = 0.25f;
	[SerializeField]
	private float m_rotSpeed = 1.5f;
	[SerializeField]
	private float m_degreesPerSec = 120f;

	#endregion

	#region Private Variables

	private PlayerCamera m_gameCamera = null;
	private float m_direction = 0f;
	private float m_speed = 0f;
	private float m_horizontal = 0f;
	private float m_vertical = 0f;
	private bool m_isRunning = false;
	private const float DEAD_ZONE = .01f;
	
	private Animator m_animator; 
	private int m_locomotionId = 0;
	private int m_locomotionPivot_R = 0;
	private int m_locomotionPivot_L = 0;
	private AnimatorStateInfo m_stateInfo;

	#endregion

	#region Methods

	void Start()
	{
		//set up the animator
		SetUpAnimator();

		if(m_gameCamera == null)
		{
			m_gameCamera = PlayerCamera.PlayerCam;
		}
	}

	public void UpdateMotor() 
	{
		//get the state info for the current state
		m_stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

		//create a variable for the character's angle
		float angle = 0f;

		//reset the direction
		m_direction = 0f;

		//Get the input from the player
		m_horizontal = Input.GetAxis(GameControllerHash.LeftStick.HORIZONTAL);
		m_vertical = Input.GetAxis(GameControllerHash.LeftStick.VERTICAL);

		//check to see if the player is running
		m_isRunning = Input.GetButton(GameControllerHash.Buttons.B);

		//Translate the input to "world space"
		InputToWorldSpace(this.transform, m_gameCamera.transform, ref m_direction, ref m_speed, ref angle, m_isRunning);

		//set the values for mechanim
		m_animator.SetFloat("Speed", m_speed, m_speedDamp, Time.deltaTime);
		m_animator.SetFloat("Direction", m_horizontal, m_dirDamp, Time.deltaTime);

		Debug.Log("Angle: " + angle);
		Debug.Log("Direction: " + m_direction);

//		if (m_speed > DEAD_ZONE) 
//		{
//			if(!IsPivoting() )
//				m_animator.SetFloat("Angle", angle);
//		}
//		else //if( m_speed < DEAD_ZONE && Mathf.Abs(m_horizontal) < DEAD_ZONE)
//		{
//			//m_animator.SetFloat("Direction", 0f);
//			m_animator.SetFloat("Angle", 0f);
//		}

		if(m_speed > DEAD_ZONE)
		{
			if(!IsPivoting())
			{
				m_animator.SetFloat("Angle", angle);
			}
		}
		if(m_speed < DEAD_ZONE || Mathf.Abs(m_horizontal) < DEAD_ZONE)
		{
			m_animator.SetFloat("Angle", 0f);
			m_animator.SetFloat("Direction", 0f);
		}
	}

	public void UpdateMotorFixed()
	{
		if(IsInLocomotion() && ( (m_direction >= 0 && m_speed >= 0) ||  (m_direction < 0 && m_speed < 0) ) && !IsPivoting() )
		{
			Vector3 toRot = new Vector3(0, m_degreesPerSec * (m_horizontal < 0f ? -1f: 1f), 0f);
			Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, toRot, Mathf.Abs(m_horizontal) ); 
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
			this.transform.rotation = this.transform.rotation * deltaRotation;
		}
	}

	void InputToWorldSpace(Transform player, Transform camera, ref float directionOut, ref float speedOut, ref float angleOut, bool isRunning)
	{
		//the player is essentially the actual direction of the character
		Vector3 playerDiretion = player.forward;
		
		Vector3 axisDirection = new Vector3(m_horizontal, 0, m_vertical);
		
		if (isRunning)
			speedOut = (axisDirection.magnitude) * 2;
		else
			speedOut = axisDirection.magnitude;
		
		//Cache the camera direction
		Vector3 camDir = camera.forward;
		camDir.y = 0f;
		camDir.Normalize();
		
		//cameraDirection.y = 0f;
		Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, camDir);
		
		//Convert input in World Space coords
		Vector3 moveDirection = rotation * axisDirection;
		//		Vector3 axisSign = Vector3.Cross(rotation * axisDirection, playerDiretion);
		float axisSign = Vector3.Cross(moveDirection, playerDiretion).y >= 0 ? 1f : -1f;
		
		Debug.DrawRay(new Vector3(player.position.x, player.position.y + 2f, player.position.z), moveDirection, Color.green);
		Debug.DrawRay(new Vector3(player.position.x, player.position.y + 2f, player.position.z), playerDiretion, Color.yellow);
		Debug.DrawRay(new Vector3(player.position.x, player.position.y + 2f, player.position.z), axisDirection, Color.blue);
		Debug.DrawRay(new Vector3(player.position.x, player.position.y + 2f, player.position.z), Vector3.Cross(moveDirection, playerDiretion), Color.red);
		
		//float angleToMove = Vector3.Angle(playerDiretion, rotation.eulerAngles) * (axisSign.y >= 0 ? -1f : 1f);
		float angleToMove = Vector3.Angle(camDir - playerDiretion, moveDirection) * axisSign;
		//Debug.Log("Angle To Move: " + angleToMove);


		if(!IsPivoting() )
			angleOut = angleToMove;
		
		angleToMove /= 90f;
		
		directionOut = (angleToMove * m_rotSpeed);
	}

	void SetUpAnimator()
	{
		m_animator = this.GetComponent<Animator>();
		m_locomotionId = Animator.StringToHash("Base Layer.Locomotion");
		m_locomotionPivot_R = Animator.StringToHash ("Base Layer.LocomotionPivot_R");
		m_locomotionPivot_R = Animator.StringToHash ("Base Layer.LocomotionPivot_R");
	}

	private bool IsInLocomotion()
	{
		return m_stateInfo.nameHash == m_locomotionId;
	}

	private bool IsPivoting()
	{
		return m_stateInfo.nameHash == m_locomotionPivot_L || m_stateInfo.nameHash == m_locomotionPivot_R;
	}

	#endregion
}
