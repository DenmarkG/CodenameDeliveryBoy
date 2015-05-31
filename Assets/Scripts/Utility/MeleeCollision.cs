using UnityEngine;
using System.Collections;

public class MeleeCollision : MonoBehaviour 
{
    private Character_Stalker m_owner = null;
    private Collider m_collider = null;

    private void Awake()
    {
        m_collider = this.gameObject.GetComponent<Collider>();
    }

	private void Start () 
    {
        // Set the owner of this object
        // then disable self collisions with the owner's collidder
        // (self collisions make the animations look terrible)
        if (FindAndSetOwner())
        {
            Physics.IgnoreCollision(m_collider, m_owner.GetComponent<CharacterController>().GetComponent<Collider>() );
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.gameObject.tag == "Player")
        {
            m_owner.GetComponent<Motor_Stalker>().HitPlayer();
        }
    }

    private bool FindAndSetOwner()
    {
        m_owner = this.GetComponentInParent<Character_Stalker>();

        return m_owner != null;
    }
}
