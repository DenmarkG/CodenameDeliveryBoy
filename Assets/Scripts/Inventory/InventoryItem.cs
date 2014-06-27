using UnityEngine;
using System.Collections;

public class InventoryItem
{
	#region Serialized Private Variables

	[SerializeField]
	protected string m_myName = "";

	#endregion

	protected Texture2D m_itemImage = null;
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

	public Texture2D ItemImage
	{
		get { return m_itemImage; }
	}

	public string Name
	{
		get { return m_myName; }
	}
}
