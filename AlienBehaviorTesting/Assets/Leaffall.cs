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
    public float fm = 10f;
    
    public Animator anim;
    
    
    
    
    
	// Use this for initialization
	void Start () {
        
        Debug.Log("2");
	
		//Randomly initlaize force direction to left or right
		if (Random.Range (0,2) == 1)
		{
        
			forceDirL = true;
			rigidbody.AddRelativeForce(0, 25.0f * fm, 15.0f * fm);
		}
		else 
		{
          
			forceDirL = false;
			rigidbody.AddRelativeForce(0, 25.0f * fm, -15.0f * fm);
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
                anim.SetTrigger("rightBoost");
                    
        		rigidbody.AddRelativeForce(0, 50.0f * fm, -30.0f * fm);
			}
			else
			{
                anim.SetTrigger("leftBoost");
				rigidbody.AddRelativeForce(0, 50.0f * fm, 30.0f * fm);
			}
			forceDirL = !forceDirL;
			nextBoost -= boostInterval;
        }
	
	}
	
	void OnCollisionEnter(Collision c)
	{
	
		if (c.gameObject.CompareTag("Ground"))
		{
			float upForce = fm * 0.1f;
			rigidbody.AddForce(new Vector3(0.0f, upForce, 0.0f));
            Debug.Log ("Ground force: "+upForce);
			nextBoost = startPosY;
		}
		
	
	}
	
}
