using UnityEngine;
using System.Collections;

public class BasicShipMovementRandom : MonoBehaviour {
	
	
	public GameObject flightAreaObj;
	private BoxCollider flightArea;
	private float zBound;
	private float zNegBound;
	private float xBound;
	private float xNegBound;
	private float yBound;
	private float yNegBound;
	
	
	private Vector3 nextBoostV;
	
	//Start postion of object
	private float startPosY;
	
	//Direction to apply the boost force
    private bool forceDirL;
	public float boostInterval = 50f;
	
    private float nextBoostT;

    
    //force multiplier
    public float fm = 5f;
    
	public float minBoost = 30f;
    public float maxBoost = 60f;
	public float minBoostY = 150f;
	public float maxBoostY = 300f;
	private float nextBoostZ;
	private float nextBoostX;
	private float nextBoostY;
    
    public Animator anim;
    
    private float z;
    private float x;
	private float y;
    
    
    
	// Use this for initialization
	void Start () {
	
		flightArea = flightAreaObj.collider as BoxCollider;
		zBound = (flightArea.size.z * flightAreaObj.transform.localScale.z) + flightAreaObj.transform.position.z;
		zNegBound = (flightArea.size.z * flightAreaObj.transform.localScale.z) * -1 + flightAreaObj.transform.position.z;
		xBound =  (flightArea.size.x * flightAreaObj.transform.localScale.x) + flightAreaObj.transform.position.x;
		xNegBound = (flightArea.size.x * flightAreaObj.transform.localScale.x) * -1 + flightAreaObj.transform.position.x;
		yBound =  (flightArea.size.y * flightAreaObj.transform.localScale.y) + flightAreaObj.transform.position.y;
		yNegBound = (flightArea.size.y * flightAreaObj.transform.localScale.y) * -1 + flightAreaObj.transform.position.y;
		
      	
		nextBoostT = 40;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
    	nextBoostT--;
    	
        if (nextBoostT <= 0) 
        {
			Debug.Log("boost timer down");
			z = transform.position.z;
			x = transform.position.x;
			y = transform.position.y;
			
			if (z < zBound && z > zNegBound && x < xBound && x > xNegBound && y < yBound && y > yNegBound  )
			{
				
				nextBoostZ = Random.Range(-maxBoost,maxBoost);
				nextBoostX = Random.Range(-maxBoost,maxBoost);
				nextBoostY = Random.Range(minBoostY, maxBoostY);
			}
			if (z > zBound)
			{
				rigidbody.velocity = new Vector3(rigidbody.velocity.x,rigidbody.velocity.y,0);
				nextBoostZ = Random.Range(minBoost,maxBoost) * -1;
				;
				
			}
			if (z < zNegBound)
			{
				rigidbody.velocity = new Vector3(rigidbody.velocity.x,rigidbody.velocity.y,0);
				nextBoostZ = Random.Range(minBoost,maxBoost) ;
				
			}
			if (x > xBound)
			{
				rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,rigidbody.velocity.z);
				nextBoostX = Random.Range(minBoost,maxBoost) * -1;
				anim.SetTrigger("leftBoost");
			}
			if ( x < xNegBound)
			{
				rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,rigidbody.velocity.z);
				nextBoostX = Random.Range(minBoost,maxBoost);
				
				
			}
			
			if (y > yBound)
			{
				rigidbody.velocity = new Vector3(rigidbody.velocity.x,0,rigidbody.velocity.z);
				Debug.Log("aboveY");
				nextBoostY = 0;
				
				
			}
			if ( y < yNegBound)
			{
				Debug.Log("belowY");
				rigidbody.velocity = new Vector3(rigidbody.velocity.x,0,rigidbody.velocity.z);
				nextBoostY = maxBoostY;
				
				
			}
			
			
			nextBoostV = new Vector3(nextBoostX*fm,nextBoostY*fm,nextBoostZ*fm);
			if (nextBoostY >= maxBoost)
			{
				anim.SetTrigger("bothBoost");
			}
			else if (nextBoostX > 0)
			{
				anim.SetTrigger("leftBoost");
			}
			else if (nextBoostX <0)
			{
				anim.SetTrigger("rightBoost");
			}
			
			
			rigidbody.AddForce(nextBoostV);
			
			Debug.Log("boost happened"+nextBoostV);
			nextBoostT = boostInterval;
		}
	}
	
	void OnCollisionEnter(Collision c)
	{
	
		if (c.gameObject.CompareTag("Ground"))
		{
			float upForce = fm * 0.001f;
			rigidbody.AddForce(new Vector3(0.0f, upForce, 0.0f));
            Debug.Log ("Ground force: "+upForce);
			nextBoostT = startPosY;
		}
		
	
	}
	
}
