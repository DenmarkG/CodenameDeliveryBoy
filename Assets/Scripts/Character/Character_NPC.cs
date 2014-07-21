using UnityEngine;
using System.Collections;

public class Character_NPC : CharacterBase 
{
	protected override void Awake()
	{
		base.Awake();
		m_missionManager = new MissionManager();
		m_motor = this.gameObject.AddComponent<Motor_NPC>();
		m_overWorldState = new State_CharacterOverworld(m_motor);
		m_stateMachine.SetCurrentState(m_overWorldState);
	}

	public void Update()
	{
		if (!m_isPaused)
		{
			m_stateMachine.UpdateState();
		}
	}

	public void FixedUpdate()
	{
		if (!m_isPaused)
		{
			m_stateMachine.UpdateStateFixed();
		}
	}
	
	public override void OnSpeak()
	{
		base.OnSpeak();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			GuiManager.ShowDialog("Ilsa, I’m no good at being noble but it doesn’t take much to see that the problems of " +
			           "three little people dont amount to a hill o beans in this crazy world. Someday youll understand that");
		}
	}
}
