using UnityEngine;
using System.Collections;

public class BasicShipMovementRandom : MonoBehaviour {
	

	//Must assign flight area object with box collider set to trigger
	public GameObject flightAreaObj;
	private BoxCollider flightArea;

	//Bounds set by collider
	private float zBound;
	private float zNegBound;
	private float xBound;
	private float xNegBound;
	private float yBound;
	private float yNegBound;
	

	private Vector3 nextBoostV;//Velocity for next boost
	private float nextBoostT;// Timer for next boost

	public float boostInterval = 1f;//How often to boost

	//Start postion of ship
	private float startPosY;
	
    //force multiplier
    public float fm = 5f;
    
	public float minBoost = 30f;
    public float maxBoost = 60f;
	public float minBoostY = 150f;
	public float maxBoostY = 300f;

	private float nextBoostZ;
	private float nextBoostX;
	private float nextBoostY;
    
    public Animator anim;//Connection to animator
    
    private float z;
    private float x;
	private float y;
    
    
    
	// Use this for initialization
	void Start () {
	
		flightArea = flightAreaObj.GetComponent<Collider>() as BoxCollider;

		//Find uppder and lower bounds of flight area
		zBound = (flightArea.size.z * flightAreaObj.transform.localScale.z) + flightAreaObj.transform.position.z;
		zNegBound = (flightArea.size.z * flightAreaObj.transform.localScale.z) * -1 + flightAreaObj.transform.position.z;
		xBound =  (flightArea.size.x * flightAreaObj.transform.localScale.x) + flightAreaObj.transform.position.x;
		xNegBound = (flightArea.size.x * flightAreaObj.transform.localScale.x) * -1 + flightAreaObj.transform.position.x;
		yBound =  (flightArea.size.y * flightAreaObj.transform.localScale.y) + flightAreaObj.transform.position.y;
		yNegBound = (flightArea.size.y * flightAreaObj.transform.localScale.y) * -1 + flightAreaObj.transform.position.y;
		
      	//Initialize boost timer
		nextBoostT = boostInterval;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
    	nextBoostT -= Time.deltaTime;//Decrement timer
    	
        if (nextBoostT <= 0)//If a boost is to be applied
        {

			//find ship position
			z = transform.position.z;
			x = transform.position.x;
			y = transform.position.y;

			//If ship is within all the bounds, set to randomly boost
			if (z < zBound && z > zNegBound && x < xBound && x > xNegBound && y < yBound && y > yNegBound  )
			{
				
				nextBoostZ = Random.Range(-maxBoost,maxBoost);
				nextBoostX = Random.Range(-maxBoost,maxBoost);
				nextBoostY = Random.Range(minBoostY, maxBoostY);
			}

			//If ship is above Z bound, stop it in Z, set to boost it only negativley in Z
			if (z > zBound)
			{
				GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x,GetComponent<Rigidbody>().velocity.y,0);
				nextBoostZ = Random.Range(minBoost,maxBoost) * -1;
				;
				
			}
			//If ship is below neg Z bound, stop it in Z, set to boost it only pos in Z
			if (z < zNegBound)
			{
				GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x,GetComponent<Rigidbody>().velocity.y,0);
				nextBoostZ = Random.Range(minBoost,maxBoost) ;
				
			}
			//If ship is above X bound, stop it in X, set to boost it only negativley in X
			if (x > xBound)
			{
				GetComponent<Rigidbody>().velocity = new Vector3(0,GetComponent<Rigidbody>().velocity.y,GetComponent<Rigidbody>().velocity.z);
				nextBoostX = Random.Range(minBoost,maxBoost) * -1;
				//anim.SetTrigger("leftBoost");
			}
			//If ship is above neg X bound, stop it in X, set to boost it only pos in X
			if ( x < xNegBound)
			{
				GetComponent<Rigidbody>().velocity = new Vector3(0,GetComponent<Rigidbody>().velocity.y,GetComponent<Rigidbody>().velocity.z);
				nextBoostX = Random.Range(minBoost,maxBoost);
			}

			//If ship is above Y bound, stop it in Y, set to no y boost
			if (y > yBound)
			{
				GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x,0,GetComponent<Rigidbody>().velocity.z);
				nextBoostY = 0;
			}

			//If ship is below neg Y bound, stop it in Y, set to max Y boost
			if ( y < yNegBound)
			{

				GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x,0,GetComponent<Rigidbody>().velocity.z);
				nextBoostY = maxBoostY;
				
				
			}
			
			//Compile boost for each axis
			nextBoostV = new Vector3(nextBoostX*fm,nextBoostY*fm,nextBoostZ*fm);

			//set anim state based on y or x boosts
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
			
			//Apply the boost
			GetComponent<Rigidbody>().AddForce(nextBoostV);

			//Reset boost timer
			nextBoostT = boostInterval;
		}
	}
	
	void OnCollisionEnter(Collision c)
	{
		//If ship hits the ground, max boost it
		if (c.gameObject.CompareTag("Ground"))
		{
			float upForce = fm * maxBoostY;
			GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, upForce, 0.0f));   
			nextBoostT = boostInterval;
		}
		
	
	}
	
}
