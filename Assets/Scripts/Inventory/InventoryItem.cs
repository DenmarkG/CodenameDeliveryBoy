using UnityEngine;
using System.Collections;

public class InventoryItem
{
	#region Serialized Private Variables

	[SerializeField]
	protected string m_itemName = "";

	#endregion

	protected Texture2D m_itemImage = null;
	protected Inventory m_inventory; // the player inventory to add to upon contact. 
	protected bool m_bIsConsistent = false;
	
	public virtual string Print()
	{
		return m_itemName;
	}
	
	public bool IsConsistent
	{
		get { return m_bIsConsistent; }
	}

	public Texture2D ItemImage
	{
		get 
		{ 
			if (m_itemImage == null)
            {
                if (m_itemName != "")
                {
                    m_itemImage = Resources.Load<Texture2D>("ItemTextures/T_" + m_itemName);
                }
            }

            if (m_itemImage != null)
            {
                return m_itemImage;
            }
            else
            {
                return new Texture2D(32, 32);
            }
		}
	}

	public string ItemName
	{
		get { return m_itemName; }
	}
}
