using UnityEngine;
using System.Collections;

public class Food : PickupItem 
{
	public Food()
	{
		m_canRespawn = false;
		m_myName = "Groceries";
		m_itemImage = Resources.Load<Texture2D> ("ItemTextures/T_Food");
	}
}