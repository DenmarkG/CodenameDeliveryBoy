using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	#region Serialized Private Variables

	[SerializeField]
	private float m_maxHealth = 100f;
	[SerializeField]
	private Texture2D m_outlineTexture = null;
	[SerializeField]
	private Texture2D m_healthTexture = null;
	
	#endregion

	#region Private Variables

	private float m_currentHealth;
    private float m_displaySize_X = 150f;
    private float m_displaySize_Y = 25f;
    private Rect m_displayRect;

	#endregion
	
	void Awake()
	{
        m_displayRect = new Rect(Screen.width - m_displaySize_X, 0, m_displaySize_X, m_displaySize_Y);
		m_currentHealth = m_maxHealth;
	}

	void Start()
	{
		//GuiManager.OnUpdateGUI += DrawHealthBar;
        GuiManager.OnUpdateGUI += DrawHealthStatus;
	}

	#region Functions()

	public bool ModifyHealth(float amount)
	{
		if(m_currentHealth <= m_maxHealth && m_currentHealth > 0)
		{
			m_currentHealth += amount;
			RescaleHealth();
			return true;
		}
		
		return false;
	}

	public void DrawHealthBar()
	{
		//this scales constrained. fix
		//[???]
		GUI.Label(new Rect(0, 0, m_healthTexture.width * (m_currentHealth / m_maxHealth), m_healthTexture.height), m_healthTexture);
		GUI.Label(new Rect(0, 0, m_outlineTexture.width, m_outlineTexture.height), m_outlineTexture);
	}

    private void DrawHealthStatus()
    {
        GUI.Box(m_displayRect, "Health: " + (m_currentHealth/m_maxHealth) * 100 + "%");
    }

	private void RescaleHealth()
	{
		if(m_currentHealth > m_maxHealth)
        {
            m_currentHealth = m_maxHealth;
        }
        else if (m_currentHealth < 0)
        {
            m_currentHealth = 0;
        }
	}

	#endregion

	#region Properties

	public float CurrentHealth
	{
		get {return m_currentHealth; }
	}

    public bool IsAlive
    {
        get { return m_currentHealth > 0; }
    }

	#endregion
}
