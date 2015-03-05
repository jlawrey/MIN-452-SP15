using UnityEngine;
using System.Collections;

public class followPlayer : MonoBehaviour {
	
	private float flyHeight = 2f;
	private float yBoost = 20f;
	private float moveForce = 10f;
	private float maxSpeed = 5f;
	private float orbitSpeed = 5f;
	private float orbitCorrect = 5f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Ray rayToGround =  new Ray(transform.position, Vector3.down);
		RaycastHit hit;
		Physics.Raycast(rayToGround, out hit);
		if (hit.distance <= flyHeight)
		{
		
			rigidbody.AddForce(new Vector3(0,yBoost,0));
		}
		if (transform.InverseTransformDirection(rigidbody.velocity).z < maxSpeed)
			rigidbody.AddRelativeForce(new Vector3(0,0,moveForce));
		
		if (transform.InverseTransformDirection(rigidbody.velocity).x > orbitSpeed )
		{

			rigidbody.AddRelativeForce(-orbitCorrect,0,0);
		}
		if (transform.InverseTransformDirection(rigidbody.velocity).x < -orbitSpeed )
		{

			rigidbody.AddRelativeForce(orbitCorrect,0,0);
		}
		
		
	}
}
