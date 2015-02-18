using UnityEngine;
using System.Collections;

public class Mission_01 : Mission
{
	private InventoryItem m_food = null;

	private Character_Player m_player;

    // The total number of food storage locations in the scene
    private int m_totalFoodStores = 0;

	public Mission_01()
	{
		m_missionName = "Get Food Supplies";
		m_missionDescription = "Get the food supplies from the green cube, and deliver to the blue cube.";
		m_food = new Food();
	}

	public override void BeginMission ()
	{
		m_currenState = MissionState.IN_PROGRESS;
		m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_Player>();
	}

	public override bool CheckForCompletion()
	{
		return m_player.PlayerInventory.Contains(m_food);
	}

	public override void EndMission ()
	{
		m_currenState = MissionState.SUCCESS;
		m_player.PlayerMissionManager.UpdateMission(this);
        //m_player.PlayerInventory.RemoveItem(m_groceries);

        Inventory inventory = m_player.PlayerInventory;

        // find the total number of food storage locations found
        int foodStoresFound = 0;
        for (int i = 0; i < inventory.Count; ++i)
        {
            if (inventory.GetItemByIndex(i) == m_food)
            {
                ++foodStoresFound;
            }
        }

        // report the number found
		GuiManager.DisplayStatusMessage("Mission Complete!\n\tFound " + foodStoresFound + " / " + m_totalFoodStores);

        // End the game
        GameManager.EndGame();
	}

	public InventoryItem Item
	{
		get { return m_food; }
	}

    public void AddFoodStore()
    {
        ++m_totalFoodStores;
    }
}
