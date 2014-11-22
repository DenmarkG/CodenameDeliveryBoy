using UnityEngine;
using System.Collections;

public class State_CharacterOverworld : State_Base
{
	Motor_Base m_motor;

	public State_CharacterOverworld(Motor_Base pMotor)
	{
		m_motor = pMotor;
	}

	public override void EnterState()
	{
		//
	}

	public override void UpdateState()
	{
		if (m_motor != null)
        {
            m_motor.UpdateMotor();
        }
	}

	public override void UpdateStateFixed()
	{
		if (m_motor != null)
        {
            m_motor.UpdateMotorFixed();
        }
	}

	public override void LateUpdateState()
	{
		//
	}

	public override void ExitState()
	{
		//
	}
}
