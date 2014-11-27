using UnityEngine;
using System.Collections;

public class Motor_Stalker : Motor_Base 
{
    private Vector3 m_targetPosition = Vector3.zero;
    private const float MAX_DIST_FROM_TARGET = .5f;
    private const float TURN_ANGLE_DEAD_ZONE = 5f;

    protected override void Awake()
    {
        base.Awake();
        m_charController = this.GetComponent<CharacterController>();
    }

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

    protected void Rotate(Vector3 targetDir)
    {
        //create a step value to rotate the player over time
        float step = m_rotationSpeed * Clock.DeltaTime;

        //create a rotation with the forward vector the same as the move direction
        Quaternion qTargetDir = Quaternion.LookRotation(targetDir, Vector3.up);

        //set the rotation to the lerp between the current and the desired direction
        transform.rotation = Quaternion.Lerp(this.transform.rotation, qTargetDir, step);
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
