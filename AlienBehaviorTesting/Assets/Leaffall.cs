using UnityEngine;
using System.Collections;

public class Leaffall : MonoBehaviour {
	
	//Start postion of object
	private float startPosY;
	
	//Direction to apply the boost force
    private bool forceDirL;
	public float boostInterval = 1.0f;
	
    private float nextBoost;
    
    //force multiplier
    public float fm = 3000f;
    
    
    
    
    
    
	// Use this for initialization
	void Start () {
	
		//Randomly initlaize force direction to left or right
		if (Random.Range (0,2) == 1)
		{
			forceDirL = true;
			rigidbody.AddForce(0, 25.0f * fm, 15.0f * fm);
		}
		else 
		{
			forceDirL = false;
			rigidbody.AddForce(0, 25.0f * fm, -15.0f * fm);
		}
			
		//initialize startPos and nextBoost
		startPosY = transform.position.y;
		nextBoost = startPosY- boostInterval;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (transform.position.y <= nextBoost)
        {
        	if (forceDirL)
        	{
        		rigidbody.AddForce(0, 50.0f * fm, -30.0f * fm);
			}
			else
			{
				rigidbody.AddForce(0, 50.0f * fm, 30.0f * fm);
			}
			forceDirL = !forceDirL;
			nextBoost -= boostInterval;
        }
	
	}
	
	void OnCollisionEnter(Collision c)
	{
	
		if (c.gameObject.CompareTag("Ground"))
		{
			Debug.Log ("Ground Force");
			rigidbody.AddForce(new Vector3(0.0f, (1f * fm), 0.0f));
			nextBoost = startPosY- boostInterval;
		}
		
	
	}
	
}
