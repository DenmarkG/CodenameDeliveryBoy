using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuiManager : MonoBehaviour 
{
	#region Public Variables

	public delegate void UpdateGUI();
	public static event UpdateGUI OnUpdateGUI;

	float m_hSliderValue = 1f;

	#endregion

	#region Private Serialized Variables

	[SerializeField]
	private GUISkin m_Skin = null;
	[SerializeField]
	private float m_statusBoxWidth = 300f;
	[SerializeField]
	private float m_statusBoxHeight = 50f;
	[SerializeField]
	private float m_statusMessageDisplayTime = 15f;

	#endregion

	#region Private Variables

	private Health m_playerHealth;
	private List<ScreenMask> m_screenMasks = new List<ScreenMask>();

	//variables for displaying status messages
	private static Rect m_statusBox;
	private static List<string> m_statusMessages = new List<string>();
	private string m_currentStatusMessage = "";


	//create a static instance
	private static GuiManager m_instance;

	#endregion

	#region Callbacks

	void Awake()
	{
		m_statusBox = new Rect (0, Screen.height - m_statusBoxHeight, m_statusBoxWidth, m_statusBoxHeight);
		m_instance = this;
	}

	public void OnGUI()
	{
		GUI.skin = m_Skin;

		if (OnUpdateGUI != null)
		{
			OnUpdateGUI();
		}

		if (m_currentStatusMessage != "")
		{
			GUI.Box(m_statusBox, m_currentStatusMessage);
		}

		if (m_screenMasks.Count != 0)
		{
			foreach (ScreenMask mask in m_screenMasks)
			{
				mask.Draw();
			}
		}

		//time scale test
//		m_hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), m_hSliderValue, 0, 20f);
//		Clock.TimeScale = m_hSliderValue;
	}

	#endregion

	#region Custom Public Methods

	public static void DisplayStatusMessage (string messageToDisplay)
	{
		m_statusMessages.Add(messageToDisplay);
		m_instance.StartCoroutine("CountDownStatusMessageTimer", m_instance.m_statusMessageDisplayTime);
	}

	private IEnumerator CountDownStatusMessageTimer (float countTimeInSeconds)
	{
		float currentTime = 0;
		m_currentStatusMessage = m_statusMessages[0];

		while (currentTime <= countTimeInSeconds)
		{
			currentTime += Clock.DeltaTime;
			yield return null;
		}

		//remove the first status message in the list
		m_statusMessages.Remove (m_currentStatusMessage);

		//assign the next message to the status box if there is one
		if (m_statusMessages.Count > 0)
		{
			m_currentStatusMessage = m_statusMessages[0];
			m_instance.StartCoroutine("CountDownStatusMessageTimer", m_instance.m_statusMessageDisplayTime);
		}
		else
		{
			m_currentStatusMessage = "";
		}
	}

	#endregion

	#region Properties

	public static Rect GetStatusBoxRect
	{
		get { return m_statusBox; }
	}

	#endregion
}
