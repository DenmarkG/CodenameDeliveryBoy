using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mission
{
	#region Protected Variables

	//The name of the current mission (for differentiating between multiple
	protected string m_missionName = "";

	//The description/goal of the current mission.
	protected string m_missionDescription = "";

	//this will hold a list of missions that can be accepted only after this mission is completed
	protected List<Mission> m_dependentMissions = new List<Mission>();

	//The state of the mission. If it has not started, it will be invalid or locked
	protected MissionState m_currenState = MissionState.INVALID;

	#endregion

	#region Methods

	public virtual bool CheckRequirements(Character_Player player)
	{
		//check to see if the player meets the necessary requirements to start
		//returns true by default for missions that have no requirements
		return true;
	}

	/// <summary>
	/// Actions to be done when the mission begins, when it is added to the mission manager 
	/// </summary>
	public virtual void BeginMission()
	{

	}

	/// <summary>
	/// Checks the completion requirements, to see if the mission should end
	/// </summary>
	/// <returns><c>true</c>, if mission is complete, <c>false</c> otherwise.</returns>
	public virtual bool CheckForCompletion()
	{
		//check to see if the mission has been completed;
		//returns true by default
		return true;
	}

	/// <summary>
	/// Actions to be completed upon ending this mission
	/// </summary>
	public virtual void EndMission()
	{
		//end the mission
	}

	#endregion

	#region Properties

	public string GetInfo
	{
		get { return m_missionName + ":\n\t\t" + m_currenState.ToString (); }
	}

	public MissionState GetMissionState
	{
		get { return m_currenState; }
	}

	#endregion

	/// <summary>
	/// Enumeration for the mission state
	/// </summary>
	public enum MissionState
	{
		INVALID,
		LOCKED,
		IN_PROGRESS,
		SUCCESS,
		FAIL
	}
}