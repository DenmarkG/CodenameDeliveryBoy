using UnityEngine;
using System.Collections;

public class Motor_Stalker : Motor_Base 
{
    [SerializeField]
    private float m_angleOfVision = 60f;

    private Vector3 m_targetPosition = Vector3.zero;
    private const float MAX_DIST_FROM_TARGET = .5f;
    private const float TURN_ANGLE_DEAD_ZONE = 5f;

    // Reference to the player in the game
    private Character_Player m_player = null;

    protected override void Awake()
    {
        base.Awake();
        m_charController = this.GetComponent<CharacterController>();
        m_player = GameManager.Player;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        if (LookAtPlayer())
    //        {
    //            Debug.Log("Player in sight");
    //        }
    //    }
    //}

    #region PUBLIC FUNCTIONS

    public override void UpdateMotor()
    {
        MoveToTarget(m_targetPosition);
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

    #endregion
}
