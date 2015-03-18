using UnityEngine;
using System.Collections;

public class AlienDeath : MonoBehaviour {

	public GameObject deathParticle;
	private float upShift = 2f;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter(Collision c)
	{
	
		if (c.gameObject.CompareTag("Weapon"))
		{
		
			explode ();
		
		}
		
	}

	void OnTriggerEnter(Collider target)
	{
		if(target.tag == "Player")
		{
			didHitPlayer ();
		}
			

	}

	void didHitPlayer()
	{
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
