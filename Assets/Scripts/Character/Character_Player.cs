﻿using UnityEngine;
using System.Collections;

public class Character_Player : CharacterBase 
{
	private Inventory m_inventory = null;

	private PlayerMotor m_playerMotor = null;

	protected override void OnAwake ()
	{
		m_inventory = new Inventory();

		//make sure the player has a motor
		if(m_playerMotor == null)
		{
			m_playerMotor = this.GetComponent<PlayerMotor>();
			if(m_playerMotor == null)
			{
				m_playerMotor = this.gameObject.AddComponent<PlayerMotor>();
			}
		}

		//set the inital state
		m_stateMachine.SetCurrentState(new StatePlayerOverworld(m_playerMotor) );
	}

	void Start()
	{
		m_inventory.AddItem(new Wallet() );
	}

	void Update()
	{
		m_stateMachine.UpdateState();

		//Show Missions
		if (Input.GetKeyDown(KeyCode.M) )
		{
			GuiManager.OnUpdateGUI += m_missionManager.DisplayCurrentMissions;
		}

		if (Input.GetKeyUp(KeyCode.M) )
		{
			GuiManager.OnUpdateGUI -= m_missionManager.DisplayCurrentMissions;
		}

		//Toggle the inventory
		if (Input.GetKeyDown (KeyCode.I))
			m_inventory.ToggleInventory ();
	}

	void FixedUpdate()
	{
		m_stateMachine.UpdateStateFixed();
	}

	#region Properties

	public MissionManager PlayerMissionManager
	{
		get { return m_missionManager; }
	}

	public Inventory PlayerInventory
	{
		get { return m_inventory; }
	}

	public PlayerMotor Motor
	{
		get { return m_playerMotor; }
	}

	#endregion
}
