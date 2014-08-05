using UnityEngine;
using System.Collections;

public class Food : PickupItem 
{
	public Food()
	{
		m_canRespawn = false;
		m_itemName = "Food";

		//this line was causing errors in the editor
//		m_itemImage = Resources.Load<Texture2D> ("ItemTextures/T_Food");
	}
}