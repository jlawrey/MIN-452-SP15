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
		halo.GetComponent<Light>().range = lightChange;
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
				
				if(cow[i].gameObject.GetComponent<HingeJoint>() != null)
				{
					halo.GetComponent<Light>().range -= lightChange;
						if (halo.GetComponent<Light>().range <= lightChange)
							halo.GetComponent<Light>().range = lightChange;
					cow[i].GetComponent<HingeJoint>().breakForce = .001f;
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
			
			
			if(c.gameObject.GetComponent<HingeJoint>() == null){
				
				c.gameObject.AddComponent<HingeJoint>();
				halo.GetComponent<Light>().range += lightChange;
				ship.SendMessage("gotObject");
			}
			
			
			if(c.gameObject.GetComponent<HingeJoint>() != null){
			//
			
			c.gameObject.GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();
			
	
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
