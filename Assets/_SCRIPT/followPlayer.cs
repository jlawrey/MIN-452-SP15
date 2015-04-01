using UnityEngine;
using System.Collections;

public class followPlayer : MonoBehaviour {
	

	private float force = 100f;
	private float moveForce = 500f;
	private float maxSpeed = .5f;
	private float orbitSpeed = 1f;
	private float orbitCorrect = 5f;
	private GameObject player;
	private float waitTime;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		waitTime = Random.Range (0, 1f);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{


		if (transform.parent == null) {

			waitTime -= Time.deltaTime;
			if (waitTime <= 0){

			

			if (transform.InverseTransformDirection (rigidbody.velocity).z < maxSpeed)
				rigidbody.AddRelativeForce (new Vector3 (0, 0, moveForce));
		
			if (transform.InverseTransformDirection (rigidbody.velocity).x > orbitSpeed) {

				rigidbody.AddRelativeForce (-orbitCorrect, 0, 0);
			}
			if (transform.InverseTransformDirection (rigidbody.velocity).x < -orbitSpeed) {

				rigidbody.AddRelativeForce (orbitCorrect, 0, 0);
			}
			}
			/*if (Vector3.Distance (transform.position, player.transform.position) < 2f) {
				transform.position = player.transform.position;

			}*/
		}
		
	}	

/*public IEnumerator Attack(){
		
		float waittime = Random.Range (1, 18.5f);
		yield return new WaitForSeconds (waittime);
		//isAttacking = true;
		//print (waittime + " ATTACK!!!");
		gameObject.rigidbody.AddRelativeForce (0, 0, force);
		gameObject.rigidbody.AddRelativeTorque (force/4,0,0);
		
	}*/
	
}
