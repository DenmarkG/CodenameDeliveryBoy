using UnityEngine;
using System.Collections;


public class PlayerMotor : MonoBehaviour 
{
	[SerializeField]
	[Range(0,1)]
	private float m_moveDamp = 0.25f;
	[SerializeField]
	private float m_rotSpeed = 1.5f;
	[SerializeField]
	private float m_degreesPerSec = 120f;
	[SerializeField]
	private Camera m_gameCamera = null;

	private float m_direction = 0f;
	private float m_speed = 0f;
	private float m_horizontal = 0f;
	private float m_vertical = 0f;


	private Animator m_animator; 
	private int m_locomotionId = 0;
	private AnimatorStateInfo m_stateInfo;

	protected virtual void Awake()
	{
		m_animator = this.GetComponent<Animator>();
		m_locomotionId = Animator.StringToHash("Base Layer.Locomotion");

		if(m_gameCamera == null)
		{
			m_gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		}
	}

	public void UpdateMotor() 
	{
		//get the state info for the current state
		m_stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

		m_horizontal = Input.GetAxis("Horizontal");
		m_vertical = Input.GetAxis("Vertical");

		//Translate the input to "world space"
		InputToWorldSpace(this.transform, m_gameCamera.transform, ref m_direction, ref m_speed);

		m_animator.SetFloat("Speed", m_speed);
		m_animator.SetFloat("Direction", m_horizontal, m_moveDamp, Time.deltaTime);
	}

	public void UpdateMotorFixed()
	{
		if(IsInLocomotion() && ( (m_direction >= 0 && m_speed >= 0) ||  (m_direction <= 0 && m_speed <= 0) ) )
		{
			Vector3 toRot = new Vector3(0, m_degreesPerSec * (m_horizontal < 0f ? -1f: 1f), 0f);
			Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, toRot, Mathf.Abs(m_horizontal) ); 
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
			this.transform.rotation = this.transform.rotation * deltaRotation;
		}
	}

	void InputToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut)
	{
		//the root is essentially the actual direction of the character
		Vector3 rootDiretion = root.forward;

		Vector3 axisDirection = new Vector3(m_horizontal, 0, m_vertical);

		speedOut = axisDirection.sqrMagnitude;

		//Get camera rotation
		Vector3 cameraDirection = camera.forward;
		cameraDirection.y = 0f;
		Quaternion refShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(cameraDirection) );

		//Convert input in World Space coords
		Vector3 moveDirection = refShift * axisDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDiretion);

		// Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		// Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDiretion, Color.magenta);
		// Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), axisDirection, Color.blue);

		float angleToMove = Vector3.Angle(rootDiretion, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

		angleToMove /= 180f;

		directionOut = angleToMove * m_rotSpeed;
	}

	private bool IsInLocomotion()
	{
		return m_stateInfo.nameHash == m_locomotionId;
	}
}
