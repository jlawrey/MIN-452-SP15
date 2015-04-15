using UnityEngine;
using System.Collections;

public class fireballDestruction : MonoBehaviour {

	private float lifespan = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	lifespan -= Time.deltaTime;
	if (lifespan <= 0) {
			Destroy(gameObject);
		}


	}

	void OnTriggerEnter(Collider target)
	{
		if(target.tag == "Player" || target.tag == "Miss")
		{
			Destroy(gameObject, 0.5f);
		}
		
		
	}

}
