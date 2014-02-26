using UnityEngine;
using System.Collections;

public class Motor_Player : Motor_Base
{	
	private float horz = 0f;
	private float vert = 0f;
	
	//private Character_Player myCharacter;
	
	protected override void OnAwake()
	{
		attackRange = 2.5f;
		attackCoolDown = .75f;
		//myCharacter = this.GetComponent<Character_Player>();
	}
	
	public override void UpdateMotor()
	{
		if(bCanMove)
		{
			moveVector = Vector3.zero;
			
			horz = Input.GetAxis("Horizontal");
			vert = Input.GetAxis("Vertical");
			bAttacking = Input.GetKeyDown(KeyCode.F);
			bRunning = Input.GetKey(KeyCode.LeftShift);
			
			moveVector = Vector3.zero;
			
			if(bCanAttack && bAttacking)
			{
				Attack();
			}
		
			if(horz != 0 || vert != 0)
			{
				Rotate(horz, vert);
				moveVector += (horz * Vector3.right) + (vert * Vector3.forward);
			}
			
			ApplyGravity();
		
			if(moveVector.magnitude > 1)
			{
				moveVector = Vector3.Normalize(moveVector);
			}
			
			if(!bRunning)
			{
				moveVector *= moveSpeed;
			}
			else
			{
				moveVector *= moveSpeed * 1.5f;
			}
				
			myCharacterController.Move(moveVector * Time.deltaTime);
		}
	}
	
	GameObject CheckForEnemy()
	{
		RaycastHit hit;
		if(Physics.Raycast(myTransform.position, myTransform.forward, out hit, attackRange))
		{
			if(hit.transform.tag == "Enemy")
			{
				return hit.transform.gameObject;
			}
		}
		return null;
	}
	
	protected override void Attack ()
	{
		Debug.Log("Attacking");
		bCanAttack = false;
		StartCoroutine("AttackCoolDown", attackCoolDown);
		target = CheckForEnemy();
		if(target != null)
		{
			//Health targetHealth = target.GetComponent<Health>();
			//targetHealth.TakeDamage(myCharacter.baseAttack);
		}
	}
	
	
}
