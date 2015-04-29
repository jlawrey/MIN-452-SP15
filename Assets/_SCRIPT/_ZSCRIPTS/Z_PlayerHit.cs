using UnityEngine;
using System.Collections;

public class Z_PlayerHit : MonoBehaviour {

	public AudioClip[] playerSounds;
	public GameObject phit;
	public GameObject MMM_Player_Score;
	public Z_Score zsc;

	// Use this for initialization
	void Awake () {

		MMM_Player_Score = GameObject.FindGameObjectWithTag ("MMM_P");
		zsc = MMM_Player_Score.GetComponent<Z_Score> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider thing){
		if (thing.gameObject.tag == "Alien" || thing.gameObject.tag == "Fireball") {
			StartCoroutine(PlayerHit());
		}
	}

	IEnumerator PlayerHit(){

		zsc.AddDeath ();
		phit.gameObject.SetActive (true);
		audio.PlayOneShot (playerSounds[0]);
		yield return new WaitForSeconds(.5f);
		phit.gameObject.SetActive (false);
	}
}
