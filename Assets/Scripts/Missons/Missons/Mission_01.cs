using UnityEngine;
using System.Collections;

public class Mission_01 : Mission
{
	[SerializeField]
	private InventoryItem m_groceries = null;

	private Character_Player m_player;

	public Mission_01()
	{
		m_missionName = "Deliver Groceries";
		m_missionDescription = "Get the Groceries from the green cube, and deliver to the blue cube.";
		m_groceries = new Food();
	}

	public override void BeginMission ()
	{
		m_currenState = MissionState.IN_PROGRESS;
		m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_Player>();
	}

	public override bool CheckForCompletion()
	{
		return m_player.PlayerInventory.Contains(m_groceries);
	}

	public override void EndMission ()
	{
		m_currenState = MissionState.SUCCESS;
		m_player.PlayerMissionManager.UpdateMission(this);
		m_player.PlayerInventory.RemoveItem(m_groceries);
	}

	public InventoryItem Item
	{
		get { return m_groceries; }
	}
}
