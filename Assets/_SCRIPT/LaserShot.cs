﻿using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour 
{
	
	 int damage = 1;
	 float lifespan = 5f;
	 float force = 700;
	
	
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
		}
	}
	
	void OnTriggerEnter(Collider target)
	{

		if (target.tag != "Player" && target.tag != "Miss") {

			rigidbody.velocity = new Vector3 (-rigidbody.velocity.x, -rigidbody.velocity.y, -rigidbody.velocity.z);
		}
	}
	
}
