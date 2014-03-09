using UnityEngine;
using System.Collections;

[System.Serializable]
public class Mission
{
	[SerializeField]
	protected string m_missionName = "";
	[SerializeField]
	protected string m_missionDescription = "";
	[SerializeField]
	protected MissionState m_currenState = MissionState.INVALID;

	public virtual bool CheckRequirements(Character_Player player)
	{
		//check to see if the player meets the necessary requirements to start
		//returns true by default for missions that have no requirements
		return true;
	}
	
	public virtual void BeginMission()
	{
		//begin the mission
	}

	public virtual bool CheckForCompletion()
	{
		//check to see if the mission has been completed;
		//returns true by default
		return true;
	}

	public virtual void EndMission()
	{
		//end the mission
	}

	public string GetInfo()
	{
		return m_missionName + ": " + m_currenState.ToString();
	}

	public MissionState GetMissionState
	{
		get { return m_currenState; }
	}
}


public enum MissionState
{
	INVALID,
	IN_PROGRESS,
	SUCCESS,
	FAIL
}