using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour
{
	[SerializeField]	
	private string m_myName = "";
	
	protected Inventory inventory; // the player inventory to add to upon contact. 
	protected bool bIsConsistent = false;
	
	void Awake()
	{
		OnAwake();
	}
	
	//Methods
	public virtual void AddToInventory(Inventory pInventory)
	{
		inventory = pInventory;
		inventory.AddItem(this);
	}
	
	public virtual string Print()
	{
		return m_myName;
	}

	protected virtual void OnAwake()
	{
		//
	}

	public virtual void DropItem()
	{
		//
	}
	
	public bool IsConsistent
	{
		get { return bIsConsistent; }
	}
}
