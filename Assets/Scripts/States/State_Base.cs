using UnityEngine;
using System.Collections;

public abstract class State_Base 
{
	public abstract void EnterState();
	public abstract void UpdateState();
	public abstract void UpdateStateFixed();
	public abstract void ExitState();
}
