using UnityEngine;
using System.Collections;

public class State_Camera_Orbit : State_Base 
{
	//private member variables
	private PlayerCamera m_camera = null;
//	private Transform m_targetTransform = null;
	
	//private Vector3 m_desiredPosition = Vector3.zero;
	
	private bool m_bUsingController = false;
	
	private Vector3 m_moveVector = Vector3.zero;
	private Vector3 m_targetPos = Vector3.zero;
	Vector3 m_lookDir = Vector3.zero;
	
	public State_Camera_Orbit(PlayerCamera pCamera)
	{
		m_camera = pCamera;
//		m_targetTransform = m_camera.CameraTarget;
	}
	
	
	public override void EnterState ()
	{
		m_bUsingController = GameManager.IsUsingController;
	}
	
	public override void UpdateState()
	{
		float input_X = 0f;
		float input_Y = 0f;

		if (m_bUsingController)
			GetControllerInput(out input_X, out input_Y);
		else
			GetMouseInput(out input_X, out input_Y);
			
		//convert to the horizontal input to coordinates
		m_moveVector = Vector3.zero;
		
		//if (Mathf.Abs(input_X) > m_camera.DeadZone)
			m_moveVector.x -= input_X;
		
		//if (Mathf.Abs(input_Y) > m_camera.DeadZone)
			m_moveVector.y -= input_Y;

		//update the distance for the camera
		float deltaZoom = -Input.GetAxis("Mouse ScrollWheel");
		m_camera.DistanceAway += deltaZoom;
	}
	
	public override void UpdateStateFixed()
	{
		//no physics step for the camera
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
		
		m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, m_targetPos, Time.deltaTime * m_camera.SmoothSpeed);

		float step = m_camera.OrbitSpeed * Clock.DeltaTime;
		m_camera.transform.RotateAround(m_camera.CameraTarget.position, m_camera.CameraTarget.up, step *  m_moveVector.x);
		m_camera.SmoothLookAt();
		m_camera.transform.RotateAround(m_camera.CameraTarget.position, m_camera.CameraTarget.right, step *  m_moveVector.y);
		m_camera.SmoothLookAt();
	}
	
	public override void ExitState()
	{
		//
	}

	#region input methods
	
	private void GetMouseInput(out float input_X, out float input_Y)
	{
		input_X = Input.GetAxis("Mouse X");
		input_Y = Input.GetAxis("Mouse Y");
	}
	
	private void GetControllerInput(out float mouse_X, out float mouse_Y)
	{
		mouse_X = 0f;
		mouse_Y = 0f;
	}
	
	#endregion
}