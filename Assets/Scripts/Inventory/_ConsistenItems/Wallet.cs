using UnityEngine;
using System.Collections;

public class Wallet : InventoryItem 
{
	[SerializeField]
	private int m_maxCapacity = 250;
	
	private int m_currentAmount = 55;
	
	protected override void OnAwake()
	{
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
	
	public override string Print()
	{
		return m_currentAmount.ToString("C");
	}
}
