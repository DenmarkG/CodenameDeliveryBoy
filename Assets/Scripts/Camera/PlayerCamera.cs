using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{
	#region Private Serialized Variables

	[SerializeField]
	private float m_distanceAway = 3f; //How far away the camera should be
	[SerializeField]
	private float m_offsetHeight = 1.5f;
	[SerializeField]
	[Range(0,10)]
	private float m_smooth = 0.25f;
	[SerializeField]
	private float m_smoothDampTime = 0.1f;
	[SerializeField]
	private float m_snapSpeed = 20f;

	#endregion

	#region Private Variables

	private Transform m_target = null;
	private Vector3 m_targetPos = Vector3.zero;
	private Vector3 m_lookDir = Vector3.zero;
	private Vector3 m_velSmooth = Vector3.zero;
	private static PlayerCamera m_instance = null;
	private float m_deadZone = .1f;
	private Vector3 m_desiredPosition = Vector3.zero;
	private Transform m_transform = null;

	#endregion

	void Awake()
	{
		if (m_instance == null)
			m_instance = this;
	}

	void Start () 
	{
		m_transform = this.transform;

		if(m_target == null)
		{
			m_target = GameObject.FindGameObjectWithTag("PlayerCameraTarget").transform;
		}

		m_desiredPosition = m_target.position - ( (m_target.up * m_offsetHeight) + (m_target.forward * -m_distanceAway) );
	}

	void LateUpdate () 
	{
		if(Input.GetAxis("LEFT_TRIGGER") < -m_deadZone && !IsInvoking("ResetCamera") )
		{
			StartCoroutine("ResetCamera");
		}

		//set the offset to the player each frame
		m_targetPos = m_target.position;
		m_targetPos.y = m_offsetHeight;

		//calculate the direction from camera to player, kill y, and normalize to create valid direction
		//m_lookDir = characterOffset - m_transform.position;
		m_lookDir = m_targetPos - m_transform.position;
		//m_lookDir.y = 0;
		m_lookDir.Normalize();

		//calculate the target position
		m_targetPos = m_targetPos - (m_lookDir * m_distanceAway);
		
		//move the camera to the new position
		transform.position = Vector3.Lerp(transform.position, m_targetPos, Time.deltaTime * m_smooth);

		//Look at the target
		SmoothLookAt();
	}
	#region Custom Methods
	
	void SmoothLookAt()
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

	/// <summary>
	/// This doesn't work yet
	/// </summary>
	/// <returns>The camera.</returns>
	IEnumerator ResetCamera()
	{
		Vector3 relativePos = m_target.position - m_desiredPosition;

		while (Vector3.Distance(m_transform.position, relativePos) > .1f )
		{
			relativePos = m_target.position - m_desiredPosition;
			m_transform.position = Vector3.Lerp(m_transform.position, relativePos, m_snapSpeed * Time.deltaTime);
			Debug.DrawRay(m_transform.position, relativePos, Color.green);
			SmoothLookAt();
			yield return null;
		}
	}

	#endregion

	#region Properties

	public static PlayerCamera PlayerCam
	{
		get { return m_instance; }
	}

	#endregion
}
