using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class spawnFormation : MonoBehaviour {

	public int globalDelay = 6;
	public int numToSpawn = 1;
	public float spawnInterval = 5f;
	private float spawnTimer;
	public bool vFormation = true;
	public bool rankFormation = false;
	public bool collumnFormation = false;

	public bool jetpackAlien = true;
	public bool eyeballAlien = true;
	public float eyballSpawnChance;

	private List<GameObject> formations;
	public GameObject vFormObj;
	public GameObject rankFormObj;
	public GameObject collumnFormObj;

	public GameObject jetpackAlienObj;
	public GameObject eyeballAlienObj;

	private List<GameObject> aliens;
	private GameObject player;

	// Use this for initialization
	void Start () 
	{
		formations = new List<GameObject>();
		if (vFormation)
			formations.Add(vFormObj);
		if (rankFormation)
			formations.Add(rankFormObj);
		if (collumnFormation)
			formations.Add(collumnFormObj);

		aliens = new List<GameObject>();
		if (jetpackAlien)
			aliens.Add(jetpackAlienObj);
		if (eyeballAlien)
			aliens.Add(eyeballAlienObj);


		spawnTimer = 1;

		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		spawnTimer -= Time.deltaTime;
		if (numToSpawn > 0) 
		{
			if (spawnTimer <= -globalDelay)
			{
				GameObject newFormation = Instantiate(formations[ Random.Range(0,formations.Count) ], transform.position, Quaternion.identity) as GameObject;
				newFormation.GetComponent<lookAt>().target = player.transform;
				Transform[] children = newFormation.GetComponentsInChildren<Transform>();
				foreach (Transform child in children)
				{
					GameObject typeToSpawn = aliens[0];
					if (eyeballAlien && Random.Range(0f,1f) < eyballSpawnChance)
						typeToSpawn = aliens[1];

					GameObject newAlien = Instantiate( typeToSpawn, child.position, Quaternion.identity) as GameObject;

					if(newAlien.GetComponent<lookAt>() != null){
						newAlien.GetComponent<lookAt>().target = player.transform;
					}
					if(newAlien.GetComponent<Z_alien_AI_01>() != null){
						newAlien.GetComponent<Z_alien_AI_01>().target = player.transform;
					}
					newAlien.transform.parent = child;
					if( newAlien.transform.parent == newFormation.transform)
						Destroy(newAlien);
				}
				numToSpawn--;
				spawnTimer = spawnInterval;
				globalDelay = 2;
			}



		}
		else
		Destroy (gameObject);
	
	}
}
