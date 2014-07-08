using UnityEngine;
using System.Collections;

public class State_Camera_Orbit : State_Base 
{
	//private member variables
	private PlayerCamera m_camera = null;
	private Transform m_target = null;

	Vector3 m_desiredPosition = Vector3.zero;

	private float m_mouseX = 0f;
	private float m_mouseY = 0f;

//	private Vector3 m_mousePos = Vector3.zero;

	public State_Camera_Orbit(PlayerCamera pCamera)
	{
		//store the camera component that was passed in as the camera to be used by this script
		m_camera = pCamera;
	}
	
	public override void EnterState ()
	{	
		//reset the mouse values, so the camera doesn't jump when entering the state
		m_mouseX = 0f;
		m_mouseY = 0f;
		//get the target object from the camera that was passed in
		m_target = m_camera.CameraTarget;

		//set the desired position to be the current position in relation to the player's position
		m_desiredPosition = m_target.position - (m_target.forward * -m_camera.DistanceAway);
	}
	
	public override void UpdateState ()
	{
		//cache the input from the mouse
		if (Mathf.Abs(Input.GetAxis("Mouse X") ) > m_camera.DeadZone)
			m_mouseX += Input.GetAxis("Mouse X")  * m_camera.OrbitSpeed;

		if (Mathf.Abs(Input.GetAxis("Mouse Y") ) > m_camera.DeadZone)
			m_mouseY += Input.GetAxis("Mouse Y") * m_camera.OrbitSpeed;

		m_camera.DistanceAway += -Input.GetAxis("Mouse ScrollWheel") * m_camera.ZoomSpeed;

		//clamp the y value to the range [-360,360]
		m_mouseY = m_camera.ClampAngle(m_mouseY, -360, 360);
	}

	public override void UpdateStateFixed ()
	{
		//
	}

	public override void LateUpdateState()
	{
		//set the direction of the camera to be the Forward vector times the Distance away of the camera
		Vector3 direction = m_camera.transform.forward * -m_camera.DistanceAway;

		//create a rotation based on the current mouse x and y values
		Quaternion rotation = Quaternion.Euler(m_mouseY, m_mouseX, 0f);

		//calculate the desired position based on the position, direction, and roation that we just calculated
		m_desiredPosition = m_target.position + rotation * direction;

		//move the camera to the new position
		m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, m_desiredPosition, Time.deltaTime);
		
		//Look at the target
		m_camera.SmoothLookAt();
	}
	
	public override void ExitState ()
	{
		//
	}
}
