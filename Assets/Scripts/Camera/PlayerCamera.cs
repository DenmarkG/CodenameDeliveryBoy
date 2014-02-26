using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{
	#region Private Serialized Variables

	[SerializeField]
	private float m_distanceAway = 5f; //How far away the camera should be
	[SerializeField]
	private float m_distanceUp = 2f;
	[SerializeField]
	private float m_offsetHeight = 2f;
	[SerializeField]
	private float m_offsetDistance = 2f;
	[SerializeField]
	[Range(0,10)]
	private float m_smooth = 0.25f;
	[SerializeField]
	private Transform m_target = null;
	[SerializeField]
	private Vector3 m_velSmooth = Vector3.zero;
	[SerializeField]
	private float m_smoothDampTime = 0.1f;

	#endregion

	#region Private Variables

	//private Vector3 m_heightOffset = Vector3.zero;
	private Vector3 m_targetPos = Vector3.zero;
	private Vector3 m_lookDir = Vector3.zero;

	#endregion


	void Start () 
	{
		if(m_target == null)
		{
			m_target = GameObject.FindGameObjectWithTag("PlayerCameraTarget").transform;
		}

		//m_heightOffset.y += m_offset;
	}

	void LateUpdate () 
	{
		//set the offset to the player each frame
		//Vector3 characterOffset = m_target.position + m_heightOffset;
		m_targetPos = m_target.position;
		m_targetPos += (m_offsetHeight * m_target.up) - (m_offsetDistance * m_target.forward);

		//calculate the direction from camera to player, kill y, and normalize to create valid direction
		//m_lookDir = characterOffset - this.transform.position;
		m_lookDir = m_targetPos - this.transform.position;
		m_lookDir.y = 0;
		m_lookDir.Normalize();

		//calculate the target position
		m_targetPos = ( m_targetPos + m_target.up * m_distanceUp ) - (m_lookDir * m_distanceAway);

		//move the camera to the new position
		//SmoothPostion(this.transform.position, m_targetPos);
		transform.position = Vector3.Lerp(transform.position, m_targetPos, Time.deltaTime * m_smooth);

		//Look at the target
		transform.LookAt(m_target);
	}

	void SmoothPostion(Vector3 fromPos, Vector3 toPos)
	{
		this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref m_velSmooth, m_smoothDampTime);
	}
}
