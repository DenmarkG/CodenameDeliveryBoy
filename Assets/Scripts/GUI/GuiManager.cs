using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class GuiManager : MonoBehaviour
{
    #region Public Variables

    public delegate void UpdateGUI();
    public static event UpdateGUI OnUpdateGUI;

    // When testing the timescale, uncomment this
    //float m_hSliderValue = 1f;

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
    //private List<ScreenMask> m_screenMasks = new List<ScreenMask>();

    //variables for displaying status messages
    private static Rect m_statusBox;
    private static List<string> m_statusMessages = new List<string>();
    private string m_currentStatusMessage = "";

    // variables for displaying dialog
    private static Rect m_textDialogBox;
    private string m_currentDialogString = "";

    // the message to display when the game is running
    private string m_runningMessage = "";
    private Rect m_runningMessageRect;
    // the message to display when the game is paused
    private string m_pausedMessage = "";
    private Rect m_pausedMessageRect;

    //create a static instance
    private static GuiManager m_instance;

    #endregion

    #region Callbacks

    void Awake()
    {
        m_statusBox = new Rect(0, Screen.height - m_statusBoxHeight, m_statusBoxWidth, m_statusBoxHeight);
        m_textDialogBox = new Rect((Screen.width - m_textDialogBoxWidth) / 2, Screen.height - m_textDialogBoxHeight, m_textDialogBoxWidth, m_textDialogBoxHeight);

        m_instance = this;
    }

    void Start()
    {
        // If running on windows, setup the status pause messages
        if (GameManager.IsWindows)
        {
            InitPauseMessages();
        }
    }

    public void OnGUI()
    {
        GUI.skin = m_Skin;

        if (!Clock.IsPaused)
        {
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

            if (GameManager.IsWindows)
            {
                // Tell the player how to get to the pause menu
                GUI.Box(m_runningMessageRect, m_runningMessage);
            }

            //time scale test
            //m_hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), m_hSliderValue, 0, 20f);
            //Clock.TimeScale = m_hSliderValue;
        }
        else
        {

            DiplayControlsOnWindows();
        }
    }

    #endregion

    #region Custom Public Methods

    public static void DisplayStatusMessage(string messageToDisplay)
    {
        m_statusMessages.Add(messageToDisplay);
        m_instance.StartCoroutine(m_instance.CountDownStatusMessageTimer(m_instance.m_statusMessageDisplayTime));
    }

    public static void ShowDialog(string dialogToDisplay, Action<bool> EndCallback)
    {
        m_instance.StartCoroutine(m_instance.PrintDialog(dialogToDisplay, EndCallback));
    }

    #endregion

    #region Private Methods

    private IEnumerator PrintDialog(string dialogToDisplay, Action<bool> EndCallback)
    {
        //this variable holds the position of the next character that will be shown to the screen
        StringBuilder builder = new StringBuilder(dialogToDisplay.Length + 1);

        for (int letterIndex = 0; letterIndex < dialogToDisplay.Length; ++letterIndex)
        {
            if (!Clock.IsPaused)
            {
                builder.Append(dialogToDisplay[letterIndex]);
                m_currentDialogString = builder.ToString();

                //This if check will allow the player to skip to the end of the dialog 
                if (Input.GetKey(KeyCode.Space) || Input.GetAxis(GameControllerHash.Buttons.A) != 0)
                {
                    m_currentDialogString = dialogToDisplay;
                    letterIndex = dialogToDisplay.Length;
                }

                yield return new WaitForSeconds(m_textDelay);
            }
            else
            {
                yield return null;
            }
        }

        //add a timer to prevent the player from skipping past the end of the dialog
        yield return new WaitForSeconds(m_endTextDelay);


        // Continue looping here until the player presses the button to escape the conversation
        while (!(Input.GetKey(KeyCode.Space) || Input.GetAxis(GameControllerHash.Buttons.A) != 0))
        {
            yield return null;
        }

        // clear the dialog
        m_currentDialogString = "";

        // execute the callback if there is one
        if (EndCallback != null)
        {
            EndCallback(true);
        }

        // Force garbage collection since I just created about a hundred strings (character arrays)
        GC.Collect();
    }

    private IEnumerator CountDownStatusMessageTimer(float countTimeInSeconds)
    {
        float currentTime = 0;
        m_currentStatusMessage = m_statusMessages[0];

        while (currentTime <= countTimeInSeconds)
        {
            currentTime += Clock.DeltaTime;
            yield return null;
        }

        //remove the first status message in the list
        m_statusMessages.Remove(m_currentStatusMessage);

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

    // For showing the control setup on screen
    private void DiplayControlsOnWindows()
    {
        // Show the control setup
        GUI.Box(m_pausedMessageRect, m_pausedMessage);
    }

    private void InitPauseMessages()
    {
        m_runningMessage = "Press RETURN to Pause";
        m_runningMessageRect = new Rect(15, 25, 200, 25);
        m_pausedMessage = "Paused\nWASD OR Arrow keys = Move\nSHIFT = Run\n1 = Toggle Mission Status\n2 = Toggle Inventory\nF = Enter orbit camera mode.";
        m_pausedMessageRect = new Rect((Screen.width / 2) - 300, (Screen.height / 2) - 150, 600, 300);
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
