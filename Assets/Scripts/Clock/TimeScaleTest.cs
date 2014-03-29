using UnityEngine;
using System.Collections;

public class TimeScaleTest : MonoBehaviour 
{
	[SerializeField]
	private float m_rotSpeed = 3f;

	private Transform m_transform = null;

	void Awake()
	{
		m_transform = this.transform;
	}

	void Update()
	{
		m_transform.rotation *= Quaternion.Euler(Vector3.up * m_rotSpeed * Clock.GetTimeScale);
	}
}
