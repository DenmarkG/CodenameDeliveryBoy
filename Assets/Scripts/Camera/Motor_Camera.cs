using UnityEngine;
using System.Collections;

public class Motor_Camera : MonoBehaviour 
{
	public float moveSpeed = 5f;
	public float rotSpeed = 5f;
	
	private Transform playerTransform;
	private Transform myTransform;
	private Vector3 desiredPosition;
	
	void Awake()
	{
		//find the player
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		
		//cache the transform
		myTransform = this.transform;
		
		//find the desired position based ont the player's start position
		desiredPosition = playerTransform.position - myTransform.position;
	}
	
	void LateUpdate()
	{
		//the step amount to LerpBy
		float step = moveSpeed * Time.deltaTime;
		
		//find the postion from the player's position relative to the desired position
		Vector3 relativePlayerPos = playerTransform.position - desiredPosition;
		
		//move from this position and to the desired position
		myTransform.position = Vector3.Slerp(myTransform.position, relativePlayerPos, step);
	}
	
	/*void SmoothLookAt ()
    {
        // Create a vector from the camera towards the player.
        Vector3 relPlayerPosition = player.position - transform.position;
        
        // Create a rotation based on the relative position of the player being the forward vector.
        Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
        
        // Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
    }*/
}
