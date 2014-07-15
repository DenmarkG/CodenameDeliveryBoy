using UnityEngine;
using System.Collections;

public class State_Camera_Follow : State_Base
{
	//private member variables
	PlayerCamera m_camera = null;
	Vector3 m_targetPos = Vector3.zero;
	Vector3 m_lookDir = Vector3.zero;

	public State_Camera_Follow(PlayerCamera pCamera)
	{
		m_camera = pCamera;
	}

	public override void EnterState ()
	{
//		Debug.Log("Follow State Entered");
	}

	public override void UpdateState ()
	{	
		//
	}

	public override void UpdateStateFixed ()
	{
		//
	}

	public override void LateUpdateState()
	{
		//set the offset to the player each frame
		m_targetPos = m_camera.CameraTarget.position;
		m_targetPos.y = m_camera.OffsetHeight;
		
		//calculate the direction from camera to player, kill y, and normalize to create valid direction
		m_lookDir = m_targetPos - m_camera.transform.position;
		m_lookDir.Normalize();
		m_camera.LookDirection = m_lookDir;
		
		//calculate the target position
		m_targetPos = m_targetPos - (m_lookDir * m_camera.DistanceAway);
		
		//move the camera to the new position
		Vector3 targetPos = Vector3.Lerp(m_camera.transform.position, m_targetPos, Time.deltaTime * m_camera.SmoothSpeed);
		targetPos.Normalize();
		targetPos *= m_camera.DistanceAway;

		m_camera.transform.position = targetPos;
		
		//Look at the target
		m_camera.SmoothLookAt();
	}

	public override void ExitState()
	{
		//
	}
}
