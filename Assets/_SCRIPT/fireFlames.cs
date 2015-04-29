using UnityEngine;
using System.Collections;

public class fireFlames : MonoBehaviour {

	float fireRate = 12;
	float fireTimer;
	float fireDelay = 2f;
	float fireForce = 1000f;
	public GameObject flame;
	ParticleRenderer[] renderers;
	Transform player;
	float startRotate;
	float endRotate;
	bool isFiring = false;

	// Use this for initialization
	void Start () {
		fireTimer = 3;
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		//transform.LookAt (player);
		startRotate = transform.rotation.x;
		endRotate = transform.rotation.x + 90;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		fireTimer -= Time.deltaTime;
		if (fireTimer <= 0 )
		{
			if(transform.rotation.x < endRotate)
			{
			
				Quaternion newRotation = Quaternion.AngleAxis(90, -Vector3.left);
				transform.rotation= Quaternion.Slerp(transform.rotation, newRotation, .05f);  
			}


			if(!isFiring)
			{
				StartCoroutine("fireFlamethrower");

			}


		}
		// > startRotate
		if (!isFiring && transform.rotation.x > startRotate) {


			Quaternion newRotation = Quaternion.AngleAxis(-90, -Vector3.left);
			transform.rotation= Quaternion.Slerp(transform.rotation, newRotation, .05f);  
		}
	
	}

	public IEnumerator fireFlamethrower()
	{
	
		isFiring = true;
		yield return new WaitForSeconds (fireDelay);
		Vector3 fbPostion = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 5);
		GameObject fireball = Instantiate (flame, fbPostion , new Quaternion(0,0,0,0)) as GameObject;
		fireball.transform.LookAt(player);
		fireball.GetComponent<Rigidbody>().AddRelativeForce(0,0,fireForce);
		fireTimer = fireRate;
		isFiring = false;

	}



	
}
