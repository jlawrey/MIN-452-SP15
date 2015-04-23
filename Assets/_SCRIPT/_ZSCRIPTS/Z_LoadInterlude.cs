using UnityEngine;
using System.Collections;

public class Z_LoadInterlude : MonoBehaviour {

	public static string to_scene = "Main";


	// Use this for initialization
	void Start () {
		StartCoroutine(LoadScene (to_scene));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator LoadScene(string scene){

		GameObject cursorcam = GameObject.FindGameObjectWithTag ("CursorCam");
		if (cursorcam != null) {
			Destroy(cursorcam);
		}
		yield return new WaitForSeconds (2);
		Application.LoadLevel (scene);
	}
}
