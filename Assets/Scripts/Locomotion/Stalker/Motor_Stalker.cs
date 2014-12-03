using UnityEngine;
using System.Collections;

public class Motor_Stalker : Motor_Base 
{
    // The angle of vision of the Ai
    [SerializeField]
    private float m_angleOfVision = 30f;
    // The min distance the AI can see 
    private float m_nearPlaneDistance = .3f;
    // How far the AI can see
    [SerializeField]
    private float m_farPlaneDistance = 5f;
    
    // Objects in this range will activate the enemy sight
    [SerializeField]
    private float m_searchRadius = 6f;

    private Vector3 m_targetPosition = Vector3.zero;
    private const float MAX_DIST_FROM_TARGET = .5f;
    private const float TURN_ANGLE_DEAD_ZONE = 5f;

    // Reference to the player in the game
    private Character_Player m_player = null;

    // The camera that will act as the line of sight trigger for the stalker
    private Camera m_camera = null;
    // The planes of the viewing Frustum. If they interset the player's aabb, then the player in in the FOV
    private Plane[] m_planes = null;

    // Whether or not the enemy should check for nearby objects of interest. 
    // This is done purely for performance reasons
    private bool m_shouldCheckForNearbyObjects = false;

    protected override void Awake()
    {
        base.Awake();
        m_charController = this.GetComponent<CharacterController>();
        m_player = GameManager.Player;

        if (m_camera == null)
        {
            GameObject cam = new GameObject("AI Cam");
            m_camera = cam.AddComponent<Camera>();
            m_camera.transform.position = m_transform.position + (Vector3.up * EYE_HEIGHT);
            m_camera.nearClipPlane = m_nearPlaneDistance;
            m_camera.farClipPlane = m_farPlaneDistance;
            m_camera.fieldOfView = m_angleOfVision;
            m_camera.enabled = false;
        }

        m_camera.transform.parent = m_transform;
        m_planes = GeometryUtility.CalculateFrustumPlanes(m_camera);

        // Make sure that the search radius is >= the view distance
        // This way we can start looking as soon as the player is in range
        if (m_searchRadius < m_farPlaneDistance)
        {
            m_searchRadius = m_farPlaneDistance;
        }
        
        // Then set the radius of the attached sphere collider to the search radius
        this.gameObject.GetComponent<SphereCollider>().radius = m_searchRadius;
    }

    #region PUBLIC FUNCTIONS

    public override void UpdateMotor()
    {
        MoveToTarget(m_targetPosition);
    }

    public override void UpdateMotorFixed()
    {
        m_planes = GeometryUtility.CalculateFrustumPlanes(m_camera);
        if (LookForObjectsOfInterestInFOV(ref m_planes, m_player))
        {
            Debug.Log("Player visible");
        }
    }

    public void SetNewTarget(Vector3 newTarget)
    {
        if (newTarget != m_targetPosition)
        {
            m_targetPosition = newTarget;
        }
    }

    #endregion


    #region PRIVATE FUNCTIONS

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_shouldCheckForNearbyObjects = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_shouldCheckForNearbyObjects = false;
        }
    }

    private void MoveToTarget(Vector3 targetPos)
    {
        Vector3 moveDir = targetPos - m_transform.position;
        m_speed = 1;
        {
            if (Vector3.Angle(moveDir, this.transform.forward) > TURN_ANGLE_DEAD_ZONE)
            {
                Rotate(moveDir);
            }
            
            m_animator.SetFloat("Speed", m_speed);
        }
    }

    // Raycasts toward the player. 
    // Returns true if the player is seen
    private bool LookAtPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(m_transform.position, m_player.transform.position - m_transform.position, out hit, 10f) )
        {
            if (Vector3.Angle(m_transform.position, m_player.transform.position) <= m_angleOfVision)
            {
                return true;
            }
            
        }
        
        return false;
    }

    // returns true if the enemy sees an object of interest
    private bool LookForObjectsOfInterestInFOV(ref Plane[] planes, Character_Player player)
    {
        if (GeometryUtility.TestPlanesAABB(planes, player.collider.bounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region PROPERTIES

    public bool TargetReached
    {
        get 
        { 
            if (m_targetPosition != Vector3.zero)
            {
                return (m_transform.position - m_targetPosition).sqrMagnitude < MAX_DIST_FROM_TARGET; 
            }
            else
            {
                return true;
            }
        }
    }

    public Vector3 CurrentTarget
    {
        get { return m_targetPosition; }
    }

    public bool ShouldCheckForNearbyObjects
    {
        get { return m_shouldCheckForNearbyObjects; }
        set { m_shouldCheckForNearbyObjects = value; }
    }

    #endregion
}
