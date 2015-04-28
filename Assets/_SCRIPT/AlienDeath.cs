using UnityEngine;
using System.Collections;

public class AlienDeath : MonoBehaviour {

	public GameObject deathParticle;
	private float upShift = 2f;
	public AudioClip[] deathSounds;
	public GameObject MMM_Player_Score;
	public Z_Score zsc;

	// Use this for initialization
	void Awake () {
	
		MMM_Player_Score = GameObject.FindGameObjectWithTag ("MMM_P");
		zsc = MMM_Player_Score.GetComponent<Z_Score> ();
	}


	void OnParticleCollision(GameObject thing){

			print ("the particle collided");
			StartCoroutine(explode ());
			zsc.AddScore ();
	}

	void OnCollisionEnter(Collision c)
	{
		//print (c.gameObject.name);
		if (c.gameObject.tag == "Weapon")
		{
			print (gameObject.name + " hit with " + c.gameObject.name);
			StartCoroutine(explode ());
			zsc.AddScore ();

		}

		if (c.gameObject.tag == "Fireball")
		{
			StartCoroutine(explode ());
		}

		if (c.gameObject.tag == "Shield") {
			audio.PlayOneShot(deathSounds[1]);
		}
		
	}

	void OnTriggerEnter(Collider target)
	{
		if(target.tag == "Player" || target.tag == "Miss")
		{
			StartCoroutine( didHitPlayer () );
		}

		if (target.tag == "Fireball")
		{
			StartCoroutine(explode ());
		}

	}

	public IEnumerator didHitPlayer()
	{

		//Wii.SetRumble (1, true);
		yield return new WaitForSeconds(.5f);
		//Wii.SetRumble (1, false);
		Destroy (gameObject);
	}

	IEnumerator explode()
	{
		print ("Hit Explode");
		audio.PlayOneShot(deathSounds[0]);
		Vector3 particlePosition = new Vector3(transform.position.x,transform.position.y, transform.position.z - 1);
		Instantiate (deathParticle, particlePosition, Quaternion.identity);
		//Destroy(deathParticle, 1);
		yield return new WaitForSeconds (deathSounds[0].length);
		Destroy(gameObject);

	}
}
