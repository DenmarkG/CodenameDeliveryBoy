using UnityEngine;
using System.Collections;

public class Motor_Player : Motor_Base
{
    // True if the player is in cover
    private bool m_isInCover = false;

    // The max distance the player can be from cover before it adjusts automagically
    private const float MAX_DIST_FROM_COVER = .35f;
    // How close the player can get to a corner before stopping
    private const float MAX_DIST_FROM_CORNER = .5f;
    //How far to cast a single ray
    private const float MAX_RAYCAST_DIST = .9f;

    //reference to the game's camera
    protected DBCamera m_camera = null;

	protected override void Awake ()
	{
        base.Awake();
	}

    private void Start()
    {
        m_camera = DBCamera.GetCamera;
        m_charController = this.gameObject.GetComponent<CharacterController>();
        if (m_charController == null)
        {
            m_charController = this.gameObject.AddComponent<CharacterController>();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (!m_isInCover && Vector3.Dot(other.normal, m_transform.forward) <= 0)
        {
            if (CheckForCover(other))
            {
                EnterCover();
            }
        }
    }

	public override void UpdateMotor ()
	{
		if (!m_bLocked)
		{
			//reset the angle to zero
			m_angle = 0f;

			//Get the input from the player
			m_horizontal = Input.GetAxis(GameControllerHash.LeftStick.HORIZONTAL);
			m_vertical = Input.GetAxis(GameControllerHash.LeftStick.VERTICAL);

            if (m_isInCover)
            {

                // Early out and exit cover when the Action button is pressed
                if (m_isInCover && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown(GameControllerHash.Buttons.B)))
                {
                    LeaveCover();
                    return;
                }

                if (Mathf.Abs(m_horizontal) > DEAD_ZONE && ClampToCover(this.transform.right * m_horizontal))
                {
                    m_animator.SetFloat("Direction", -m_horizontal);
                    m_animator.SetFloat("Speed", Mathf.Abs(m_horizontal));
                }
                else
                {
                    m_animator.SetFloat("Direction", 0);
                    m_animator.SetFloat("Speed", 0);
                }

                // Remove any inaccuracies that may have occured due to animation
                CorrectTransformError();
                // Lock the camera behind the player
                if (!m_camera.IsInOrbit)
                {
                    m_camera.LockCamera();
                }
            }
            else
            {
                //check to see if the player is running
                m_isRunning = Input.GetButton(GameControllerHash.Buttons.B) || Input.GetKey(KeyCode.LeftShift);

                //now convert the movement to world space
                ConvertInputToWorldSpace();

                //set the values for the animator
                m_animator.SetFloat("Speed", m_speed);
                m_animator.SetFloat("Direction", m_direction);

                //[#todo]check to see if an elseif can be made instead later
                if (m_speed < DEAD_ZONE && Mathf.Abs(m_horizontal) < DEAD_ZONE)
                {
                    m_animator.SetFloat("Speed", 0);
                    m_animator.SetFloat("Angle", 0);
                }
            }
			//[#todo] implement a pause method that utilizes this method
			m_animator.speed = Clock.TimeScale;
		}
	}

	public override void LockMotion()
	{
		base.LockMotion();
		m_animator.SetFloat("Speed", 0);
		m_animator.SetFloat("Direction", 0);
		m_animator.SetFloat("Angle", 0);
	}
	
	public override void UnlockMotion()
	{
		base.UnlockMotion();
	}

