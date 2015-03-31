using UnityEngine;
using System.Collections;

public class jetpackHover : MonoBehaviour {
	private float flyHeight = 0.5f;
	private float yBoost = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Ray rayToGround =  new Ray(transform.position, Vector3.down);
		RaycastHit hit;
		Physics.Raycast(rayToGround, out hit);
		if (hit.distance <= flyHeight)
		{
			
			rigidbody.AddForce(new Vector3(0,yBoost,0));
		}

	
	}
}
