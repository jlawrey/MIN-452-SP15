using UnityEngine;
using System.Collections;

public class cowSeek : MonoBehaviour {
	
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

			acquireTarget(livestock[Random.Range(0, livestock.Length)]);
	
	}
	
	
	if (hasObject)
	{
			acquireTarget(portal);
	
	}
	
	if (target != null){
		
		transform.position = Vector3.Lerp(transform.position, overTarget, Time.deltaTime);
		overTarget = new Vector3( target.transform.position.x, target.transform.position.y + hoverHeight,  target.transform.position.z);
	
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
