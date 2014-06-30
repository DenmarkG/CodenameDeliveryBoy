using UnityEngine;
using System.Collections;

public class StateMachineBase 
{
	private State_Base m_currentState = null;
	
	public void UpdateState()
	{
		if(m_currentState != null)
		{
			m_currentState.UpdateState(); 
		}
	}

	public void UpdateStateFixed()
	{
		if(m_currentState != null)
		{
			m_currentState.UpdateStateFixed(); 
		}
	}

	public void LateUpdateState()
	{
		if (m_currentState != null)
		{
			m_currentState.LateUpdateState();
		}
	}
	
	public void SetCurrentState(State_Base newState)
	{
		if(m_currentState != null)
		{
			m_currentState.ExitState();
		}
		
		m_currentState = newState; 
		
		if(m_currentState != null)
		{
			m_currentState.EnterState();
		}
	}
	
	public State_Base CurrentState
	{
		get { return m_currentState; }
	}
}
