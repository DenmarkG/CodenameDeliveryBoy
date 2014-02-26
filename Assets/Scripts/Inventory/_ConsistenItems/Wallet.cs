using UnityEngine;
using System.Collections;

public class Wallet : InventoryItem 
{
	public int maxCapacity = 250;
	
	private int currentAmount = 55;
	
	protected override void OnAwake()
	{
		bIsConsistent = true;
	}
	
	public void AddMoney(int amount)
	{
		currentAmount += amount;
		if(currentAmount > maxCapacity)
		{
			currentAmount = maxCapacity;
		}
	}
	
	public bool PayMoney(int amount)
	{
		if(currentAmount - amount < 0)
		{
			return false;
		}
		else
		{
			currentAmount -= amount;
			return true;
		}
	}
	
	public override string Print()
	{
		return currentAmount.ToString("C");
	}
	
}
