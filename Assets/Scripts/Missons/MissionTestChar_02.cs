using UnityEngine;
using System.Collections;

public class MissionTestChar_02 : MonoBehaviour 
{
	[SerializeField]
	private InventoryItem m_itemToGive = MissionHash.mission_01.Item;

	private Mission mission = MissionHash.mission_01;

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			if(mission.GetMissionState == MissionState.IN_PROGRESS)
			{
				Character_Player player = other.gameObject.GetComponent<Character_Player>();
				if(!player.PlayerInventory.Contains(m_itemToGive) )
					player.PlayerInventory.AddItem(m_itemToGive);
			}
		}
	}
}
