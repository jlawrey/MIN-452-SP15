using UnityEngine;
using System.Collections;

public class moveFormation : MonoBehaviour {

	private float distance = 8f;
	private float interval = Random.Range(2f,5f);
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

			float lateralSpeed = Random.Range (0f,0.1f);
			if(goRight ){

				transform.Translate(lateralSpeed,0,0,Space.World);
			}else  {
				transform.Translate(-lateralSpeed,0,0,Space.World);
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
