using UnityEngine;
using System.Collections;

public class Motor_Player : Motor_Base
{
	protected override void Awake ()
	{
		base.Awake ();
	}

	public override void UpdateMotor ()
	{
		if (!m_bLocked)
		{
			//reset the angle to zero
			m_angle = 0f;

			//get the info for the current animation states
			m_animStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
	//		m_animTransistionInfo = m_animator.GetAnimatorTransitionInfo(0);

			//get the state info for the current state
			m_stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

			//Get the input from the player
			m_horizontal = Input.GetAxis(GameControllerHash.LeftStick.HORIZONTAL);
			m_vertical = Input.GetAxis(GameControllerHash.LeftStick.VERTICAL);

			//check to see if the player is running
			m_isRunning = Input.GetButton(GameControllerHash.Buttons.B) || Input.GetKey(KeyCode.LeftShift);

			//now convert the movement to world space
			ConvertInputToWorldSpace();

			//set the values for the animator
			m_animator.SetFloat("Speed", m_speed);
			m_animator.SetFloat("Direction", m_direction);

			if (m_speed > DEAD_ZONE && !IsPivoting)
			{
//				m_animator.SetFloat("Angle", m_angle);
			}
			//[#todo]check to see if an elseif can be made instead later
			if (m_speed < DEAD_ZONE && Mathf.Abs(m_horizontal) < DEAD_ZONE)
			{
				m_animator.SetFloat("Speed", 0);
				m_animator.SetFloat("Angle", 0);
			}

			//[#todo] implement a pause method that utilizes this method
			m_animator.speed = Clock.TimeScale;
		}
	}

	public override void LockMotion()
	{
		base.LockMotion();
		m_animator.SetFloat("Speed", 0);
		m_animator.SetFloat("Direction", 0);
		m_animator.SetFloat("Angle", 0);
	}
	
	public override void UnlockMotion()
	{
		base.UnlockMotion();
	}

	private void ConvertInputToWorldSpace()
	{
		//**************************************************************************************************
		//This function is currently a work in progress
		//**************************************************************************************************

		//Remap the input from the controller to world space
		Transform cameraTransform = m_camera.transform;
		Vector3 moveVector = Vector3.zero;

		moveVector.x = (m_horizontal * cameraTransform.right.x) + (m_vertical * cameraTransform.forward.x);
		moveVector.z = (m_horizontal * cameraTransform.right.z) + (m_vertical * cameraTransform.forward.z);

		if (moveVector.magnitude > 1)
			moveVector.Normalize();

		//cache the forward vector of the player and the camera
		//also remove the y values for each, since we only want to calculate in 2 dimensions
		Vector3 playerDirection = this.transform.forward;
		playerDirection.y = 0;
		Vector3 cameraDiretion = m_camera.transform.forward;
		cameraDiretion.y = 0;
		cameraDiretion.Normalize(); //normalize the camera vector to keep length consistent

		//if the player is running set the speed to 2. Otherwise set it to the length of the input vector

		if (m_isRunning && m_speed > DEAD_ZONE)
			m_speed = Mathf.Lerp(m_speed, RUN_SPEED, Clock.DeltaTime * RUN_TRANSITION_SPEED);
		else
			m_speed = Mathf.Lerp(m_speed, moveVector.magnitude, Clock.DeltaTime * RUN_TRANSITION_SPEED);

		//get the angle between the camera's forward vector and the movement vector
		m_angle = Vector3.Angle(cameraDiretion, moveVector) * (m_horizontal >= 0 ? 1 : -1);

		m_direction = m_angle / 90f;
//		Debug.Log("Angle: " + m_angle);
//		Debug.Log("Direction: " + m_direction);

		if (moveVector.magnitude > DEAD_ZONE)
			Rotate(moveVector);

		//Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), inputAxisDirection, Color.green);
		Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), cameraDiretion, Color.blue);
		Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), moveVector, Color.red);
		Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), playerDirection, Color.red);
	}

	void Rotate(Vector3 targetDir)
	{
		//create a step value to rotate the player over time
		float step = m_rotationSpeed * Clock.DeltaTime;

		//create a rotation with the forward vector the same as the move direction
		Quaternion qTargetDir = Quaternion.LookRotation(targetDir, Vector3.up);
	
		//set the rotation to the lerp between the current and the desired direction
		transform.rotation = Quaternion.Lerp(this.transform.rotation, qTargetDir, step);

	}
	#region Private Properties

	private bool IsInLocomotion
	{
		get { return m_stateInfo.nameHash == m_locomotionId; }
	}
	
	private bool IsPivoting
	{
		get { return m_stateInfo.nameHash == m_locomotionPivot_L || m_stateInfo.nameHash == m_locomotionPivot_R; }
	}
	
	#endregion

	#region Public Properties

	public float AnimatorSpeed
	{
		get { return m_animator.speed; }
		set { m_animator.speed = value; }
	}

	#endregion
}