using UnityEngine;
using System.Collections;

public class liveStock : MonoBehaviour {
	
	private float getUpTime = 4f;
	private float getUpTimer;
	// Use this for initialization
	void Start () {
	getUpTimer = getUpTime;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
	getUpTimer -= Time.deltaTime;
	
		if (gameObject.rigidbody.velocity == Vector3.zero && getUpTimer <=0)
		{
		
			transform.rotation = new Quaternion(0,0,0,0);
			getUpTimer = getUpTimer;
		}
		
	
	}
	
	void OnCollisionEnter(Collision c)
	{
		
		if (c.gameObject.CompareTag("Portal"))
		{
			Destroy(gameObject);
		}
	}
	
}
