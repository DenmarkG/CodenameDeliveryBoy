using UnityEngine;
using System.Collections;

public class RaycastTest : MonoBehaviour 
{
	[SerializeField]
	private int m_numRays = 8;
	[SerializeField]
	[Range(0,360)]
	private int m_angle = 75;
	[SerializeField]
	private float m_distance = 25;

	void Update()
	{
		Vector3 dir = this.transform.forward;
		Vector3 pos = this.transform.position;


		for (int i = (int) -Mathf.Floor(m_numRays / 2); i <= Mathf.Floor(m_numRays / 2); ++i)
		{
			//get the angle to cast the ray at
			float currentAngle = (m_angle / m_numRays) * i;

			//convert to radians
			currentAngle = currentAngle * (Mathf.PI / 180);

			Vector3 newDir = Rotate2D(dir.x, dir.y, dir.z, currentAngle);

			RaycastHit hit;
			if (Physics.Raycast(pos, newDir, out hit, m_distance) )
			{
				Debug.Log("hit something!");
			}

			//Debug.DrawRay(pos, newDir * m_distance, Color.white);
		}
	}

	Vector3 Rotate2D(float x, float y, float z, float angle)
	{
		float _x = (x * Mathf.Cos(angle) ) - (z * Mathf.Sin(angle) );
		float _z = (x * Mathf.Sin(angle) ) + (z * Mathf.Cos(angle) );
		//Vector3 newVec3 = new Vector3(_x, _y, z);

		return new Vector3(_x, y, _z);
	}
}
