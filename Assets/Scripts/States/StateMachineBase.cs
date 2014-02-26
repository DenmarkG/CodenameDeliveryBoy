using UnityEngine;
using System.Collections;

public class StateMachineBase 
{
	private StateBase currentState = null;
	
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
	
	public void SetCurrentState(StateBase newState)
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
	
	public StateBase CurrentState
	{
		get { return currentState; }
	}
}
