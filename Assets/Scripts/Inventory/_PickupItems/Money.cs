using UnityEngine;
using System.Collections;

public class Money : PickupItem 
{
	private Wallet playerWallet;

	public Money()
	{
		m_canRespawn = false;
	}
}
