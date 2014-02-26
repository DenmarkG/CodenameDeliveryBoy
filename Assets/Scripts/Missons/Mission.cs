using UnityEngine;
using System.Collections;

public class Mission
{
	[SerializeField]
	protected string m_missionName = "";
	[SerializeField]
	protected string m_missionDescription = "";

	protected MissionState currentState = MissionState.INVALID;

	public virtual bool CheckRequirements()
	{
		//check to see if the player meets the necessary requirements to start
		return false;
	}

	public virtual void BeginMission()
	{
		//begin the mission
	}

	public virtual void EndMission()
	{
		//end the mission
	}

	public string GetInfo()
	{
		return m_missionName + ": " + currentState.ToString() + "\n" + m_missionDescription;
	}
	
	protected enum MissionState
	{
		INVALID,
		IN_PROGRESS,
		SUCCESS,
		FAIL
	}
}
