using UnityEngine;
using System.Collections;

public class moveVertical : MonoBehaviour {


	bool goUp = false;
	Transform player ;
	float bottom = 2;
	float top = 8;
	float verticalSpeed = 0.05f;

	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").transform;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (transform.position.y  >  player.position.y + top)
		{ goUp = false;}
		if (transform.position.y  <  player.position.y -  bottom)
		{ goUp = true;}
		
		
		if(goUp ){
			
			transform.Translate(0,verticalSpeed,0,Space.World);
		}else  {
			transform.Translate(0,-verticalSpeed,0,Space.World);
		}
	
	}
}
