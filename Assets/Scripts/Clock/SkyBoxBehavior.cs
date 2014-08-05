using UnityEngine;
using System.Collections;

public class SkyBoxBehavior : MonoBehaviour
{
	private Material m_daySkyBox = null;
	private Material m_dawnDuskSkyBox = null;
	private Material m_nightSkyBox = null;
	
	private Material m_currentSky = null;
	Material m_nextSky = null;
	
	private TimeOfDay m_currentTimeOfDay = TimeOfDay.INVALID;
	
	private float m_transitionTime = 50f;
	
	public void SetSkyBoxes(float pHours)
	{
		m_daySkyBox = Resources.Load<Material> ("Skyboxes/Sunny2 Skybox");
		m_dawnDuskSkyBox = Resources.Load<Material>("Skyboxes/Dusk1");
		m_nightSkyBox = Resources.Load<Material>("Skyboxes/StarryNightSkybox");
		
		if (pHours > 5 && pHours <= 7)
		{
			m_currentTimeOfDay = TimeOfDay.DAWN;
			m_currentSky = m_dawnDuskSkyBox;
		}
		else if (pHours > 7 && pHours <= 18)
		{
			m_currentTimeOfDay = TimeOfDay.DAY;
			m_currentSky = m_daySkyBox;
		}
		else if (pHours > 18 && pHours <= 20)
		{
			m_currentTimeOfDay = TimeOfDay.DUSK;
			m_currentSky = m_dawnDuskSkyBox;
		}
		else
		{
			m_currentTimeOfDay = TimeOfDay.NIGHT;
			m_currentSky = m_nightSkyBox;
		}
		
		RenderSettings.skybox = m_currentSky;
	}
	
	public void UpdateTimeOfDay(float pHours)
	{
		if ( (pHours >= 5 && pHours < 7) && ( m_currentTimeOfDay != TimeOfDay.DAWN ) )
		{
			m_currentTimeOfDay = TimeOfDay.DAWN;
			m_nextSky = m_dawnDuskSkyBox;
			StartCoroutine("TransitionToNextSky");
		}
		else if (pHours >= 7 && pHours < 18 && ( m_currentTimeOfDay != TimeOfDay.DAY ) )
		{
			m_currentTimeOfDay = TimeOfDay.DAY;
			m_nextSky = m_daySkyBox;
			StartCoroutine("TransitionToNextSky");
		}
		else if (pHours >= 18 && pHours < 20 && (m_currentTimeOfDay != TimeOfDay.DUSK) )
		{
			m_currentTimeOfDay = TimeOfDay.DUSK;
			m_nextSky = m_dawnDuskSkyBox;
			StartCoroutine("TransitionToNextSky");
		}
		else if ( (pHours >= 20 || pHours < 5) && m_currentTimeOfDay != TimeOfDay.NIGHT )
		{
			m_currentTimeOfDay = TimeOfDay.NIGHT;
			m_nextSky = m_nightSkyBox;
			StartCoroutine("TransitionToNextSky");
		}
		
		//Debug.Log(pHours);
		//Debug.Log(m_currentTimeOfDay.ToString() );
	}
	
	public IEnumerator TransitionToNextSky()
	{		
		float transisitionTime = 0f;
		//Material sky = m_currentSky;
		
		while (transisitionTime <= m_transitionTime)
		{
			//sky.Lerp(m_currentSky, m_nextSky, 1f);
			//create a new shader
//			RenderSettings.skybox = sky;
			RenderSettings.skybox = m_nextSky;
			transisitionTime += Clock.DeltaTime;
			yield return null;
		}
		
		m_currentSky = m_nextSky;
	}
	
	private enum TimeOfDay
	{
		INVALID,
		DAWN,
		DAY,
		DUSK,
		NIGHT
	}
}
