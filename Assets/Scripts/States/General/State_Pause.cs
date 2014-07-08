using UnityEngine;
using System.Collections;

public class State_Pause : State_Base
{
	private Animator m_animator = null;

	public State_Pause(Animator pAnimator)
	{
		m_animator = pAnimator;
	}

	public override void EnterState()
	{
		if (m_animator != null)
		{
//			m_animator.SetBool()
		}
	}

	public override void UpdateState()
	{
		//
	}

	public override void UpdateStateFixed()
	{
		//
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
