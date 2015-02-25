using UnityEngine;
using System.Collections;

public class BasicShipMovementHover : MonoBehaviour {

	
	public int health = 1;
	public float moveSpeed = 10f;
	
	public GameObject flightAreaObj;
	private BoxCollider flightArea;
	private float zBound;
	private float zNegBound;
	private float xBound;
	private float xNegBound;
	private float yBound;
	private float yNegBound;
	

	
	
	// Use this for initialization
	void Start () {
	
		flightArea = flightAreaObj.collider as BoxCollider;
		zBound = (flightArea.size.z * flightAreaObj.transform.localScale.z) + flightAreaObj.transform.position.z;
		zNegBound = (flightArea.size.z * flightAreaObj.transform.localScale.z) * -1 + flightAreaObj.transform.position.z;
		xBound =  (flightArea.size.x * flightAreaObj.transform.localScale.x) + flightAreaObj.transform.position.x;
		xNegBound = (flightArea.size.x * flightAreaObj.transform.localScale.x) * -1 + flightAreaObj.transform.position.x;
		yBound =  (flightArea.size.y * flightAreaObj.transform.localScale.y) + flightAreaObj.transform.position.y;
		yNegBound = (flightArea.size.y * flightAreaObj.transform.localScale.y) * -1 + flightAreaObj.transform.position.y;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
}
