﻿using UnityEngine;
using System.Collections;

public class MissionTestChar_02 : MonoBehaviour 
{
	private InventoryItem m_itemToGive = MissionHash.mission_01.Item;

	private Mission mission = MissionHash.mission_01;

    private void Start()
    {
        // Update the number of food storage location to include this one
        ((Mission_01)mission).AddFoodStore();
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			if(mission.GetMissionState == Mission.MissionState.IN_PROGRESS)
			{
				Character_Player player = other.gameObject.GetComponent<Character_Player>();
                //if(!player.PlayerInventory.Contains(m_itemToGive) )
                if (m_itemToGive != null)
                {
                    player.PlayerInventory.AddItem(m_itemToGive);
                    m_itemToGive = null;
                }
					
			}
		}
	}
}
