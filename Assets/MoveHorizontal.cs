using UnityEngine;
using System.Collections;

public class MoveHorizontal : MonoBehaviour {

	float speed= 2f;
	bool goRight = false;
	Transform player ;
	float interval = 4;
	float lateralSpeed;
	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").transform;
		lateralSpeed = Random.Range (0f,0.1f);
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
