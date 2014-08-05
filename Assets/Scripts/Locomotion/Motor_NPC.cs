using UnityEngine;
using System.Collections;

public class Motor_NPC : Motor_Base
{
	public override void UpdateMotor ()
	{
		m_animator.speed = Clock.TimeScale;
	}

	public override void UpdateMotorFixed ()
	{
		//
	}
}
