using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawnMenuAlien : MonoBehaviour 
{

		private float spawnInterval = 4f;
		private float spawnTimer;

		public GameObject jetpackAlienObj;
		public GameObject eyeballAlienObj;
		
		private List<GameObject> aliens;

		
		// Use this for initialization
		void Start () 
		{

			aliens = new List<GameObject>();

				aliens.Add(jetpackAlienObj);

				aliens.Add(eyeballAlienObj);
			
			
			spawnTimer = spawnInterval;
			

		}
		
		// Update is called once per frame
		void FixedUpdate () 
		{

				spawnTimer-= Time.deltaTime;

	
				if (spawnTimer <= 0)
				{
					    Debug.Log("Should spawn");
						GameObject typeToSpawn = aliens[0];
						if (Random.Range(0f,1f) < .5)
							typeToSpawn = aliens[1];
						
						GameObject newAlien = Instantiate( typeToSpawn, gameObject.transform.position, Quaternion.identity) as GameObject;
						string randAnim = "MenuAnim1";
						newAlien.GetComponent<Animator>().Play(randAnim);
						Destroy(newAlien, 5);
						spawnTimer = spawnInterval;
				}
				
					

			}
			
			
	}