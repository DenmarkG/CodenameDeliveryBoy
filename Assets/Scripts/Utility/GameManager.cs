﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;

    private static Character_Player m_player = null;

    // True if running on windows or web player
    private bool m_isWindows = true;

    public delegate void Pause();
    public static event Pause OnPause;

    public delegate void Unpause();
    public static event Unpause OnUnPause;

    private bool m_isPaused = false;

    private Clock m_clock = null;

    private static DialogManager m_dialogManager = null;

    private bool m_bIsUsingController = false;

    void Awake()
    {
        // Determine the current platform
        m_isWindows = ((Application.platform == RuntimePlatform.WindowsPlayer) || (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsWebPlayer) ? true : false);
        m_bIsUsingController = Input.GetJoystickNames().Length != 0;
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_Player>();
        m_dialogManager = new DialogManager(m_player);
        m_instance = this;
    }

    void Start()
    {
        m_clock = Clock.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TogglePause();
        }

        if (!m_player.GetComponent<Health>().IsAlive)
        {
            // end the game
            EndGame();
        }
    }

    private void OnDestroy()
    {
        OnPause = null;
        OnUnPause = null;
    }

    public static void EndGame(float timeBeforeReload = 5f)
    {
        m_instance.StartCoroutine(m_instance.ResetLevel(timeBeforeReload));
    }

    public IEnumerator ResetLevel(float timeBeforeReload = 5f)
    {
        GuiManager.OnUpdateGUI += m_instance.DisplayEndGameText;

        yield return new WaitForSeconds(timeBeforeReload);

        MissionHash.Instance.ResetMisisons();

        Application.LoadLevel(Application.loadedLevel);
    }

    private void DisplayEndGameText()
    {
        if (!m_player.GetComponent<Health>().IsAlive)
        {
            GUI.Box(new Rect((Screen.width / 2) - (150 / 2), (Screen.height / 2), 150, 25), "You Lose");
        }
        else
        {
            GUI.Box(new Rect((Screen.width / 2) - (150 / 2), (Screen.height / 2), 150, 25), "You Win");
        }
    }

    void TogglePause()
    {
        if (m_isPaused)
        {
            m_clock.StartClock();
            ResumeGame();
        }
        else
        {
            m_clock.PauseClock();
            PauseGame();
        }

        m_isPaused = !m_isPaused;
    }

    void PauseGame()
    {
        if (OnPause != null)
            OnPause();
    }

    void ResumeGame()
    {
        if (OnUnPause != null)
            OnUnPause();
    }

    public bool IsPaused
    {
        get { return m_isPaused; }
    }

    public static GameManager Instance
    {
        get { return m_instance; }
    }

    public static DialogManager GetDialogManager
    {
        get { return m_dialogManager; }
    }

    public static bool IsUsingController
    {
        get { return m_instance.m_bIsUsingController; }
    }

    public static bool IsWindows
    {
        get { return m_instance.m_isWindows; }
    }

    public static Character_Player Player
    {
        get { return m_player; }
    }
}

