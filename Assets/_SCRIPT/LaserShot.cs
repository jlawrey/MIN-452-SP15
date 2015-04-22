using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour 
{
	
	 int damage = 1;

	 public float force = 700;
	public GameObject deathParticle;
	
	// Use this for initialization
	void Start () 
	{
		rigidbody.AddRelativeForce (0, 0, force);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{

	}
	
	void OnTriggerEnter(Collider target)
	{

		//if (target.tag == "Shield" || target.tag == "Weapon") 
		
		//if (target.tag != "Player" && target.tag != "Miss")
		if (target.tag == "Shield" || target.tag == "Weapon" || target.tag == "Alien" ) 
		{


			transform.rotation = new Quaternion(-transform.rotation.x,-transform.rotation.y, transform.rotation.z, transform.rotation.w);
			//transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z +1);
			rigidbody.velocity = new Vector3 (rigidbody.velocity.x, rigidbody.velocity.y, -rigidbody.velocity.z);

		}
	}


	
}
