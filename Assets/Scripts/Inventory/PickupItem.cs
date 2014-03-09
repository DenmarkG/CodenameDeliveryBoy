using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupItem : InventoryItem
{
	public int m_myValue;
	protected bool m_canRespawn;
	protected float m_respawnTime;
	
	protected virtual void DestroyAndRespawn()
	{
		this.renderer.enabled = false;
		this.collider.enabled = false;
		StartCoroutine("CoolDown", m_respawnTime);
	}
	
	protected virtual void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
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
