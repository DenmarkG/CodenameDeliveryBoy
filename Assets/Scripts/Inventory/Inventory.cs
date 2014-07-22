using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public delegate void AddInventoryItemDelegate();

[System.Serializable]
public class Inventory  
{
	#region Private serialized variables

	[SerializeField]
	private List<InventoryItem> m_inventoryItems = new List<InventoryItem>();
	
	// //Tuneable variables for changing the way the inventory is displayed on screen
	[SerializeField]
	private int m_inventoryDisplayRows = 2;
	[SerializeField]
	private int m_inventoryDisplayColumns = 3;
	[SerializeField]
	private int m_iconSize = 50;

	#endregion

	#region private unserialzed variables

	//the total items will be based on the display numbers
	private int m_maxItemCount = 0;

	private bool m_isVisible = false;

	#endregion

	public Inventory()
	{
		m_maxItemCount = m_inventoryDisplayColumns * m_inventoryDisplayRows;
	}
	
	public void AddItem(InventoryItem newItem)
	{
		if(m_inventoryItems.Count < m_maxItemCount - 1)
		{
			m_inventoryItems.Add(newItem);
		}

		GuiManager.DisplayStatusMessage(newItem.ItemName + " Added!");
	}

	public void RemoveItem(InventoryItem itemToRemove)
	{
		if(m_inventoryItems.Contains(itemToRemove) )
	   	{
			m_inventoryItems.Remove(itemToRemove);
		}
	}

	public void ToggleInventoryDisplay()
	{
		if (m_isVisible)
			GuiManager.OnUpdateGUI -= DisplayInventory;
		else
			GuiManager.OnUpdateGUI += DisplayInventory;

		m_isVisible = !m_isVisible;
	}

	void DisplayInventory()
	{
		int m_displayStartPos_X = (Screen.width / 2) - ( (m_inventoryDisplayColumns * m_iconSize) / 2);
		int m_displayStartPos_Y = (Screen.height / 2) - ( (m_inventoryDisplayRows * m_iconSize) / 2);

		for(int gridY = 0; gridY < m_inventoryDisplayRows; gridY++)
		{
			for(int gridX = 0; gridX < m_inventoryDisplayColumns; gridX++)
			{
				int positionIndex = (gridY * m_inventoryDisplayColumns) + gridX;
				InventoryItem currentItem = null;
				if (!(positionIndex >= m_inventoryItems.Count) )
					currentItem = m_inventoryItems[positionIndex];

				//create the rectangle
				Rect displayRect = new Rect( m_displayStartPos_X + (m_iconSize * gridX), m_displayStartPos_Y + (m_iconSize * gridY), m_iconSize, m_iconSize);

				//Draw the box
				if(currentItem != null)
				{
					GUI.Box(displayRect, currentItem.ItemImage);
				}
				else
				{
					GUI.Box(displayRect, "");
				}
			}
		}
	}

	public bool Contains(InventoryItem item)
	{
		return m_inventoryItems.Contains(item);
	}
}