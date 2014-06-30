using UnityEngine;
using System.Collections;

public class State_Camera_Orbit : State_Base 
{
	//private member variables
	private PlayerCamera m_camera = null;
	private Transform m_cameraTransform = null;
	private Transform m_target = null;
	private Vector3 m_lookDir = Vector3.zero;

	Vector3 m_desiredPosition = Vector3.zero;

	private float m_mouseX = 0f;
	private float m_mouseY = 0f;

	public State_Camera_Orbit(PlayerCamera pCamera)
	{
		//store the camera component that was passed in as the camera to be used by this script
		m_camera = pCamera;
	}
	
	public override void EnterState ()
	{
		//cache the transform compenent of the camera
		m_cameraTransform = m_camera.transform;
		
		//get the target and look direction from the camera that was passed in
		m_target = m_camera.CameraTarget;
		m_lookDir = m_camera.LookDirection;

//		Debug.Log("Orbit State Entered");
		m_desiredPosition = m_target.position - m_camera.transform.position;
	}
	
	public override void UpdateState ()
	{
		//cache the input from the mouse
		m_mouseX = Input.GetAxis("Mouse X");
		m_mouseY = Input.GetAxis("Mouse Y");
	}

	public override void UpdateStateFixed ()
	{
		//
	}

	public override void LateUpdateState()
	{
		//set the offset to the player each frame
		Vector3 m_targetPos = m_camera.CameraTarget.position;
		m_targetPos.x += m_mouseX;
		m_targetPos.y += m_mouseY;
		
		//calculate the direction from camera to player, kill y, and normalize to create valid direction
		m_lookDir = m_targetPos - m_camera.transform.position;
		m_lookDir.Normalize();
		m_camera.LookDirection = m_lookDir;
		
		//calculate the target position
		m_targetPos = m_targetPos - (m_lookDir * m_camera.DistanceAway);
		
		//move the camera to the new position
		m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, m_targetPos, Time.deltaTime * m_camera.SmoothSpeed);
		
		//Look at the target
		m_camera.SmoothLookAt();
	}
	
	public override void ExitState ()
	{
		//
	}
}
