using UnityEngine;
using System.Collections;

public class Character_NPC : CharacterBase 
{
	protected override void OnAwake()
	{
		//
	}
	
	public override void OnPause()
	{
		base.OnPause();
		m_paused = true;
	}
	
	public override void OnResume()
	{
		base.OnResume();
		m_paused = false;
	}
	
	public override void OnSpeak()
	{
		base.OnSpeak();
	}
}
