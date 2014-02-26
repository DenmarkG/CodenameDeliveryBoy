using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionManager
{
	public List<Mission> m_missionList = null;

	public MissionManager()
	{
		m_missionList = new List<Mission>();
	}

	public void AddMission(Mission newMission)
	{
		if(!m_missionList.Contains(newMission))
		{
			m_missionList.Add(newMission);
		}
	}
	
	public string DisplayMissions()
	{
		string missions = "";
		foreach(Mission m in m_missionList)
		{
			missions += m.ToString() + "\n";
		}
		
		return missions;
	}
}
