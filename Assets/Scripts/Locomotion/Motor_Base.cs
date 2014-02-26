using UnityEngine;
using System.Collections;

public class Motor_Base : MonoBehaviour 
{
	public float moveSpeed = 5f;
	public float rotSpeed = 2.5f;
	public float attackCoolDown = 1; //the rate of attack in seconds
	public float attackRange = 15;
	public float gravity = 21f;
	
	protected bool bRunning;
	protected bool bCanAttack;
	protected bool bCanMove;
	protected Transform myTransform;
	protected CharacterController myCharacterController;
	protected bool bAttacking = false;
	protected GameObject target;
	protected Vector3 moveVector = Vector3.zero;
	//private Animator myAnimator;

	void Awake()
	{
		bCanMove = true;
		bCanAttack = true;
		bRunning = false;
		myTransform = this.transform;
		myCharacterController = this.GetComponent<CharacterController>();
		OnAwake();
		//myAnimator = this.GetComponent<Animator>();
		//myAnimator.SetBool("Alive", true);
	}
	

	#region Virtual Functions
	protected virtual void OnAwake()
	{
	}
	
	public virtual void UpdateMotor()
	{
	}
	
	protected void Rotate(float pHorz, float pVert)
	{
		float step = rotSpeed * Time.deltaTime;
		Vector3 targetDir = new Vector3(pHorz, 0, pVert);
		Quaternion qTargetDir = Quaternion.LookRotation(targetDir, Vector3.up);
		Quaternion newRot = Quaternion.Lerp(myTransform.rotation, qTargetDir, step);
		myTransform.rotation = newRot;
	}
	
	protected void Rotate(Vector3 direction)
	{
		float step = rotSpeed * Time.deltaTime;
		Quaternion qTargetDir = Quaternion.LookRotation(direction, Vector3.up);
		Quaternion newRot = Quaternion.Lerp(myTransform.rotation, qTargetDir, step);
		myTransform.rotation = newRot;
	}
	
	protected virtual IEnumerator AttackCoolDown(float coolDownTime)
	{
		yield return new WaitForSeconds(coolDownTime);
		bCanAttack = true;
	}
		
	protected virtual void Attack()
	{
		Debug.Log("Attacking");
	}
	#endregion
	
	#region Functions
	protected void ApplyGravity()
	{
		if(!myCharacterController.isGrounded)
		{
			moveVector.y -= gravity * Time.deltaTime;
		}
	}
	#endregion
	
	
	#region Properties
	protected bool IsAttacking
	{
		get { return bAttacking; }
	}
	#endregion
}
