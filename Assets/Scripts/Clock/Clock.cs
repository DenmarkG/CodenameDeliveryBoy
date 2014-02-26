using UnityEngine;
using System;
using System.Collections;

public class Clock : MonoBehaviour 
{
	[SerializeField]
	private int dayStartTime = 900;	//Time to start the day in Military Time (i.e. 0900 = 9:00 am, 1700 = 5:00pm)
	
	private float currentSeconds = 0f;
	private int currentMinutes = 0;
	private int currentHours  = 0;
	private string currentTime = "";
	private float myTimeScaleStartValue = 0;
	private Day currentDay = Day.MONDAY;

	/// <summary>
	/// values greater than one will speed up time, 
	/// values lower than one will slow time
	/// a value of 1 returns speed to normal;
	/// a value of 0 pauses time
	/// </summary>
	private static float m_timeScale = 1f;

	private static Clock instance = null;

	void Awake()
	{
		currentHours = (int) (dayStartTime / 100);
		currentMinutes = (int) (dayStartTime % 100);
		myTimeScaleStartValue = m_timeScale;
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			PauseClock();
		}
		
		if(Input.GetKeyDown(KeyCode.T))
		{
			StartClock();
		}
	}
	
	void OnGUI()
	{
		currentTime = UpdateTime();
		GUI.Box(new Rect((Screen.width / 2) - 75, 5, 150, 25),  currentDay.ToString() + " " + currentTime);
	}
	
	string UpdateTime()
	{
		//increase the seconds
		currentSeconds += Time.deltaTime * m_timeScale; //if the timescale is zero, then time won't increase
		
		if(currentSeconds > 59)
		{
			//if the seconds are greater than 59, add a minute
			currentMinutes += 1;
			
			if(currentMinutes > 59)
			{
				//if the minutes are greater than 59, add an hour
				currentHours += 1;
				
				if(currentHours > 23)
				{
					//if the hours are greater than 23, add a day, and reset the time
					currentHours = 0; 
					GoToNextDay();
				}
				
				//reset the minutes after the increase
				currentMinutes = 0; 
			}
			
			//reset the seconds after the increase
			currentSeconds = 0;
		}

		//return the time as a formatted string
		return string.Format("{0:D2}:{1:D2}:{2:D2}", currentHours, currentMinutes, (int)currentSeconds);
	}
	
	//method to increment days of the week
	void GoToNextDay()
	{
		if(currentDay == Day.SATURDAY)
		{
			currentDay = Day.SUNDAY;
		}
		else
		{
			currentDay += 1; 
		}
	}
	
	//public method for starting the clock externally
	public void StartClock()
	{
		m_timeScale = myTimeScaleStartValue; 
	}

	//public method for pasing the clock externally
	public void PauseClock()
	{
		m_timeScale = 0;
	}
	
	public float GetTime
	{
		get {return (currentHours * 100) + currentMinutes; }
	}

	public static Clock GetClock
	{
		get { return instance; }
	}
	
	private enum Day //day of the week enumerated
	{
		SUNDAY, MONDAY, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SATURDAY
	}

	public static void PauseGame()
	{
		//
	}

	public static float DeltaTime()
	{
		return Time.deltaTime * m_timeScale;
	}
}
