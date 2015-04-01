using UnityEngine;
using System.Collections;

public class moveFormation : MonoBehaviour {

	private float distance = 10f;
	private float interval = 2f;
	private Transform player;
	private float speed = 0.04f;
	private bool goRight = true;
	// Use this for initialization
	void Start () {
	
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Vector3.Distance (transform.position, player.position) > distance) 
		{
			transform.Translate (0, 0, speed);

			if (transform.position.x  >  player.position.x + interval)
			{ goRight = false;}
			if (transform.position.x  <  player.position.x - interval)
			{ goRight = true;}

			if(goRight ){

				transform.Translate(.02f,0,0,Space.World);
			}else  {
				transform.Translate(-.02f,0,0,Space.World);
			}

		}


		else 
		{
			foreach (Transform child in transform)
			{
				child.DetachChildren();
				Destroy(child.gameObject);
			}
			Destroy(gameObject);
		}
	
	}
}
