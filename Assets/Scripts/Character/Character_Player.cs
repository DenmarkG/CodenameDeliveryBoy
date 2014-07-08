using UnityEngine;
using System.Collections;

public class Character_Player : CharacterBase 
{
	private Inventory m_inventory = null;

	protected override void Awake ()
	{
		base.Awake();
		m_inventory = new Inventory();
		m_missionManager = new MissionManager();

		//make sure the player has a motor
		if(m_motor == null)
		{
			m_motor = this.GetComponent<PlayerMotor>();
			if(m_motor == null)
			{
				m_motor = this.gameObject.AddComponent<PlayerMotor>();
			}
		}

		//set the inital state
		m_overWorldState = new State_CharacterOverworld(m_motor);
		m_stateMachine.SetCurrentState(m_overWorldState);
	}

	void Start()
	{
		m_inventory.AddItem(new Wallet() );
	}

	void Update()
	{
		if (!m_isPaused)
		{
			m_stateMachine.UpdateState();
			
			//Show Missions
			if (Input.GetKeyDown(KeyCode.M) )
				m_missionManager.ToggleMissionDisplay();
			
			//Toggle the inventory
			if (Input.GetKeyDown (KeyCode.I) )
				m_inventory.ToggleInventoryDisplay();
		}
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

	#endregion
}
