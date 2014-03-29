using UnityEngine;
using System.Collections;

public class ScreenMask 
{
	#region Private Serialized Variables

	[SerializeField]
	private Texture2D m_texture = null;

	[SerializeField]
	private Vector2 m_position = Vector2.zero;

	#endregion

	#region Private Variables

	private Vector2 m_size = Vector2.zero;
	private Rect m_displayRect = new Rect(0,0,0,0);
	//private float m_alpha = 1f;

	#endregion

	//Constructor
	public ScreenMask(float pos_X, float pos_Y, Texture2D texture)
	{
		m_position.x = pos_X;
		m_position.y = pos_Y;

		m_displayRect = new Rect(m_position.x, m_position.y, m_texture.width, m_texture.height);

		OnActivate();
	}

	#region Functions

	public void Draw()
	{
		GUI.Box(m_displayRect, m_texture);
	}

	public bool SetAlpha(float alpha)
	{
		return false;
	}

	public bool SetPosition(float pos_X, float pos_Y)
	{
		if (pos_X > 0f && pos_Y > 0f)
		{
			m_position.x = pos_X;
			m_position.y = pos_Y;
			return true;
		}

		return false;
	}

	public bool SetSize(float size_X, float size_Y)
	{
		if(size_X > 0 && size_Y > 0)
		{
			m_texture.Resize( (int) size_X, (int) size_Y);
			return true;
		}

		return false;
	}

	#endregion

	#region
		
	private void OnActivate()
	{
		GuiManager.OnUpdateGUI += Draw;
	}

	private void OnDeactivate()
	{
		GuiManager.OnUpdateGUI -= Draw;
	}

	#endregion

	#region Properties

	public Vector2 GetPosition
	{
		get { return m_position; }
	}

	public Vector2 GetSize
	{
		get { return m_size; }
	}

	public Texture2D GetTexture
	{
		get { return m_texture; }
		set {m_texture = value; }
	}

	#endregion

}
