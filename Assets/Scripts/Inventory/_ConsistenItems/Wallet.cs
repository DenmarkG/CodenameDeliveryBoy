using UnityEngine;
using System.Collections;

[System.Serializable]
public class Wallet : InventoryItem 
{
	[SerializeField]
	private int m_maxCapacity = 250;
	
	private int m_currentAmount = 55;

	public Wallet()
	{
		m_myName = "Wallet";
		bIsConsistent = true;
	}
	
	public void AddMoney(int amount)
	{
		m_currentAmount += amount;
		if(m_currentAmount > m_maxCapacity)
		{
			m_currentAmount = m_maxCapacity;
		}
	}
	
	public bool PayMoney(int amount)
	{
		if(m_currentAmount - amount < 0)
		{
			return false;
		}
		else
		{
			m_currentAmount -= amount;
			return true;
		}
	}

	public void IncreaseCapacity(int newMaxCapacity)
	{
		if(newMaxCapacity > m_maxCapacity)
		{
			m_maxCapacity = newMaxCapacity;
		}
	}

	public override string Print()
	{
		return m_currentAmount.ToString("C");
	}
}
