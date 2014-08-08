﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GuiManager : MonoBehaviour 
{
	#region Public Variables

	public delegate void UpdateGUI();
	public static event UpdateGUI OnUpdateGUI;

//	float m_hSliderValue = 1f;

	#endregion

	#region Private Serialized Variables

	[SerializeField]
	private GUISkin m_Skin = null;
	[SerializeField]
	private float m_statusBoxWidth = 300f;
	[SerializeField]
	private float m_statusBoxHeight = 50f;
	[SerializeField]
	private float m_statusMessageDisplayTime = 3f;
	[SerializeField]
	private float m_textDialogBoxWidth = 350f;
	[SerializeField]
	private float m_textDialogBoxHeight = 150f;
	[SerializeField]
	private float m_textDelay = .1f;
	[SerializeField]
	private float m_endTextDelay = 1.5f;

	#endregion

	#region Private Variables

	private Health m_playerHealth;
	private List<ScreenMask> m_screenMasks = new List<ScreenMask>();

	//variables for displaying status messages
	private static Rect m_statusBox;
	private static List<string> m_statusMessages = new List<string>();
	private string m_currentStatusMessage = "";

	//variables for displaying dialog
	private static Rect m_textDialogBox;
	private string m_currentDialogString = "";


	//create a static instance
	private static GuiManager m_instance;

	#endregion

	#region Callbacks

	void Awake()
	{
		m_statusBox = new Rect (0, Screen.height - m_statusBoxHeight, m_statusBoxWidth, m_statusBoxHeight);
		m_textDialogBox = new Rect( (Screen.width - m_textDialogBoxWidth) / 2, Screen.height - m_textDialogBoxHeight, m_textDialogBoxWidth, m_textDialogBoxHeight);
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

		if (m_currentDialogString != "")
		{
			GUI.Box(m_textDialogBox, m_currentDialogString);
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
		m_instance.StartCoroutine(m_instance.CountDownStatusMessageTimer(m_instance.m_statusMessageDisplayTime) );
	}

	public static void ShowDialog(string dialogToDisplay, Action<bool> EndCallback)
	{
		m_instance.StartCoroutine(m_instance.PrintDialog (dialogToDisplay, EndCallback) );
	}

	#endregion

	#region Private Methods

	//[#todo] move this to the Dialog Manager
	private IEnumerator PrintDialog(string dialogToDisplay, Action<bool> EndCallback)
	{
		//this variable holds the position of the next character that will be shown to the screen
		int letterIndex = 0;

		//until the string passed in is not equal to the one that is shown on screen, we will continue to add letters to it
		while (m_currentDialogString != dialogToDisplay)
		{
			//The letters should stop printing when the game is paused, then resume when no longer paused
			if (!Clock.IsPaused)
			{
				m_currentDialogString += dialogToDisplay[letterIndex];
				++letterIndex;

				//This if check will allow the player to skip through the dialog faster
				if (Input.GetKey(KeyCode.Space) || Input.GetAxis(GameControllerHash.Buttons.A) != 0 )
				{
					m_currentDialogString = dialogToDisplay;
					letterIndex = dialogToDisplay.Length;
				}

				yield return new WaitForSeconds(m_textDelay);
			}
			else
			{
				//if the game is paused, simply skip adding letters to the displayed string
				yield return null;
			}
		}

		//add a timer to prevent the player from skipping past the end of the dialog
		yield return new WaitForSeconds(m_endTextDelay);


		//once the strings are equal, continue displaying the dialog until the player exits the conversation
		while(m_currentDialogString == dialogToDisplay)
		{
			//once the player presses the button to escape the conversation, clear the dialog and exit this loop
			if (Input.GetKey(KeyCode.Space) || Input.GetAxis(GameControllerHash.Buttons.A) != 0 )
			{
				m_currentDialogString = "";
				break;
			}
			yield return null;
		}

		if (EndCallback != null)
			EndCallback (true);
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

	public static Rect TextDialogBox
	{
		get { return m_textDialogBox; }
	}
	#endregion
}