	protected void ConvertInputToWorldSpace()
	{
		//**************************************************************************************************
		//This function is currently a work in progress
		//**************************************************************************************************

		//Remap the input from the controller to world space
		Transform cameraTransform = m_camera.transform;
		Vector3 moveVector = Vector3.zero;

		moveVector.x = (m_horizontal * cameraTransform.right.x) + (m_vertical * cameraTransform.forward.x);
		moveVector.z = (m_horizontal * cameraTransform.right.z) + (m_vertical * cameraTransform.forward.z);

        if (moveVector.magnitude > 1)
        {
            moveVector.Normalize();
        }

		//cache the forward vector of the player and the camera
		//also remove the y values for each, since we only want to calculate in 2 dimensions
		Vector3 playerDirection = this.transform.forward;
		playerDirection.y = 0;
		Vector3 cameraDiretion = m_camera.transform.forward;
		cameraDiretion.y = 0;
		cameraDiretion.Normalize(); //normalize the camera vector to keep length consistent

		//if the player is running set the speed to 2. Otherwise set it to the length of the input vector

        if (m_isRunning && m_speed > DEAD_ZONE)
        {
            m_speed = Mathf.Lerp(m_speed, RUN_SPEED, Clock.DeltaTime * RUN_TRANSITION_SPEED);
        }
        else
        {
            m_speed = Mathf.Lerp(m_speed, moveVector.magnitude, Clock.DeltaTime * RUN_TRANSITION_SPEED);
        }

		//get the angle between the camera's forward vector and the movement vector
		m_angle = Vector3.Angle(cameraDiretion, moveVector) * (m_horizontal >= 0 ? 1 : -1);

		m_direction = m_angle / 90f;
//		Debug.Log("Angle: " + m_angle);
//		Debug.Log("Direction: " + m_direction);

		if (moveVector.magnitude > DEAD_ZONE)
        {
            Rotate(moveVector);
        }
			
        //Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), cameraDiretion, Color.blue);
        //Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), moveVector, Color.red);
        //Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), playerDirection, Color.red);
	}

    #region PRIVATE FUNCTIONS

    private bool CheckForCover(ControllerColliderHit other)
    {
        if ((m_charController.collisionFlags & CollisionFlags.Sides) != 0 && 
            (other.gameObject.tag != "NPC" && other.gameObject.tag != "Stalker") &&
            (other.gameObject.tag != "Ground"))
        {
            return true;
        }

        return false;
    }

    private bool CheckForCrouchingCover()
    {
        RaycastHit hit;
        Vector3 castPos = this.transform.position + (this.transform.up * EYE_HEIGHT);
        if (Physics.Raycast(castPos, this.transform.forward, out hit, MAX_RAYCAST_DIST))
        {
            return false;
        }

        return true;
    }

    // This function uses raycasting to keep the player close to the cover object
    // if at any point the raycast fails, the player will exit cover
    // This will also align the player to the cover's surface to prevent artifacts
    private void CorrectTransformError()
    {
        Vector3 moveVector;
        RaycastHit hit;
        if (Physics.Raycast(m_transform.position, m_transform.forward, out hit, MAX_RAYCAST_DIST))
        {
            // Correct Position error if it exists
            if (hit.distance > MAX_DIST_FROM_COVER)
            {
                //move closer
                float distanceToMove = hit.distance - MAX_DIST_FROM_COVER;
                moveVector = hit.point - m_transform.position;
                moveVector += m_transform.forward * distanceToMove;
                m_charController.Move(moveVector * Clock.DeltaTime);
            }

            // Correct orientation error if beyond the threshold
            float angle = Vector3.Angle(m_transform.forward, -hit.normal);
            if (angle > 0)
            {
                if (Vector3.Dot(m_transform.right, hit.normal) < 0)
                {
                    moveVector = Vector3.up * angle * Clock.DeltaTime;
                }
                else
                {
                    moveVector = Vector3.up * -angle * Clock.DeltaTime;
                }
                
                m_transform.Rotate(moveVector);
            }
        }
        else
        {
            //LeaveCover();
        }
    }

    // Raycasts to the left/right of the player, and makes sure that the next direction is valid
    // returns false if the direciton is not valid
    private bool ClampToCover(Vector3 dir)
    {
        Vector3 castPos = this.transform.position + dir.normalized * MAX_DIST_FROM_CORNER;
        if (Physics.Raycast(castPos, this.transform.forward, MAX_RAYCAST_DIST))
        {
            return true;
        }

        return false;
    }

    private void EnterCover()
    {
        // Snap the game camera
        m_camera.LockCamera();

        m_animator.SetBool("IsInCover", true);
        m_animator.SetBool("IsCrouching", CheckForCrouchingCover() );
        m_isInCover = true;
    }

    private void LeaveCover()
    {
        m_animator.SetBool("IsInCover", false);
        m_animator.SetBool("IsCrouching", false);
        m_isInCover = false;
    }

    #endregion

	#region Public Properties

	public float AnimatorSpeed
	{
		get { return m_animator.speed; }
		set { m_animator.speed = value; }
	}

	#endregion
}