using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void AddInventoryItemDelegate();

public class Inventory  
{
	private int maxItemCount = 5;

	public int inventoryDisplayRows = 2;
	public int inventoryDisplayColumns = 3;
	public int buttonSize = 15;
	
	public List<InventoryItem> inventoryItems = null;
	
	private int itemCount = 0;

	public Inventory()
	{
		inventoryItems = new List<InventoryItem>();
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
		if(itemCount < maxItemCount - 1)
		{
			inventoryItems.Add(newItem);
			++itemCount;
		}
	}
	
	public string PrintInventory()
	{	
		string items = string.Format("Total Items: {0}\nItems:", itemCount);
		foreach(InventoryItem i in inventoryItems)
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
}