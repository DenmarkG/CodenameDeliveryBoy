using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionManager
{
	private List<Mission> m_missionList = new List<Mission>();

	public MissionManager()
	{
		//list of missions currently in progress
		m_missionList = new List<Mission>();
	}

	/// <summary>
	/// Adds a new mission to the mission list, if a mission is not available or has already been started it will be skipped
	/// </summary>
	/// <param name="newMission">New mission.</param>
	public void AddMission(Mission newMission)
	{
		if(!m_missionList.Contains(newMission) )
		{
			newMission.BeginMission();
			m_missionList.Add(newMission);
		}

		GuiManager.DisplayStatusMessage("New Mission Added!");
	}
	
	public void DisplayCurrentMissions()
	{
		//show the current missions
		if(m_missionList.Count > 0)
		{
			string currentMissions = "";
			string completeMissions = "";
			foreach(Mission m in m_missionList)
			{
				//add each mission to the appropriate list
				if(m.GetMissionState == Mission.MissionState.IN_PROGRESS)
					currentMissions += m.GetInfo + "\n";
				else
					completeMissions += m.GetInfo + "\n";
			}
			if (currentMissions != "")
				GUI.Box(new Rect(0, 50, 150, 50), currentMissions);

			if (completeMissions != "")
				GUI.Box(new Rect(Screen.width - 150, 50, 150, 50), completeMissions);
		}
	}

	public virtual void UpdateMission(Mission updatedMission)
	{
		if(updatedMission.GetMissionState == Mission.MissionState.SUCCESS || updatedMission.GetMissionState == Mission.MissionState.FAIL)
		{
			//
		}
	}
}
