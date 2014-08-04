using UnityEngine;
using System.Collections;

public class State_Camera_Orbit : State_Base 
{
	//private member variables
	private PlayerCamera m_camera = null;
	private Transform m_target = null;

	Vector3 m_desiredPosition = Vector3.zero;

	private float m_input_X = 0f;
	private float m_input_Y = 0f;

	//this variable will be needed to cacluate mouse deltas, since that information is not captured by Unity
	private Vector2 m_lastInputPos = Vector2.zero;
	Vector2 m_inputPosition = Vector2.zero;

	private bool m_bUsingController = false;

	public State_Camera_Orbit(PlayerCamera pCamera)
	{
		//store the camera component that was passed in as the camera to be used by this script
		m_camera = pCamera;
	}
	
	public override void EnterState ()
	{	
		//reset the mouse values, so the camera doesn't jump when entering the state
		m_input_X = 0f;
		m_input_Y = 0f;

		//get the target object from the camera that was passed in
		m_target = m_camera.CameraTarget;

		m_lastInputPos = (Vector2) Input.mousePosition;

		m_bUsingController = GameManager.IsUsingController;
//		Debug.Log(m_bUsingController);
	}
	
	public override void UpdateState ()
	{
		float delta_X = 0f;
		float delta_Y = 0f;

		if (m_bUsingController)
		{
			GetControllerInput();
		}
		else
		{
			GetMouseInput(out delta_X, out delta_Y);
		}

		//if the position delta is greater than the deadzone, increment the x value. Lerp it back to zero otherwise
		if (Mathf.Abs(delta_X) > m_camera.DeadZone)
		{
			//cache the input from the mouse
			m_input_X += Mathf.Clamp(delta_X, -1, 1);

			//set this as the last mouse x position
			m_lastInputPos.x = m_inputPosition.x;
		}
		else
		{
			//if the mouse is not moving on the x axis, lerp the mouse x back to 0
			m_input_X = Mathf.Lerp(m_input_X, 0, Clock.DeltaTime  * m_camera.MoveStopSpeed);
		}

		//if the position delta is greater than the deadzone, increment the y value. Lerp it back to zero otherwise
		if (Mathf.Abs(delta_Y) > m_camera.DeadZone)
		{
			m_input_Y += Mathf.Clamp(delta_Y, -1, 1);
			
			//Debug.Log(m_mouseY);
			
			//clamp the y value to the range [-40,80]
			m_input_Y = m_camera.ClampAngle(m_input_Y, -40, 80);

			//set this as the last mouse y position
			m_lastInputPos.y = m_inputPosition.y;
		}
		else
		{
			//if the mouse is not moving on the y axis, lerp the mouse y back to 0
			m_input_Y = Mathf.Lerp(m_input_Y, 0, Clock.DeltaTime  * m_camera.MoveStopSpeed);
		}

		m_camera.DistanceAway += -Input.GetAxis("Mouse ScrollWheel") * m_camera.ZoomSpeed;
	}

	public override void UpdateStateFixed ()
	{
		//This state won't be used but must be implemented (the base class is abstract)
	}


	//[#todo] make the camera rotate in a circle around the player, right now it 
	public override void LateUpdateState()
	{
		Transform cameraTransform = m_camera.transform;

		//set the direction of the camera to be the Forward vector times the Distance away of the camera
		Vector3 direction = cameraTransform.forward * -m_camera.DistanceAway;

		//this 'works'
		//create a rotation based on the current mouse x and y values
		Quaternion rotation = Quaternion.Euler(m_input_Y, m_input_X, 0f);
		//calculate the desired position based on the position, direction, and roation that we just calculated
		m_desiredPosition = m_target.position + rotation * direction;

		//Debug.Log ("desired: " + m_desiredPosition + "\nmag: " + m_desiredPosition.magnitude);

		//move the camera to the new position
		Vector3 targetPos = Vector3.Lerp(m_camera.transform.position, m_desiredPosition, Clock.DeltaTime);
		targetPos.Normalize();
		targetPos *= m_camera.DistanceAway;

		m_camera.transform.position = targetPos;
		
		//Look at the target
		m_camera.SmoothLookAt();
	}
	
	public override void ExitState ()
	{
//		Debug.Log("leaving orbit state");
	}

	#region Input Caching Methods

	private void GetMouseInput(out float delta_X, out float delta_Y)
	{
		//get the mouse's current position
		m_inputPosition = (Vector2) Input.mousePosition;

//		float mouse_X = Input.GetAxis("Mouse X");
//		float mouse_Y = Input.GetAxis("Mouse Y");

		//calculate the change in position since the last frame
		delta_X = m_lastInputPos.x - m_inputPosition.x;
		delta_Y = m_lastInputPos.y - m_inputPosition.y;

//		delta_X = m_lastInputPos.x - mouse_X;
//		delta_Y = m_lastInputPos.y - mouse_Y;
	}

	private void GetControllerInput()
	{
		//
	}

	#endregion
}
