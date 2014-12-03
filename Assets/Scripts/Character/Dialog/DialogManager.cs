using UnityEngine;
using System;
using System.Collections;

public class DialogManager 
{
	Character_Player m_playerChar = null;
	Character_NPC m_npChar = null;

	public DialogManager(Character_Player pPlayer)
	{
		m_playerChar = pPlayer;
	}

	public void BeginDialog(Character_NPC pNPC)
	{
		m_npChar = pNPC;
		m_npChar.OnSpeak();
		m_playerChar.OnSpeak();

		//Action<bool> OnEnd = EndDialog;

		GuiManager.ShowDialog (m_npChar.GetDialog, EndDialog);
	}

	public void EndDialog(bool shoudEnd)
	{
		if (shoudEnd) 
		{
            if (m_npChar != null)
            {
                m_npChar.OnEndSpeak();
            }
			m_npChar = null;

            if (m_playerChar != null)
            {
                m_playerChar.OnEndSpeak();
            }
		}
	}
}
