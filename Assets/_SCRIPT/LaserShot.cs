using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour {
	
	public int damage = 1;
	public float lifespan = 1f;
	public float speed = 3;
	
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		lifespan -= Time.deltaTime;
		transform.Translate(new Vector3(0,0,speed));
		
		if (lifespan <= 0)
		{
			Destroy(gameObject);
		}
	}
	
	/*void OnCollisionEnter(Collision collision)
	{
		Debug.Log("hit");
		Destroy(gameObject);
	}*/
	
}
