using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour
{
	
	public string myName = "";
	//public Texture2D icon = null;
	
	protected Inventory inventory; // the player inventory to add to upon contact. 
	protected bool bIsConsistent = false;
	
	void Awake()
	{
		OnAwake();
	}
	
	//Methods
	protected virtual void OnAwake()
	{
	}

	public virtual void AddToInventory(Inventory pInventory)
	{
		inventory = pInventory;
		inventory.AddItem(this);
	}
	
	public virtual string Print()
	{
		return myName;
	}
	
	public virtual void DropItem()
	{
	}
	
	public bool IsConsistent
	{
		get { return bIsConsistent; }
	}
}
