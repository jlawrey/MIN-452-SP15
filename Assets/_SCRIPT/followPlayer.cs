using UnityEngine;
using System.Collections;

public class followPlayer : MonoBehaviour {
	
	private float flyHeight = 1.5f;
	private float yBoost = 20f;
	private float moveForce = 50f;
	private float maxSpeed = 2f;
	private float orbitSpeed = 1f;
	private float orbitCorrect = 5f;
	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
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
		if (Vector3.Distance(transform.position, player.transform.position) < 2f )
		{
			rigidbody.velocity = Vector3.zero;
		}
		
	}	
	
}
