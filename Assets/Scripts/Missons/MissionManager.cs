using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MissionManager
{
	[SerializeField]
	private List<Mission> m_missionList = new List<Mission>();

	public MissionManager()
	{
		m_missionList = new List<Mission>();
	}

	public void AddMission(Mission newMission)
	{
		if(!m_missionList.Contains(newMission) )
		{
			newMission.BeginMission();
			m_missionList.Add(newMission);
		}
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
				if(m.GetMissionState == MissionState.IN_PROGRESS)
					currentMissions += m.GetInfo() + "\n";
				else
					completeMissions += m.GetInfo() + "\n";
			}
			if (currentMissions != "")
				GUI.Box(new Rect(0, 50, 150, 50), currentMissions);

			if (completeMissions != "")
				GUI.Box(new Rect(Screen.width - 150, 50, 150, 50), completeMissions);
		}
	}

	public void UpdateMission(Mission updatedMission)
	{
		if(updatedMission.GetMissionState == MissionState.SUCCESS || updatedMission.GetMissionState == MissionState.FAIL)
		{
			//
		}
	}
}
