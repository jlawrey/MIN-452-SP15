using UnityEngine;
using System.Collections;

public class Z_Restart : MonoBehaviour {

	public float waittime = 6;


	// Use this for initialization
	void Start () {

		StartCoroutine(returnToGame(waittime));
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator returnToGame(float timer){

		yield return new WaitForSeconds (timer);
		Application.LoadLevel ("MainMenuAlt");

	}
}
