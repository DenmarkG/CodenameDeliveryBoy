using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager m_instance = null;

	private static Character_Player m_player = null;

	public delegate void Pause();
	public static event Pause OnPause;

	public delegate void Unpause();
	public static event Resume OnUnPause;

	private bool m_isPaused = false;

	private Clock m_clock = null;

	private static DialogManager m_dialogManager = null;

	private bool m_bIsUsingController = false;

	void Awake()
	{
		m_instance = this;
		m_bIsUsingController = Input.GetJoystickNames().Length != 0;
		m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_Player>();
		m_dialogManager = new DialogManager(m_player);
//		Screen.showCursor = false;
	}

	void Start()
	{
		m_clock = Clock.Instance;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P) )
		{
			TogglePause();
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
		if (OnResume != null)
			OnResume();
	}

	public bool IsPaused
	{
		get {return m_isPaused; }
	}

	public static GameManager Instance
	{
		get {return m_instance; }
	}

	public static DialogManager GetDialogManager
	{
		get { return m_dialogManager; }
	}

	public static bool IsUsingController
	{
		get { return m_instance.m_bIsUsingController; }
	}
}

