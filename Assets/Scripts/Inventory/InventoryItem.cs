using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem
{
	[SerializeField]	
	protected string m_myName = "";
	
	protected Inventory inventory; // the player inventory to add to upon contact. 
	protected bool bIsConsistent = false;
	
	public virtual string Print()
	{
		return m_myName;
	}
	
	public bool IsConsistent
	{
		get { return bIsConsistent; }
	}
}
