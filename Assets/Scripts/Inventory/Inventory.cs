using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public delegate void AddInventoryItemDelegate();

[System.Serializable]
public class Inventory  
{
	[SerializeField]
	private int m_maxItemCount = 5;
//	[SerializeField]
//	private int m_inventoryDisplayRows = 2;
//	[SerializeField]
//	private int m_inventoryDisplayColumns = 3;
//	[SerializeField]
//	private int m_buttonSize = 15;
	[SerializeField]
	private List<InventoryItem> m_inventoryItems = null;

	public Inventory()
	{
		m_inventoryItems = new List<InventoryItem>();
	}
	
//	void OnGUI()
//	{
//		if(Input.GetKeyDown(KeyCode.Q))
//		{
//			ToggleDisplay();
//		}
//		if(bIsVisible)
//		{
//			DisplayInventory();
//		}
//	}
	
	public void AddItem(InventoryItem newItem)
	{
		if(m_inventoryItems.Count < m_maxItemCount - 1)
		{
			m_inventoryItems.Add(newItem);
		}
	}

	public void RemoveItem(InventoryItem itemToRemove)
	{
		if(m_inventoryItems.Contains(itemToRemove) )
	   	{
			m_inventoryItems.Remove(itemToRemove);
		}
	}
	
	public string PrintInventory()
	{	
		string items = string.Format("Total Items: {0}\nItems:", m_inventoryItems.Count);
		foreach(InventoryItem i in m_inventoryItems)
		{
			items += "\n" + i.Print();
		}
		return items;
	}
	void ToggleDisplay()
	{
//		if(bIsVisible)
//		{
//			bIsVisible = false;
//		}
//		else
//		{
//			bIsVisible = true;
//		}
	}
	
	public void DisplayInventory()
	{
//		for(int gridY = 0; gridY < inventoryDisplayRows; gridY++)
//		{
//			for(int gridX = 0; gridX < inventoryDisplayColumns; gridX++)
//			{
//				int positionIndex = (gridY * inventoryDisplayColumns) + gridX;
//				string itemDisplayString = "";
//				
//				if(inventoryItems[positionIndex] != null)
//				{
//					itemDisplayString = inventoryItems[positionIndex].Print();
//				}
//				
//				
//				if(GUI.Button(new Rect(5, Screen.height - 105, 100, 100), itemDisplayString))
//				{
//					if(inventoryItems[positionIndex] != null && !inventoryItems[positionIndex].IsConsistent)
//					{
//						inventoryItems[positionIndex].DropItem();
//					}
//				}
//			}
//		}
	}

	public bool Contains(InventoryItem item)
	{
		return m_inventoryItems.Contains(item);
	}
}