using UnityEngine;
using System.Collections;

public class Money : PickupItem 
{
	private Wallet playerWallet;
	
	
	protected override void OnAwake()
	{
		bCanRespawn = false;
	}
	
	protected override void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			playerWallet = other.GetComponent<Wallet>();
			if(inventory != null)
			{
				playerWallet.AddMoney(myValue);
				Collect();
			}
		}
	}
}
