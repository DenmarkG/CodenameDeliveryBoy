using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager m_instance = null;

	public delegate void Pause();
	public static event Pause OnPause;

	public delegate void Resume();
	public static event Resume OnResume;

	private bool m_isPaused = false;

	private Clock m_clock = null;

	void Awake()
	{
		m_instance = this;
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
}

