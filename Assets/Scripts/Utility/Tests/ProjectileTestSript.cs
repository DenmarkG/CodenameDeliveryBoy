using UnityEngine;
using System.Collections;

public class ProjectileTestSript : MonoBehaviour 
{
	[SerializeField]
	private float m_moveSpeed = .5f;
	private Transform m_transform = null;

	private bool m_active = true;

	void Awake()
	{
		m_transform = this.transform;
	}

	void Update()
	{
		if (m_active)
		{
			RaycastHit hit;
			Vector3 nextPos = m_transform.position + (m_transform.forward * m_moveSpeed);
			if (Physics.Raycast(transform.position, Vector3.forward, out hit, m_moveSpeed) )
			{
				m_active = false;
			}
			else
			{
				m_transform.position = Vector3.Lerp(m_transform.position, nextPos, m_moveSpeed * Time.deltaTime);
			}
		}
	}

	void FixedUpdate()
	{
		//
	}
}
