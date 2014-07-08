using UnityEngine;
using System.Collections;

public class PlayerMotor : Motor_Base
{
	public override void UpdateMotor ()
	{
		//reset the angle to zero
		m_angle = 0f;

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
		m_animator.SetFloat("Angle", m_angle);

		//testing the time scale
		m_animator.speed = Clock.TimeScale;
	}

	private void ConvertInputToWorldSpace()
	{
		//**************************************************************************************************
		//This function is currently a work in progress
		//**************************************************************************************************

		//cache the forward vector of the player and the camera
		//also remove the y values for each, since we only want to calculate in 2 dimensions
		Vector3 playerDirection = this.transform.forward;
		playerDirection.y = 0;
		Vector3 cameraDiretion = m_camera.transform.forward;
		cameraDiretion.y = 0;
		cameraDiretion.Normalize(); //normalize the camera vector to keep lenght consistent

		//create a movement vector based on the input
		Vector3 inputAxisDirection = new Vector3(m_horizontal, 0, m_vertical);

		//if the player is running set the speed to 2. Otherwise set it to the length of the input vector
		m_speed = inputAxisDirection.magnitude;
		if (m_isRunning && m_speed > DEAD_ZONE)
			m_speed = 2;


		//calculate the rotation from the input vector to the player's forward
		Quaternion fromInputToPlayerRotation = Quaternion.FromToRotation(inputAxisDirection, playerDirection);

		//rotate the axis direction so that it is now oriented with the player 
		//i.e. axis z will correspond to the player's forward
		Vector3 moveVector = fromInputToPlayerRotation * inputAxisDirection;
		
		//now calculate the angle between the move vector and the camera's forward vector
//		m_angle = Vector3.Angle(cameraDiretion, moveVector);
		
		//since the angle will always return positive, we want to calculate the direction the player is turning
		//to do this we take the dot product of...
		//float axisSign = Vector3.Dot(moveVector, this.transform.right) > 0 ? 1 : -1;
		
		//now apply the sign to the angle
		//m_angle *= axisSign;
		
//		Debug.Log("angle: " + m_angle);

		//get the angle between the camera's forward vector and the movement vector
		//float angle = Vector3.Angle(cameraDiretion, inputAxisDirection) * (m_horizontal >= 0 ? 1 : -1);
		//Debug.Log("Angle: " + angle);
		//m_direction = angle / 90f;
		m_direction = m_horizontal;

		Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), inputAxisDirection, Color.green);
		Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), cameraDiretion, Color.blue);
		Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), moveVector, Color.red);
	}
	
	private bool IsInLocomotion()
	{
		return m_stateInfo.nameHash == m_locomotionId;
	}
	
	private bool IsPivoting()
	{
		return m_stateInfo.nameHash == m_locomotionPivot_L || m_stateInfo.nameHash == m_locomotionPivot_R;
	}
}