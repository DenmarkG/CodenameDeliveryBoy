using UnityEngine;
using System.Collections;

public class Food : PickupItem 
{
	protected override void OnAwake()
	{
		bCanRespawn = false;
	}
	
	protected override void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
//			inventory = other.GetComponent<Inventory>();
//			if(inventory != null)
//			{
//				inventory.AddItem(this);
//				Debug.Log ("Collected Food");
//				Collect();
//			}
		}
	}
}