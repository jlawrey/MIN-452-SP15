using UnityEngine;
using System.Collections;

public class AlienDeath : MonoBehaviour {

	public GameObject deathParticle;
	private float upShift = 2f;
	public AudioClip[] deathSounds;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter(Collision c)
	{
		print (c.gameObject.name);
		if (c.gameObject.tag == "Weapon")
		{
			StartCoroutine(explode ());
			Z_Score.AddScore();

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

		audio.PlayOneShot(deathSounds[0]);
		Vector3 particlePosition = new Vector3(transform.position.x,transform.position.y, transform.position.z);
		Instantiate (deathParticle, particlePosition, Quaternion.identity);
		//Destroy(deathParticle, 1);
		yield return new WaitForSeconds (.3f);
		Destroy(gameObject);

	}
}
