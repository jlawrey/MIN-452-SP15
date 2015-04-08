using UnityEngine;
using System.Collections;

public class fireFlames : MonoBehaviour {

	float fireRate = 8;
	float fireTimer;
	float fireDuration = 5f;
	public GameObject flame;
	ParticleRenderer[] renderers;

	float startRotate;
	float endRotate;
	bool isFiring = false;

	// Use this for initialization
	void Start () {
		fireTimer = 3;
		renderers = flame.GetComponentsInChildren<ParticleRenderer> ();
		for (int i= 0; i < renderers.Length; i++) {
			renderers [i].enabled = false;
		}

		transform.LookAt (GameObject.FindGameObjectWithTag("Player").transform);
		startRotate = transform.rotation.x;
		endRotate = transform.rotation.x - 90;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		fireTimer -= Time.deltaTime;
		if (fireTimer <= 0 )
		{
			if(transform.rotation.x > endRotate)
			{
				Quaternion newRotation = Quaternion.AngleAxis(90, -Vector3.left);
				transform.rotation= Quaternion.Slerp(transform.rotation, newRotation, .05f);  
			}


			if(!isFiring)
			{
				StartCoroutine("fireFlamethrower");

			}


		}

		if (!isFiring & transform.rotation.x > startRotate) {

			Quaternion newRotation = Quaternion.AngleAxis(-90, -Vector3.left);
			transform.rotation= Quaternion.Slerp(transform.rotation, newRotation, .05f);  
		}
	
	}

	public IEnumerator fireFlamethrower()
	{
	
		isFiring = true;


		for (int i= 0; i < renderers.Length; i++) {
			renderers [i].enabled = true;
		}

		yield return new WaitForSeconds (fireDuration);

		for (int i= 0; i < renderers.Length; i++) {
			renderers [i].enabled = false;
		}
		fireTimer = fireRate;
		isFiring = false;

	}



	
}
