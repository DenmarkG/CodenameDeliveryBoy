using UnityEngine;
using System.Collections;

public class Motor_Enemy : Motor_Base 
{
	public float wiggleRange = .5f; // a number that allows for a range of that the enemy can attack in 
	public float sightRange = 10f;
	
	public float patrolDistance = 15f;
	
	protected float distanceFromTarget = 0f;
	protected Transform targetTransform = null;
	protected bool bCanSeeTarget = false;
	//protected Character_Enemy myCharacter = null;
	protected GameObject player = null;
	
	protected override void OnAwake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		targetTransform = player.transform;
		//myCharacter = this.GetComponent<Character_Enemy>();
	}
	
	// Update is called once per frame
	public override void UpdateMotor () 
	{
		moveVector = Vector3.zero;
		
		Debug.DrawLine(myTransform.position, targetTransform.position, Color.blue);
		bCanSeeTarget = CheckForPlayer();
		
		
		if(bCanMove && bCanSeeTarget)
		{
			Vector3 relPlayerPos = targetTransform.position - myTransform.position;
			relPlayerPos.y = 0;
			Rotate(relPlayerPos);
			
			distanceFromTarget = Vector3.Distance(targetTransform.position, myTransform.position);
			
			if(distanceFromTarget > attackRange)
			{
				//move closer to the player
				Move(myTransform.forward);
			}
			
			if(distanceFromTarget <= (attackRange + wiggleRange))
			{
				Attack();
			}
		}
	}
	
	void Move(Vector3 direction)
	{				
		
		if(moveVector.magnitude > 1)
		{
			moveVector = Vector3.Normalize(moveVector);
		}
		
		moveVector += moveSpeed * direction;
			
		ApplyGravity();
		
		myCharacterController.Move(moveVector * Time.deltaTime);
	}
	
	bool CheckForPlayer()
	{
		RaycastHit hit;
		if (Physics.Raycast(myTransform.position, targetTransform.position - myTransform.position, out hit, sightRange))
		{
			if (hit.transform.tag == "Player")
			{
				target = hit.transform.gameObject;
				return true;
			}
		}
		target = null;
		return false;
	}
	
	protected override void Attack ()
	{
		Debug.Log("Attacking");
		bCanAttack = false;
		if(target != null)
		{
			//Health targetHealth = target.GetComponent<Health>();
			//targetHealth.TakeDamage(myCharacter.baseAttack);
		}
		StartCoroutine("AttackCoolDown", attackCoolDown);
	}
}
