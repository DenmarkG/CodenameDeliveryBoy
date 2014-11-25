using UnityEngine;
using System.Collections;

public class Motor_Stalker : Motor_Base 
{
    protected override void Awake()
    {
        base.Awake();
        m_charController = this.GetComponent<CharacterController>();
    }

    protected override void Start() 
	{
	    // 
	}
}
