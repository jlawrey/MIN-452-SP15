using UnityEngine;
using System.Collections;

public class Z_Score : MonoBehaviour {

	public static int score = 0;
	public static int death = 0;
	public static GameObject scoretick;


	// Use this for initialization
	void Start () {

		score = 0;
		scoretick = GameObject.FindGameObjectWithTag ("scoretick");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void AddScore(){


		score += 1;
		print ("score " + score);
		GameObject scoreguy = Resources.Load<GameObject> ("Score");
		Vector3 flat = new Vector3(scoreguy.transform.position.x +(score*.04f),scoreguy.transform.position.y,scoreguy.transform.position.z);
		Instantiate (scoreguy, flat, Quaternion.identity);
  		

	}
	public static void AddDeath(){
		
		death += 1;
		print ("death " + death);
		GameObject deathguy = Resources.Load<GameObject> ("Death");
		Vector3 flat = new Vector3(deathguy.transform.position.x + (death*.07f),deathguy.transform.position.y,deathguy.transform.position.z);
		Instantiate (deathguy, flat, Quaternion.identity);
		if (death > 9) {

			Application.LoadLevel(4);

		}
		
	}
}
