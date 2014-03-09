using UnityEngine;
using System.Collections;

public class Food : PickupItem 
{
	public Food()
	{
		m_canRespawn = false;
		m_myName = "Groceries";
	}
}