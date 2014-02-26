using UnityEngine;
using System.Collections;

public abstract class StateBase 
{
	public abstract void EnterState();
	public abstract void UpdateState();
	public abstract void UpdateStateFixed();
	public abstract void ExitState();
}
