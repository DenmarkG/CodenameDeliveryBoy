using UnityEngine;
using System.Collections;

public class Character_Player : CharacterBase 
{
	private MissionManager m_missionManager = null;
	private Inventory m_inventory = null;
	private PlayerMotor m_playerMotor = null;

	protected override void Awake ()
	{
		base.Awake ();
		m_missionManager = new MissionManager();
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

	void Update()
	{
		m_stateMachine.UpdateState();
	}

	void FixedUpdate()
	{
		m_stateMachine.UpdateStateFixed();
	}

	public PlayerMotor Motor
	{
		get { return m_playerMotor; }
	}
}
