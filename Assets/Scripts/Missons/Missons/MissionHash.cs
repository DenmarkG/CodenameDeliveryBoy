using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class MissionHash
{
    private List<Mission> m_missionList = new List<Mission>();
    private static readonly MissionHash m_instance = new MissionHash();

    //missions 
    public Mission_01 mission_01 = null;

    private MissionHash()
    {
        mission_01 = new Mission_01();
        m_missionList.Add(mission_01);
    }

    public void AddNewMission(Mission newMission)
    {
        if (!m_missionList.Contains(newMission))
        {
            m_missionList.Add(newMission);
        }
    }

    public void ResetMisisons()
    {
        foreach (Mission mission in m_missionList)
        {
            mission.ResetMission();
        }
    }

    public static MissionHash Instance
    {
        get { return m_instance; }
    }
}
