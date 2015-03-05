using UnityEngine;
using System.Collections;

public class keyMoveCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public float speed = 1f;
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey("up"))
		{
			transform.Translate(0,0,speed, null);
		}
		
		if (Input.GetKey("down"))
		{
			transform.Translate(0,0,-speed, null);
		}
		
		if (Input.GetKey("left"))
		{
			transform.Translate(speed,0,0, null);
		}
		
		if (Input.GetKey("right"))
		{
			transform.Translate(-speed,0,0, null);
		}
		
		
		
	
	
	}
}
