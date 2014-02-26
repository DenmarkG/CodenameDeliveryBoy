using UnityEngine;
using System.Collections;

public class CharacterBase : MonoBehaviour 
{
	protected bool m_paused = false;
	protected StateMachineBase m_stateMachine = null; 

	protected virtual void Awake()
	{
		m_stateMachine = new StateMachineBase();
	}

	public virtual void OnPause()
	{
		m_paused = true;
	}

	public virtual void OnResume()
	{
		m_paused = false;
	}
}
