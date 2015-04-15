using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour 
{
	
	 int damage = 1;
	 float lifespan = 5f;
	 float force = 700;
	public GameObject deathParticle;
	
	// Use this for initialization
	void Start () 
	{
		rigidbody.AddRelativeForce (0, 0, force);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		lifespan -= Time.deltaTime;

		
		if (lifespan <= 0)
		{
			Destroy(gameObject);
			Vector3 particlePosition = new Vector3(transform.position.x,transform.position.y, transform.position.z);
			Instantiate (deathParticle, particlePosition, Quaternion.identity);
			Destroy(deathParticle, 0.3f);
		}
	}
	
	void OnTriggerEnter(Collider target)
	{

		if (target.tag == "Shield" && target.tag == "Weapon") {

			rigidbody.velocity = new Vector3 (-rigidbody.velocity.x, -rigidbody.velocity.y, -rigidbody.velocity.z);
		} else {
			Vector3 particlePosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			Instantiate (deathParticle, particlePosition, Quaternion.identity);
			Destroy (deathParticle, 0.3f);
		}
	}
	
}
