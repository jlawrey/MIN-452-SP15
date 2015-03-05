using UnityEngine;
using System.Collections;

public class SpawnAliens : MonoBehaviour {
	
	public GameObject alien;
	
	private float spawnTimer;
	private float spawnTime = 10f; 
	
	private int numSpawns = 5;

	// Use this for initialization
	void Start () {
	
		//spawnTimer = spawnTime;
		spawnTimer = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		spawnTimer -= Time.deltaTime;
		
		if (spawnTimer <= 0)
		{
			for (int i = -2 ; i < numSpawns -2; i++)
			{
				Vector3 spawnPosition = new Vector3(transform.position.x + i*1.4f + Random.Range(-4,4),transform.position.y - 4,transform.position.z + i*1.4f+ Random.Range(-4,4));
				GameObject spawn = Instantiate(alien, spawnPosition, transform.rotation) as GameObject;
				lookAt script = spawn.GetComponent<lookAt>();
				script.target = GameObject.FindGameObjectWithTag("Player").transform;
			}
			
		
		spawnTimer = spawnTime;
		
		}
		
	}
}
