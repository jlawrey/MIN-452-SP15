using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class spawnFormation : MonoBehaviour {

	public int numToSpawn = 1;
	public float spawnInterval = 5f;
	private float spawnTimer;
	public bool vFormation = true;
	public bool rankFormation = false;
	public bool collumnFormation = false;


	private List<GameObject> formations;
	public GameObject vFormObj;
	public GameObject rankFormObj;
	public GameObject collumnFormObj;
	public GameObject alien;

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
		spawnTimer = 1;

		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		spawnTimer -= Time.deltaTime;
		if (numToSpawn > 0) 
		{
			if (spawnTimer <= 0)
			{
				GameObject newFormation = Instantiate(formations[ Random.Range(0,formations.Count) ], transform.position, transform.rotation) as GameObject;
				newFormation.GetComponent<lookAt>().target = player.transform;
				Transform[] children = newFormation.GetComponentsInChildren<Transform>();
				foreach (Transform child in children)
				{
					GameObject newAlien = Instantiate(alien, child.position, child.rotation) as GameObject;
					newAlien.GetComponent<lookAt>().target = player.transform;
					newAlien.transform.parent = child;
					if( newAlien.transform.parent == newFormation.transform)
						Destroy(newAlien);
				}
				numToSpawn--;
				spawnTimer = spawnInterval;
			}



		}
		else
		Destroy (gameObject);
	
	}
}
