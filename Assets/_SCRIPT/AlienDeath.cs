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
	
		if (c.gameObject.tag == "Weapon")
		{
			explode ();
			audio.PlayOneShot(deathSounds[0]);
		}
		if (c.gameObject.tag == "Shield") {
			audio.PlayOneShot(deathSounds[1]);
		}
		
	}

	void OnTriggerEnter(Collider target)
	{
		if(target.tag == "Player")
		{
			StartCoroutine( didHitPlayer () );
		}
			

	}

	public IEnumerator didHitPlayer()
	{

		//Wii.SetRumble (1, true);
		yield return new WaitForSeconds(.1f);
		//Wii.SetRumble (1, false);
		Destroy (gameObject);
	}

	void explode()
	{
		Vector3 particlePosition = new Vector3(transform.position.x,transform.position.y+ upShift,transform.position.z);
		deathParticle = Instantiate(deathParticle, particlePosition, transform.rotation) as GameObject;
		//deathParticle.rigidbody.velocity = rigidbody.velocity;
		Destroy(deathParticle, 1);
		Destroy(gameObject);

	}
}
