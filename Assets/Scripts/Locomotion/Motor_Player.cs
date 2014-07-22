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
				m_animator.SetFloat("Angle", m_angle);
			}
			//[#todo]check to see if an elseif can be made instead later
			if (m_speed < DEAD_ZONE && Mathf.Abs(m_horizontal) < DEAD_ZONE)
			{
				m_animator.SetFloat("Speed", 0);
				//m_animator.SetFloat("Angle", 0);
			}

			//[#todo] implement a pause method that utilizes this method
			m_animator.speed = Clock.TimeScale;
		}
	}

	public override void LockPlayerMotion()
	{
		base.LockPlayerMotion();
		m_animator.SetFloat("Speed", 0);
		m_animator.SetFloat("Direction", 0);
		m_animator.SetFloat("Angle", 0);
	}
	
	public override void UnlockPlayerMotion()
	{
		base.UnlockPlayerMotion();
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

		//m_direction = (playerDirection - moveVector).magnitude;

		//create a movement vector based on the input
		//Vector3 inputAxisDirection = new Vector3(0, 0, moveVector.z);

		//if the player is running set the speed to 2. Otherwise set it to the length of the input vector
		m_speed = moveVector.magnitude;

		if (m_isRunning && m_speed > DEAD_ZONE)
			m_speed = Mathf.Lerp(m_speed, RUN_SPEED, Clock.DeltaTime);


		//calculate the rotation from the input vector to the player's forward
		//Quaternion fromInputToPlayerRotation = Quaternion.FromToRotation(moveVector, playerDirection);

		//rotate the axis direction so that it is now oriented with the player 
		//i.e. axis z will correspond to the player's forward
//		Vector3 moveVector = fromInputToPlayerRotation * inputAxisDirection;
		
		//now calculate the angle between the move vector and the camera's forward vector
//		m_angle = Vector3.Angle(cameraDiretion, moveVector);
		
		//since the angle will always return positive, we want to calculate the direction the player is turning
		//to do this we take the dot product of...
		//float axisSign = Vector3.Dot(moveVector, this.transform.right) > 0 ? 1 : -1;
		
		//now apply the sign to the angle
		//m_angle *= axisSign;
		
//		Debug.Log("angle: " + m_angle);

		//get the angle between the camera's forward vector and the movement vector
		//float axisSign = Vector3.Cross(moveVector, playerDirection).y >= 0 ? -1 : 1;
		m_angle = Vector3.Angle(cameraDiretion, moveVector) * (m_horizontal >= 0 ? 1 : -1);
//		m_angle = Vector3.Angle(cameraDiretion, moveVector) * (axisSign);
		//Debug.Log("Angle: " + m_angle);
		m_direction = m_angle / 180f;
		//Debug.Log("Direction: " + m_direction);

		//Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), inputAxisDirection, Color.green);
		Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), cameraDiretion, Color.blue);
		Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), moveVector, Color.red);
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