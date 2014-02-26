using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	public float maxHealth;
	private float currentHealth;
	
	void Awake()
	{
		currentHealth = maxHealth;
	}
	
	public void RecoverHealth(float amount)
	{
		currentHealth += amount;
		if(currentHealth > maxHealth)
			currentHealth = maxHealth;
	}
	
	public void TakeDamage(float amount)
	{
		currentHealth -= amount;
		if(currentHealth < 0)
			currentHealth = 0;
	}
	
	public float CurrentHealth
	{
		get {return currentHealth; }
	}
}
