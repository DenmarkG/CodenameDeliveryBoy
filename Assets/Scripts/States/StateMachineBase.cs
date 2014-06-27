using UnityEngine;
using System.Collections;

public class StateMachineBase 
{
	private State_Base currentState = null;
	
	public void UpdateState()
	{
		if(currentState != null)
		{
			currentState.UpdateState(); 
		}
	}

	public void UpdateStateFixed()
	{
		if(currentState != null)
		{
			currentState.UpdateStateFixed(); 
		}
	}
	
	public void SetCurrentState(State_Base newState)
	{
		if(currentState != null)
		{
			currentState.ExitState();
		}
		
		currentState = newState; 
		
		if(currentState != null)
		{
			currentState.EnterState();
		}
	}
	
	public State_Base CurrentState
	{
		get { return currentState; }
	}
}
