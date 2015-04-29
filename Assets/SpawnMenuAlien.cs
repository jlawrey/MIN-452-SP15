using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawnMenuAlien : MonoBehaviour 
{

		private float spawnInterval = 8f;
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
						if (Random.Range(0f,1f) < 0.5f)
							typeToSpawn = aliens[1];
						
						GameObject newAlien = Instantiate( typeToSpawn, gameObject.transform.position, Quaternion.identity) as GameObject;
					string randAnim;
					float roll = Random.Range(0,100);
					if (roll < 33)
						randAnim = "MenuAnim1";
					else if (roll >= 33 && roll < 66)
						randAnim = "MenuAnim2";
					else
						randAnim = "MenuAnim3";

					newAlien.GetComponent<Animator>().Play(randAnim);
					spawnTimer = spawnInterval;
				}
				
					

			}
			
			
	}