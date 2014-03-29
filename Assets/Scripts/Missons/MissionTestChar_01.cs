using UnityEngine;
using System.Collections;

public class MissionTestChar_01 : MonoBehaviour
{
	[SerializeField]
	private Mission mission = MissionHash.mission_01;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			if (mission.GetMissionState == MissionState.INVALID)
			{
				Character_Player player = other.GetComponent<Character_Player>();
				if( mission.CheckRequirements(player) )
				{
					player.PlayerMissionManager.AddMission(mission);
				}
			}
			else if(mission.GetMissionState == MissionState.IN_PROGRESS)
			{
				if(mission.CheckForCompletion() )
				{
					mission.EndMission();
				}
			}
		}
	}
}
