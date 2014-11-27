using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This class holds a reference to all the waypoints in the scene as well as ways to process them
// from external classes.
public sealed class Waypoint
{
    #region PRIVATE VARIABLES

    // All the waypoints in the scene
    private Vector3[] m_allWaypoints = null;

    private static readonly Waypoint m_instance = new Waypoint();
    #endregion

    #region UNITY FUNCTIONS
    private Waypoint()
	{
        FindAllWayPoints(out m_allWaypoints);
	}

    #endregion

    #region PUBLIC FUNCTIONS

    public static void FindBestPath(Vector3 currentPos, Vector3 targetPos, out List<Vector3> path)
    {
        path = new List<Vector3>();
    }

    public static Vector3 FindNearestWaypoint(Vector3 currentPos)
    {
        Vector3 nearest = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        for (int i = 0; i < m_instance.m_allWaypoints.Length; ++i)
        {
            if ((m_instance.m_allWaypoints[i] - currentPos).sqrMagnitude < (m_instance.m_allWaypoints[i] - nearest).sqrMagnitude &&
                m_instance.m_allWaypoints[i] != currentPos)
            {
                nearest = m_instance.m_allWaypoints[i];
            }
        }
        Debug.Log("nearest: " + nearest);
        return nearest;
    }

    #endregion

    #region PRIVATE FUNCTIONS

    private void FindAllWayPoints(out Vector3[] allWaypoints)
    {
        GameObject[] allObjs = GameObject.FindGameObjectsWithTag("Waypoint");
        allWaypoints = new Vector3[allObjs.Length];
        for (int i = 0; i < allObjs.Length; ++i)
        {
            allWaypoints[i] = allObjs[i].transform.position;
            Debug.Log("Wapoint " + i + " pos: " + allObjs[i].transform.position);
        }
    }

    private void PerformAStarSearch(Vector3 currentPos, ref List<Vector3> path)
    {
        //List<Vector3> closedSet = new List<Vector3>();
        //Queue<Vector3> openSet = new Queue<Vector3>();

        //// Enqueue the start position (the current position)
        //openSet.Enqueue(currentPos);

        //
    }

    #endregion

    #region PROPERTIES

    public static Vector3[] AllWaypoints
    {
        get { return m_instance.m_allWaypoints; }
    }

    #endregion
}
