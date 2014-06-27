using UnityEngine;
using System.Collections;

public class StatePlayerOverworld : State_Base 
{
	PlayerMotor m_motor = null;

	public StatePlayerOverworld(PlayerMotor motor)
	{
		m_motor = motor;
	}

	public override void EnterState ()
	{
		//
	}

	public override void UpdateState ()
	{
		m_motor.UpdateMotor();
	}

	public override void UpdateStateFixed()
	{
		m_motor.UpdateMotorFixed();
	}

	public override void ExitState()
	{
		//
	}
}
