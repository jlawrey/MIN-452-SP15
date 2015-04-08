using UnityEngine;
using System.Collections;

public class MoveHorizontal : MonoBehaviour {


	bool goRight = false;
	Transform player ;
	float interval = 6;
	float lateralSpeed = 0.05f;
	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").transform;

	}
	
	// Update is called once per frame
	void FixedUpdate () {


		
		if (transform.position.x  >  player.position.x + interval)
		{ goRight = false;}
		if (transform.position.x  <  player.position.x - interval)
		{ goRight = true;}
		

		if(goRight ){
			
			transform.Translate(lateralSpeed,0,0,Space.World);
		}else  {
			transform.Translate(-lateralSpeed,0,0,Space.World);
		}
	
	}
}
