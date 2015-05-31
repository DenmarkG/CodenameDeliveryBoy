using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupItem : InventoryItem
{
	[SerializeField]
	protected int m_myValue;

	protected bool m_canRespawn;
	protected float m_respawnTime;
	
	public virtual IEnumerator CoolDown(GameObject itemObject, float coolDownTime)
	{
		yield return new WaitForSeconds(coolDownTime);
		itemObject.GetComponent<Renderer>().enabled = true;
		itemObject.GetComponent<Collider>().enabled = true;
	}
	
	public virtual void Collect(GameObject itemObject)
	{
		itemObject.GetComponent<Renderer>().enabled = false;
		//AddToInventory(inventory);
		GameObject.Destroy(itemObject);
	}
}
