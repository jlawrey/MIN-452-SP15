using UnityEngine;
using System.Collections;

public class pickUp : MonoBehaviour {
	
	public GameObject ship;
	private GameObject[] cow;
	public GameObject halo;
	private float lightChange = 1f;
	private bool release;
	
	void Start () {
		cow = new GameObject[GameObject.FindGameObjectsWithTag("Livestock").Length];
		halo.light.range = lightChange;
		release  = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if (release)
		{
			
		 for (int i = 0 ; i < cow.Length ; i++)
		 {

		 
			if (cow[i] != null)
			{
				
				if(cow[i].gameObject.hingeJoint != null)
				{
					halo.light.range -= lightChange;
						if (halo.light.range <= lightChange)
							halo.light.range = lightChange;
					cow[i].hingeJoint.breakForce = .001f;
					cow[i]= null;
					ship.SendMessage("releaseObject");
					
				}
				
				
			}
		  }
		release = false;
		
		}
	
	}
	
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.CompareTag("Livestock"))
		{
			
			
			if(c.gameObject.hingeJoint == null){
				
				c.gameObject.AddComponent("HingeJoint");
				halo.light.range += lightChange;
				ship.SendMessage("gotObject");
			}
			
			
			if(c.gameObject.hingeJoint != null){
			//
			
			c.gameObject.hingeJoint.connectedBody = rigidbody;
			
	
			}
			
			for (int k = 0 ; k < cow.Length ; k++)
			{
				if (cow[k] == null)
				{
					cow[k] = c.gameObject;
					k = cow.Length;//Exit loop
				}
			}
		}	
		
	
	}
	
	void releaseClaw()
	{
		release = true;
	}
	
	
}
