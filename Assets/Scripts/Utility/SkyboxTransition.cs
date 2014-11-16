using UnityEngine;
using System.Collections;

public class SkyboxTransition : MonoBehaviour
{
    // The skybox material
    private Material m_skyboxMat = null;

    // True if the it is either Dawn or Day time ~5 am - 5pm
    private bool m_isDayTime = true;

    // How long we should spend transitioning between Cubemaps
    private float m_transitionTime = 100f;

    // The value _TimeOfDay will be the Dawn/Dusk sky, day/night doesn't matter
    // when _DayNight == 1, isDayTime == false, true otherwise

    public void InitSkyboxTransition(float startTime)
    {
        m_skyboxMat = Resources.Load<Material>("Skyboxes/Transitional/TransitionalSkybox");
        if (startTime >= 5 && startTime < 17)
        {
            m_isDayTime = true;
            m_skyboxMat.SetFloat("_DayNight", 0);
            // Set the time of day
            m_skyboxMat.SetFloat("_TimeOfDay", 0);
        }
        else
        {
            m_isDayTime = false;
            m_skyboxMat.SetFloat("_DayNight", 1);
            // Set the time of day
            m_skyboxMat.SetFloat("_TimeOfDay", 1);
        }

        // Assign the skybox with updated settings to the renderer
        RenderSettings.skybox = m_skyboxMat;
    }

    public void UpdateSkybox(float hours)
    {
        // Daytime
        if (hours >= 5 && hours < 17)
        {
            m_isDayTime = true;
            m_skyboxMat.SetFloat("_DayNight", 0);
            if (hours == 9)
            {
                StartCoroutine(TransitionToNextSky(false));
            }
            else if (hours == 16)
            {
                StartCoroutine(TransitionToNextSky(true));
            }
        }
        // Night time
        else
        {
            m_isDayTime = false;
            m_skyboxMat.SetFloat("_DayNight", 1);

            if (hours == 4)
            {
                StartCoroutine(TransitionToNextSky(false));
            }
            else if (hours == 21)
            {
                StartCoroutine(TransitionToNextSky(true));
            }
        }
    }

    // This function smoothly tranisions from one skybox material to another using the exposed properties in the shader
    private IEnumerator TransitionToNextSky(bool toDawnDusk)
    {
        float transisitionTime = 0f;
        while (transisitionTime < m_transitionTime)
        {
            transisitionTime += Clock.DeltaTime;
            transisitionTime = Mathf.Clamp(transisitionTime, 0, m_transitionTime);
            if (toDawnDusk)
            {
                m_skyboxMat.SetFloat("_TimeOfDay", transisitionTime / m_transitionTime);
            }
            else
            {
                m_skyboxMat.SetFloat("_TimeOfDay", 1 - (transisitionTime / m_transitionTime));
            }

            yield return null;
        }
    }

    public bool IsDayTime
    {
        get { return m_isDayTime; }
    }
}
