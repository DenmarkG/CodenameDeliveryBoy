using UnityEngine;
using System;
using System.Collections;

public class Clock : MonoBehaviour
{
    #region private variables

    [SerializeField]
    private int m_dayStartTime = 900;	//Time to start the day in Military Time (i.e. 0900 = 9:00 am, 1700 = 5:00pm)

    private float currentSeconds = 0f;
    private int currentMinutes = 0;
    private int currentHours = 0;
    private float myTimeScaleStartValue = 0;
    private Day m_currentDay = Day.MONDAY;
    private string m_timeString = "";

    //private SkyBoxBehavior m_skybox = null;
    private SkyboxTransition m_skybox = null;

    /// <summary>
    /// values greater than one will speed up time, 
    /// values lower than one will slow time
    /// a value of 1 returns speed to normal;
    /// a value of 0 pauses time
    /// </summary>
    private static float m_timeScale = 1f;

    private static Clock m_instance = null;

    #endregion

    #region Unity Callbacks

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }

        currentHours = (int)(m_dayStartTime / 100);
        currentMinutes = (int)(m_dayStartTime % 100);
        myTimeScaleStartValue = m_timeScale;

        //m_skybox = this.gameObject.AddComponent<SkyBoxBehavior>();
        //m_skybox.SetSkyBoxes(m_dayStartTime);
        m_skybox = this.gameObject.AddComponent<SkyboxTransition>();
        m_skybox.InitSkyboxTransition(currentHours);
    }

    void Start()
    {
        GuiManager.OnUpdateGUI += DrawClock;
    }

    void Update()
    {
        UpdateTime();
    }


    #endregion

    #region Private Methods

    void DrawClock()
    {
        GUI.Box(new Rect((Screen.width / 2) - 75, 5, 150, 25), m_currentDay.ToString() + " " + m_timeString);
    }

    void UpdateTime()
    {
        //increase the seconds
        currentSeconds += Time.deltaTime * m_timeScale; //if the timescale is zero, then time won't increase

        if (currentSeconds > 59)
        {
            //if the seconds are greater than 59, add a minute
            currentMinutes += 1;

            if (currentMinutes > 59)
            {
                //if the minutes are greater than 59, add an hour
                currentHours += 1;

                //update the time of day on the skybox behavior
                //m_skybox.UpdateTimeOfDay(currentHours);
                m_skybox.UpdateSkybox(currentHours);

                if (currentHours > 23)
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
        m_timeString = string.Format("{0:D2}:{1:D2}:{2:D2}", currentHours, currentMinutes, (int)currentSeconds);
    }

    //method to increment days of the week
    void GoToNextDay()
    {
        if (m_currentDay == Day.SATURDAY)
        {
            m_currentDay = Day.SUNDAY;
        }
        else
        {
            m_currentDay += 1;
        }
    }

    #endregion

    #region Public Methods

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

    //	public static IEnumerator Timer(float time, Action<object> CallBackFunc, object functionParam)
    //	{
    //		CallBackFunc(functionParam);
    //		yield return null;
    //	}

    #endregion

    #region Properties

    public static float GetTime
    {
        get { return (m_instance.currentHours * 100) + m_instance.currentMinutes; }
    }

    public static Clock Instance
    {
        get { return m_instance; }
    }

    public static float TimeDelta
    {
        get { return m_timeScale * Time.deltaTime; }
    }

    public static float TimeScale
    {
        get { return m_timeScale; }
        set { m_timeScale = value; }
    }

    public static float DeltaTime
    {
        get { return Time.deltaTime * m_timeScale; }
    }

    public static bool IsPaused
    {
        get { return m_timeScale == 0; }
    }

    #endregion

    private enum Day //day of the week enumerated
    {
        SUNDAY, MONDAY, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SATURDAY
    }
}
