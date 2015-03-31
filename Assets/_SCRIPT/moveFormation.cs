using UnityEngine;
using System.Collections;

public class moveFormation : MonoBehaviour {

	private Transform player;
	private float speed = 0.01f;
	// Use this for initialization
	void Start () {
	
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Vector3.Distance (transform.position, player.position) > 2) 
		{
			transform.Translate (0, 0, speed);
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
