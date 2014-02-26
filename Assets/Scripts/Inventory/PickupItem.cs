using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupItem : InventoryItem
{
	public int myValue;
	protected bool bCanRespawn;
	protected float respawnTime;
	
	protected virtual void DestroyAndRespawn()
	{
		this.renderer.enabled = false;
		this.collider.enabled = false;
		StartCoroutine("CoolDown", respawnTime);
	}
	
	protected virtual void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
//			inventory = other.GetComponent<Inventory>();
			Collect();
		}
	}
	
	protected IEnumerator CoolDown(float coolDownTime)
	{
		yield return new WaitForSeconds(coolDownTime);
		this.renderer.enabled = true;
		this.collider.enabled = true;
	}
	
	protected virtual void Collect()
	{
		this.renderer.enabled = false;
		AddToInventory(inventory);
		Destroy(this.gameObject);
	}
}
