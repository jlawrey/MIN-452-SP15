using UnityEngine;
using System.Collections;

public class fireFlames : MonoBehaviour {

	float fireRate = 8;
	float fireTimer;
	float waittime = 3f;
	public GameObject flame;
	ParticleRenderer[] renderers;
	// Use this for initialization
	void Start () {
		fireTimer = 3;
		renderers = flame.GetComponentsInChildren<ParticleRenderer> ();
		for (int i= 0; i < renderers.Length; i++) {
			renderers [i].enabled = false;
		}

		transform.LookAt (GameObject.FindGameObjectWithTag("Player").transform);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		fireTimer -= Time.deltaTime;
		if (fireTimer <= 0)
		{
			StartCoroutine("fireFlamethrower");
			Quaternion newRotation = Quaternion.AngleAxis(90, Vector3.up);
			transform.rotation= Quaternion.Slerp(transform.rotation, newRotation, .05f);  
			fireTimer = fireRate;



		}
	
	}

	public IEnumerator fireFlamethrower()
	{
		transform.Rotate(-90,0,0);


		for (int i= 0; i < renderers.Length; i++) {
			renderers [i].enabled = true;
		}

		yield return new WaitForSeconds (waittime);

		for (int i= 0; i < renderers.Length; i++) {
			renderers [i].enabled = false;
		}

		transform.Rotate(90,0,0);
	}



	
}
