using UnityEngine;
using System.Collections;

public class pickUp : MonoBehaviour {

	private GameObject cow;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if (Input.GetAxis("Vertical") != 0)
		{
			if (cow != null)
			{
				cow.hingeJoint.breakForce = .001f;
				cow.AddComponent("SphereCollider") as SphereCollider;
				cow.AddComponent("HingeJoint") as HingeJoint;
				cow= null;
			}
			
		}
	
	}
	
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.CompareTag("Livestock"))
		{
			cow = c.gameObject;
			c.gameObject.hingeJoint.connectedBody = rigidbody;
		}
		
		
		
		
	
	}
	
}
