using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuiManager : MonoBehaviour 
{
	#region Public Variables

	public delegate void UpdateGUI();
	public static event UpdateGUI OnUpdateGUI;

	#endregion

	#region Private Serialized Variables

	//[SerializeField]
	//private GUISkin m_Skin = null;

	#endregion

	#region Private Variables

	private Health m_playerHealth;
	private List<ScreenMask> m_screenMasks = new List<ScreenMask>();

	#endregion

	public void OnGUI()
	{
		if (OnUpdateGUI != null)
		{
			OnUpdateGUI();
		}

		if (m_screenMasks.Count != 0)
		{
			foreach (ScreenMask mask in m_screenMasks)
			{
				mask.Draw();
			}
		}
	}
}
