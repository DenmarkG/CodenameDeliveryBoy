using UnityEngine;
using System.Collections;

public class GuiManager : MonoBehaviour 
{
	public GUISkin mySkin = null;
	
	//private string inventoryString;
	private Health playerHealth;
	//private Inventory playerInventory;
	
	void Start()
	{
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
	}
	
	void Update()
	{
		//inventoryString = playerInventory.PrintInventory();
	}
	void OnGUI()
	{
		//inventoryString = GUI.TextArea(new Rect(Screen.width - 105, 5, 100, 150),  inventoryString);
		GUI.Box(new Rect(5, 5, 50, 25), playerHealth.CurrentHealth.ToString(), mySkin.box);
		//GUI.Box(new Rect(Screen.width - 105, 5, 100, 150),  inventoryString, mySkin.box);
	}
}
