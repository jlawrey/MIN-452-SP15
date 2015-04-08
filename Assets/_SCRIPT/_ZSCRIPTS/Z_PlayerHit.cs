using UnityEngine;
using System.Collections;

public class Z_PlayerHit : MonoBehaviour {

	public AudioClip[] playerSounds;
	public GameObject phit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider thing){
		if (thing.gameObject.tag == "Alien") {
			StartCoroutine(PlayerHit());
		}
	}

	IEnumerator PlayerHit(){

		Z_Score.AddDeath ();
		phit.gameObject.SetActive (true);
		audio.PlayOneShot (playerSounds[0]);
		yield return new WaitForSeconds(.5f);
		phit.gameObject.SetActive (false);
	}
}
