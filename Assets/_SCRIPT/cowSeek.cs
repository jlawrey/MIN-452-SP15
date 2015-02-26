using UnityEngine;
using System.Collections;

public class cowSeek : MonoBehaviour {

	private float speed = 10f;
	private float wiggle = 5f;
	private GameObject[] livestock;
	private GameObject target;
	private float hoverHeight = 12;
	private bool hasObject;
	private GameObject portal;
	private Vector3 overTarget;
	public GameObject claw;
	// Use this for initialization
	void Start () {
	
	livestock = GameObject.FindGameObjectsWithTag("Livestock");
	portal = GameObject.FindGameObjectWithTag("Portal");
	}
	
	// Update is called once per frame
	void Update () {
	

	if (target == null)
	{
			if (livestock.Length != 0) {
			
			acquireTarget(livestock[Random.Range(0, livestock.Length)]);
			}
			else
			{
				acquireTarget(portal);
			}
	
	}
	
	
	if (hasObject)
	{
			acquireTarget(portal);
	
	}
	


	if (target != null){
		
			overTarget = new Vector3( target.transform.position.x, target.transform.position.y + hoverHeight,  target.transform.position.z);
		//transform.position = Vector3.Lerp(transform.position, , Time.deltaTime);


			float distance=Vector3.Distance(transform.position, overTarget);
			if(distance>0.2){


				transform.position = Vector3.Lerp (
					transform.position, overTarget,
					Time.deltaTime* speed/distance);
			}

			if(distance <= 0.2){

				overTarget = new Vector3( target.transform.position.x + Random.Range(-wiggle,wiggle), target.transform.position.y + hoverHeight  + Random.Range(-wiggle,wiggle),  target.transform.position.z  + Random.Range(-wiggle,wiggle));
			}


		
		
		
	
		}
		
		if (Physics.Linecast(transform.position,new Vector3(transform.position.x,transform.position.y - hoverHeight,transform.position.z), LayerMask.GetMask("Portal")))
		{

		claw.SendMessage("releaseClaw");
		
		target = null;
		
		livestock = GameObject.FindGameObjectsWithTag("Livestock");

		}


	}
	
	void acquireTarget(GameObject g)
	{
		//overTarget = new Vector3( g.transform.position.x, g.transform.position.y + hoverHeight,  g.transform.position.z);
		target = g;
	}
	
	
	void gotObject()
	{
		hasObject = true;
	}
	
	void releaseObject()
	{
		hasObject =  false;
	}
	
}
