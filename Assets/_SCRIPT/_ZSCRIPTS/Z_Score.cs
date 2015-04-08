using UnityEngine;
using System.Collections;

public class Z_Score : MonoBehaviour {

	public static int score = 0;
	public static int death = 0;
	public static GameObject scoretick;
	static public GameObject[] weapons;


	// Use this for initialization
	void Start () {

		scoretick = GameObject.FindGameObjectWithTag ("scoretick");
		weapons = GameObject.FindGameObjectsWithTag ("Weapon");
		foreach (GameObject weapon in weapons) {
			weapon.SetActive(false);
		}
		weapons [1].SetActive (true);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void AddScore(){

		if (score >= 3) {
			weapons[1].SetActive(false);
			weapons[0].SetActive(true);
		}
		score += 1;
		print ("score " + score);
		Vector3 flat = new Vector3(score*.04f,0,0);
		GameObject scoreguy = Resources.Load<GameObject> ("Score");
		Instantiate (scoreguy, flat, Quaternion.identity);


	}
	public static void AddDeath(){
		
		death += 1;
		print ("death " + death);
		Vector3 flat = new Vector3(death*.07f,0,0);
		GameObject deathguy = Resources.Load<GameObject> ("Death");
		Instantiate (deathguy, flat, Quaternion.identity);
		if (death > 5) {

			Application.LoadLevel(1);

		}
		
	}
}
