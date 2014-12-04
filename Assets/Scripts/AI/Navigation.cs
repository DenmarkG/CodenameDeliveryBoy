using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This class holds a reference to all the waypoints in the scene as well as ways to process them
// from external classes.
public sealed class Navigation
{
    // A public struct to improve the look of the AI.
    // This will hold a reference to the last and current waypoints.

    private Vector3 m_maxVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

    public struct ActivePoints
    {
        // the default constructor sets each vector to the max value
        public ActivePoints(bool setMax = true)
        {
            m_previousWaypoint = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            m_currentWaypoint = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        }

        public Vector3 m_previousWaypoint;
        public Vector3 m_currentWaypoint;
    }

    #region PRIVATE VARIABLES

    // All the waypoints in the scene
    private Vector3[] m_allWaypoints = null;

    private static readonly Navigation m_instance = new Navigation();
    #endregion

    // Constructor simply finds all waypoints in the scene
    private Navigation()
    {
        FindAllWayPoints(out m_allWaypoints);
    }

    #region PUBLIC FUNCTIONS

    public static Vector3 FindNearestWaypoint(ref ActivePoints activePoints)
    {
        Vector3 nearest = m_instance.m_maxVector;
        for (int i = 0; i < m_instance.m_allWaypoints.Length; ++i)
        {
            if ((m_instance.m_allWaypoints[i] - activePoints.m_currentWaypoint).sqrMagnitude <= (nearest - activePoints.m_currentWaypoint).sqrMagnitude &&
                m_instance.m_allWaypoints[i] != activePoints.m_currentWaypoint && 
                m_instance.m_allWaypoints[i] != activePoints.m_previousWaypoint)
            {
                nearest = m_instance.m_allWaypoints[i];
            }
        }
        //Debug.Log("nearest: " + nearest);

        m_instance.UpdateActivePoints(nearest, ref activePoints);
        return nearest;
    }

    public static Vector3 FindRandomWaypoint(ref ActivePoints activePoints)
    {
        Vector3 point = m_instance.m_maxVector;

        while (point == m_instance.m_maxVector || point == activePoints.m_currentWaypoint)
        {
            point = m_instance.m_allWaypoints[(int)Random.Range(0, m_instance.m_allWaypoints.Length)];
        }

        m_instance.UpdateActivePoints(point, ref activePoints);
        return point;
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
            //Debug.Log("Wapoint " + i + " pos: " + allObjs[i].transform.position);
        }
    }

    private void UpdateActivePoints(Vector3 newPos, ref ActivePoints points)
    {
        points.m_previousWaypoint = points.m_currentWaypoint;
        points.m_currentWaypoint = newPos;
    }

    #endregion

    #region PROPERTIES

    public static Vector3[] AllWaypoints
    {
        get { return m_instance.m_allWaypoints; }
    }

    #endregion
}
